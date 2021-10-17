using Godot;
using System;

public enum MinionState
{
    Idle,
    Move,
    Engage,
    Attack,
    Dead
}

public class Minion : Character
{

    public MinionState CurrentMinionState;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        ChangeMinionColor(AssignedTeam == TeamColor.Blue ? Colors.SteelBlue : Colors.OrangeRed);
        ChangeState(MinionState.Idle);
    }

    public override void _Process(float delta)
    {
        switch (CurrentMinionState)
        {
            case MinionState.Idle: Idle(); break;
            case MinionState.Move: Move(); break;
            case MinionState.Engage: Engage(); break;
            case MinionState.Attack: Attack(); break;
            case MinionState.Dead: Dead(); break;
        }
    }

    public void ChangeState(MinionState MinionState)
    {
        ExitState(CurrentMinionState);
        CurrentMinionState = MinionState;
        EnterState(CurrentMinionState);
    }

    public async void EnterState(MinionState MinionState)
    {
        switch (CurrentMinionState)
        {
            case MinionState.Idle:
                PlayAnimation("Robot_Idle_Loop");
                break;
            case MinionState.Move:
                AttackTarget = null;
                ResumeCustomPathNavigation();
                PlayAnimation("Robot_Running_Loop");
                break;
            case MinionState.Engage:
                UpdateNavigationPath(AttackTarget.GlobalTransform.origin);
                PlayAnimation("Robot_Running_Loop");
                break;
            case MinionState.Attack:
                StopAnimation();
                PlayAnimation("Robot_Punch_Loop");
                break;
            case MinionState.Dead:
                StopAnimation();
                PlayAnimation("Robot_Dead");
                base.OnDead();
                await ToSignal(GetTree().CreateTimer(2), "timeout");
                QueueFree();
                break;
        }
    }

    public void ExitState(MinionState MinionState)
    {
        switch (CurrentMinionState)
        {
            case MinionState.Idle:
                break;
            case MinionState.Move:
                break;
            case MinionState.Engage:
                break;
            case MinionState.Attack:
                UpdateAnimationPlaybackSpeed(1);
                break;
            case MinionState.Dead:
                break;
        }
    }

    private void Idle()
    {
        if (Health <= 0)
        {
            ChangeState(MinionState.Dead);
            return;
        }

        if (!IsNavigationComplete())
        {
            ChangeState(MinionState.Move);
            return;
        }

        if (EnemyEntitiesInDetectionArea().Count > 0)
        {

            AttackTarget = GetClosestEnemyEntityInDetectionArea();
            ChangeState(MinionState.Attack);
            return;
        }
    }

    private void Move()
    {
        if (Health <= 0)
        {
            ChangeState(MinionState.Dead);
            return;
        }

        if (!IsNavigationComplete())
        {
            Navigate();
        }

        if (EnemyEntitiesInDetectionArea().Count > 0)
        {

            AttackTarget = GetClosestEnemyEntityInDetectionArea();
            ChangeState(MinionState.Attack);
            return;
        }
    }

    private void Engage()
    {
        if (Health <= 0)
        {
            ChangeState(MinionState.Dead);
            return;
        }

        if (!IsNavigationComplete())
        {
            Navigate();
        }

        if (GlobalTransform.origin.DistanceTo(AttackTarget.GlobalTransform.origin) < AttackRange)
        {
            ChangeState(MinionState.Attack);
            return;
        }
    }

    private void Attack()
    {

        if (Health <= 0)
        {
            ChangeState(MinionState.Dead);
            return;
        }

        if (!EntitiesInDetectionArea.Contains(AttackTarget) || AttackTarget.Health <= 0)
        {
            if (!IsNavigationComplete())
            {
                ChangeState(MinionState.Move);
                return;
            }
            else
            {
                ChangeState(MinionState.Idle);
                return;
            }
        }

        // Incase of buffs during attack
        UpdateAnimationPlaybackSpeed(AttackSpeed);
        Model.LookAt(AttackTarget.GlobalTransform.origin, Vector3.Up);


    }

    private void Dead()
    {
        GlobalTranslate(new Vector3(0, -1 * GetProcessDeltaTime(), 0));
    }

    private void ChangeMinionColor(Color color)
    {
        SpatialMaterial spatialMaterial = new SpatialMaterial();
        spatialMaterial.AlbedoColor = color;
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/BoneAttachment2/Head").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/BoneAttachment3/ArmL").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/BoneAttachment4/ShoulderL").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/BoneAttachment5/ArmR").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/BoneAttachment6/ShoulderR").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/BoneAttachment7/LowerLegL").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/BoneAttachment8/LegL").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/BoneAttachment9/LowerLegR").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/BoneAttachment10/LegR").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/BoneAttachment11/Torso").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/HandL").Set("material/0", spatialMaterial);
        Model.GetNode<MeshInstance>("Robot/RobotArmature/Skeleton/HandR").Set("material/0", spatialMaterial);
    }
}
