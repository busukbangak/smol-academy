using Godot;
using System;

public enum SmolState
{
    Idle,
    Move,
    Talk,
    Laugh,
    Dance,
    Engage,
    Attack,
    Dead
}

public class Smol : Character
{

    public SmolState CurrentSmolState;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        ChangeState(SmolState.Idle);
    }

    public override void _Process(float delta)
    {
        switch (CurrentSmolState)
        {
            case SmolState.Idle: Idle(); break;
            case SmolState.Move: Move(); break;
            case SmolState.Talk: Talk(); break;
            case SmolState.Laugh: Laugh(); break;
            case SmolState.Dance: Dance(); break;
            case SmolState.Engage: Engage(); break;
            case SmolState.Attack: Attack(); break;
            case SmolState.Dead: Dead(); break;
        }
    }

    public void ChangeState(SmolState SmolState)
    {
        ExitState(CurrentSmolState);
        CurrentSmolState = SmolState;
        EnterState(CurrentSmolState);
    }

    public void EnterState(SmolState SmolState)
    {
        switch (CurrentSmolState)
        {
            case SmolState.Idle:
                PlayAnimation("Robot_Idle_Loop");
                break;
            case SmolState.Move:
                AttackTarget = null;
                UpdateNavigationPath((Vector3)Utilities.MouseRaycast(GetViewport().GetCamera())["position"]);
                PlayAnimation("Robot_Running_Loop");
                break;
            case SmolState.Talk:
                StopAnimation();
                PlayAnimation("Robot_No_Loop");
                break;
            case SmolState.Laugh:
                StopAnimation();
                PlayAnimation("Robot_ThumbsUp_Loop");
                break;
            case SmolState.Dance:
                StopAnimation();
                PlayAnimation("Robot_Dance_Loop");
                break;
            case SmolState.Engage:
                UpdateNavigationPath(AttackTarget.GlobalTransform.origin);
                PlayAnimation("Robot_Running_Loop");
                break;
            case SmolState.Attack:
                StopAnimation();
                PlayAnimation("Robot_Punch_Loop");
                break;
            case SmolState.Dead:
                StopAnimation();
                PlayAnimation("Robot_Dead");
                base.OnDead();
                break;
        }
    }

    public void ExitState(SmolState SmolState)
    {
        switch (CurrentSmolState)
        {
            case SmolState.Idle:
                break;
            case SmolState.Move:
                break;
            case SmolState.Talk:
                break;
            case SmolState.Laugh:
                break;
            case SmolState.Dance:
                break;
            case SmolState.Engage:
                break;
            case SmolState.Attack:
                UpdateAnimationPlaybackSpeed(1);
                break;
            case SmolState.Dead:
                break;
        }
    }

    private void Idle()
    {
        if (Health <= 0)
        {
            ChangeState(SmolState.Dead);
            return;
        }

        if (Input.IsActionJustPressed("target"))
        {
            var hit = Utilities.MouseRaycast(GetViewport().GetCamera());
            if (hit.Count > 0)
            {
                if (hit["collider"] is Entity entity && entity.Health > 0 && entity != this && entity.AssignedTeam != AssignedTeam)
                {
                    AttackTarget = entity;
                    if (EntitiesInDetectionArea.Contains(AttackTarget))
                    {
                        ChangeState(SmolState.Attack);
                    }
                    else
                    {
                        ChangeState(SmolState.Engage);
                    }
                }
                else
                {
                    ChangeState(SmolState.Move);
                }
            }
            return;
        }


        if (Input.IsActionJustPressed("talk"))
        {
            ChangeState(SmolState.Talk);
            return;
        }

        if (Input.IsActionJustPressed("laugh"))
        {
            ChangeState(SmolState.Laugh);
            return;
        }

        if (Input.IsActionJustPressed("dance"))
        {
            ChangeState(SmolState.Dance);
            return;
        }
    }

    private void Move()
    {
        if (Health <= 0)
        {
            ChangeState(SmolState.Dead);
            return;
        }

        if (!IsNavigationComplete())
        {
            Navigate();
        }
        else
        {
            ChangeState(SmolState.Idle);
            return;
        }

        if (Input.IsActionJustPressed("target"))
        {
            var hit = Utilities.MouseRaycast(GetViewport().GetCamera());
            if (hit.Count > 0)
            {
                if (hit["collider"] is Entity entity && entity.Health > 0 && entity != this && entity.AssignedTeam != AssignedTeam)
                {
                    AttackTarget = entity;
                    if (EntitiesInDetectionArea.Contains(AttackTarget))
                    {
                        ChangeState(SmolState.Attack);
                    }
                    else
                    {
                        ChangeState(SmolState.Engage);
                    }
                }
                else
                {
                    ChangeState(SmolState.Move);
                }
            }
            return;
        }


        if (Input.IsActionJustPressed("talk"))
        {
            ChangeState(SmolState.Talk);
            return;
        }

        if (Input.IsActionJustPressed("laugh"))
        {
            ChangeState(SmolState.Laugh);
            return;
        }

        if (Input.IsActionJustPressed("dance"))
        {
            ChangeState(SmolState.Dance);
            return;
        }
    }

    private void Talk()
    {
        if (Health <= 0)
        {
            ChangeState(SmolState.Dead);
            return;
        }

        if (Input.IsActionJustPressed("target"))
        {
            var hit = Utilities.MouseRaycast(GetViewport().GetCamera());
            if (hit.Count > 0)
            {
                if (hit["collider"] is Entity entity && entity.Health > 0 && entity != this && entity.AssignedTeam != AssignedTeam)
                {
                    AttackTarget = entity;
                    if (EntitiesInDetectionArea.Contains(AttackTarget))
                    {
                        ChangeState(SmolState.Attack);
                    }
                    else
                    {
                        ChangeState(SmolState.Engage);
                    }
                }
                else
                {
                    ChangeState(SmolState.Move);
                }
            }
            return;
        }


        if (Input.IsActionJustPressed("talk"))
        {
            ChangeState(SmolState.Talk);
            return;
        }

        if (Input.IsActionJustPressed("laugh"))
        {
            ChangeState(SmolState.Laugh);
            return;
        }

        if (Input.IsActionJustPressed("dance"))
        {
            ChangeState(SmolState.Dance);
            return;
        }
    }

    private void Laugh()
    {
        if (Health <= 0)
        {
            ChangeState(SmolState.Dead);
            return;
        }

        if (Input.IsActionJustPressed("target"))
        {
            var hit = Utilities.MouseRaycast(GetViewport().GetCamera());
            if (hit.Count > 0)
            {
                if (hit["collider"] is Entity entity && entity.Health > 0 && entity != this && entity.AssignedTeam != AssignedTeam)
                {
                    AttackTarget = entity;
                    if (EntitiesInDetectionArea.Contains(AttackTarget))
                    {
                        ChangeState(SmolState.Attack);
                    }
                    else
                    {
                        ChangeState(SmolState.Engage);
                    }
                }
                else
                {
                    ChangeState(SmolState.Move);
                }
            }
            return;
        }


        if (Input.IsActionJustPressed("talk"))
        {
            ChangeState(SmolState.Talk);
            return;
        }

        if (Input.IsActionJustPressed("laugh"))
        {
            ChangeState(SmolState.Laugh);
            return;
        }

        if (Input.IsActionJustPressed("dance"))
        {
            ChangeState(SmolState.Dance);
            return;
        }
    }

    private void Dance()
    {
        if (Health <= 0)
        {
            ChangeState(SmolState.Dead);
            return;
        }

        if (Input.IsActionJustPressed("target"))
        {
            var hit = Utilities.MouseRaycast(GetViewport().GetCamera());
            if (hit.Count > 0)
            {
                if (hit["collider"] is Entity entity && entity.Health > 0 && entity != this && entity.AssignedTeam != AssignedTeam)
                {
                    AttackTarget = entity;
                    if (EntitiesInDetectionArea.Contains(AttackTarget))
                    {
                        ChangeState(SmolState.Attack);
                    }
                    else
                    {
                        ChangeState(SmolState.Engage);
                    }
                }
                else
                {
                    ChangeState(SmolState.Move);
                }
            }
            return;
        }


        if (Input.IsActionJustPressed("talk"))
        {
            ChangeState(SmolState.Talk);
            return;
        }

        if (Input.IsActionJustPressed("laugh"))
        {
            ChangeState(SmolState.Laugh);
            return;
        }

        if (Input.IsActionJustPressed("dance"))
        {
            ChangeState(SmolState.Dance);
            return;
        }
    }

    private void Engage()
    {
        if (AttackTarget.Health <= 0)
        {
            ChangeState(SmolState.Idle);
            return;
        }

        if (Health <= 0)
        {
            ChangeState(SmolState.Dead);
            return;
        }

        if (!IsNavigationComplete())
        {
            Navigate();
        }

        if (EntitiesInDetectionArea.Contains(AttackTarget))
        {
            ChangeState(SmolState.Attack);
            return;
        }

        if (Input.IsActionJustPressed("target"))
        {
            var hit = Utilities.MouseRaycast(GetViewport().GetCamera());
            if (hit.Count > 0)
            {
                if (hit["collider"] is Entity entity && entity.Health > 0 && entity != this && entity.AssignedTeam != AssignedTeam)
                {
                    AttackTarget = entity;
                    if (EntitiesInDetectionArea.Contains(AttackTarget))
                    {
                        ChangeState(SmolState.Attack);
                    }
                    else
                    {
                        ChangeState(SmolState.Engage);
                    }
                }
                else
                {
                    ChangeState(SmolState.Move);
                }
            }
            return;
        }

        if (Input.IsActionJustPressed("talk"))
        {
            ChangeState(SmolState.Talk);
            return;
        }

        if (Input.IsActionJustPressed("laugh"))
        {
            ChangeState(SmolState.Laugh);
            return;
        }

        if (Input.IsActionJustPressed("dance"))
        {
            ChangeState(SmolState.Dance);
            return;
        }

    }

    private void Attack()
    {
        if (Health <= 0)
        {
            ChangeState(SmolState.Dead);
            return;
        }

        // Incase of buffs during attack
        UpdateAnimationPlaybackSpeed(AttackSpeed);

        if (AttackTarget == null)
        {

            ChangeState(SmolState.Idle);
            return;
        }

        if (!EntitiesInDetectionArea.Contains(AttackTarget))
        {
            ChangeState(SmolState.Engage);
        }

        if (Input.IsActionJustPressed("target"))
        {
            var hit = Utilities.MouseRaycast(GetViewport().GetCamera());
            if (hit.Count > 0)
            {
                if (hit["collider"] is Entity entity && entity.Health > 0 && entity != this && entity.AssignedTeam != AssignedTeam)
                {
                    AttackTarget = entity;
                    if (EntitiesInDetectionArea.Contains(AttackTarget))
                    {
                        ChangeState(SmolState.Attack);
                    }
                    else
                    {
                        ChangeState(SmolState.Engage);
                    }
                }
                else
                {
                    ChangeState(SmolState.Move);
                }
            }
            return;
        }


        if (Input.IsActionJustPressed("talk"))
        {
            ChangeState(SmolState.Talk);
            return;
        }

        if (Input.IsActionJustPressed("laugh"))
        {
            ChangeState(SmolState.Laugh);
            return;
        }

        if (Input.IsActionJustPressed("dance"))
        {
            ChangeState(SmolState.Dance);
            return;
        }
    }

    private void Dead()
    {
        if (Health > 0)
        {
            ChangeState(SmolState.Idle);
            return;
        }
    }
}
