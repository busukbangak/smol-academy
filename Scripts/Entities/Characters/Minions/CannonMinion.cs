using Godot;
using System;

public enum CannonMinionState
{
    Idle,
    Move,
    Engage,
    Attack,
    Dead
}

public class CannonMinion : Character
{

    public CannonMinionState CurrentCannonMinionState;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        ChangeCannonMinionColor(AssignedTeam == TeamColor.Blue ? Colors.SteelBlue : Colors.OrangeRed);
        ChangeState(CannonMinionState.Idle);
    }

    public override void _Process(float delta)
    {
        switch (CurrentCannonMinionState)
        {
            case CannonMinionState.Idle: Idle(); break;
            case CannonMinionState.Move: Move(); break;
            case CannonMinionState.Engage: Engage(); break;
            case CannonMinionState.Attack: Attack(); break;
            case CannonMinionState.Dead: Dead(); break;
        }
    }

    public void ChangeState(CannonMinionState cannonMinionState)
    {
        ExitState(CurrentCannonMinionState);
        CurrentCannonMinionState = cannonMinionState;
        EnterState(CurrentCannonMinionState);
    }

    public async void EnterState(CannonMinionState cannonMinionState)
    {
        switch (cannonMinionState)
        {
            case CannonMinionState.Idle:
                break;
            case CannonMinionState.Move:
                AttackTarget = null;
                ResumeCustomPathNavigation();
                break;
            case CannonMinionState.Engage:
                UpdateNavigationPath(AttackTarget.GlobalTransform.origin);
                break;
            case CannonMinionState.Attack:
                StopAnimation();
                AttackTimer.Start();
                break;
            case CannonMinionState.Dead:
                StopAnimation();
                base.OnDead();
                await ToSignal(GetTree().CreateTimer(2), "timeout");
                QueueFree();
                break;
        }
    }

    public void ExitState(CannonMinionState cannonMinionState)
    {
        switch (cannonMinionState)
        {
            case CannonMinionState.Idle:
                break;
            case CannonMinionState.Move:
                break;
            case CannonMinionState.Engage:
                break;
            case CannonMinionState.Attack:
                break;
            case CannonMinionState.Dead:
                break;
        }
    }

    private void Idle()
    {
        if (Health <= 0)
        {
            ChangeState(CannonMinionState.Dead);
            return;
        }

        if (EnemyEntitiesInDetectionArea().Count > 0)
        {

            AttackTarget = GetClosestEnemyEntityInDetectionArea();
            ChangeState(CannonMinionState.Attack);
            return;
        }

        if (!IsNavigationComplete())
        {
            ChangeState(CannonMinionState.Move);
            return;
        }
    }

    private void Move()
    {
        if (Health <= 0)
        {
            ChangeState(CannonMinionState.Dead);
            return;
        }

        if (EnemyEntitiesInDetectionArea().Count > 0)
        {
            AttackTarget = GetClosestEnemyEntityInDetectionArea();
            ChangeState(CannonMinionState.Attack);
            return;
        }

        if (!IsNavigationComplete())
        {
            Navigate();
            return;
        }

    }

    private void Engage()
    {
        if (Health <= 0)
        {
            ChangeState(CannonMinionState.Dead);
            return;
        }

        if (!IsNavigationComplete())
        {
            Navigate();
        }

        if (GlobalTransform.origin.DistanceTo(AttackTarget.GlobalTransform.origin) < AttackRange)
        {
            ChangeState(CannonMinionState.Attack);
            return;
        }
    }

    private void Attack()
    {

        if (Health <= 0)
        {
            ChangeState(CannonMinionState.Dead);
            return;
        }

        if (!EntitiesInDetectionArea.Contains(AttackTarget) || AttackTarget.Health <= 0)
        {
            if (!IsNavigationComplete())
            {
                ChangeState(CannonMinionState.Move);
                return;
            }
            else
            {
                ChangeState(CannonMinionState.Idle);
                return;
            }
        }

        if (AttackTimer.IsStopped())
        {
            var projectile = FireProjectile();
            projectile.Translation = GetNode<Position3D>("ShootingAnchor").GlobalTransform.origin;
            AttackTimer.Start();
        }
        
        // Incase of buffs during attack
        UpdateAnimationPlaybackSpeed(AttackSpeed);
        Model.LookAt(AttackTarget.GlobalTransform.origin, Vector3.Up);
    }

    private void Dead()
    {
        GlobalTranslate(new Vector3(0, -1 * GetProcessDeltaTime(), 0));
    }

    private void ChangeCannonMinionColor(Color color)
    {
        SpatialMaterial spatialMaterial = new SpatialMaterial();
        spatialMaterial.AlbedoColor = color;
        Model.GetNode<MeshInstance>("Bot Drone/group579868033").Set("material/0", spatialMaterial);
    }
}
