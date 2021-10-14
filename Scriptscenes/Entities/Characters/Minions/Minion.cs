using Godot;
using System;

public enum MinionState
{
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

        ChangeState(MinionState.Move);
    }

    public override void _Process(float delta)
    {
        switch (CurrentMinionState)
        {
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
            case MinionState.Move:
                AttackTarget = null;
                var rng = new RandomNumberGenerator();
                UpdateNavigationPath(new Vector3(0, 0, 0));
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

        /*   if (Input.IsActionJustPressed("target"))
          {
              var hit = Utilities.MouseRaycast(GetViewport().GetCamera());
              if (hit.Count > 0)
              {
                  if (hit["collider"] is Entity entity && entity.Health > 0 && entity != this)
                  {
                      AttackTarget = entity;
                      if (GlobalTransform.origin.DistanceTo(AttackTarget.GlobalTransform.origin) < AttackRange)
                      {
                          ChangeState(MinionState.Attack);
                      }
                      else
                      {
                          ChangeState(MinionState.Engage);
                      }
                  }
                  else
                  {
                      ChangeState(MinionState.Move);
                  }
              }
              return;
          }
   */
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

        /* if (Input.IsActionJustPressed("target"))
        {
            var hit = Utilities.MouseRaycast(GetViewport().GetCamera());
            if (hit.Count > 0)
            {
                if (hit["collider"] is Entity entity && entity.Health > 0 && entity != this)
                {
                    AttackTarget = entity;
                    if (GlobalTransform.origin.DistanceTo(AttackTarget.GlobalTransform.origin) < AttackRange)
                    {
                        ChangeState(MinionState.Attack);
                    }
                    else
                    {
                        ChangeState(MinionState.Engage);
                    }
                }
                else
                {
                    ChangeState(MinionState.Move);
                }
            }
            return;
        }

        */
    }

    private void Attack()
    {

        if (Health <= 0)
        {
            ChangeState(MinionState.Dead);
            return;
        }

        // Incase of buffs during attack
        UpdateAnimationPlaybackSpeed(AttackSpeed);

        if (AttackTarget == null)
        {
            ChangeState(MinionState.Move);
            return;
        }
        /* 
                if (Input.IsActionJustPressed("target"))
                {
                    var hit = Utilities.MouseRaycast(GetViewport().GetCamera());
                    if (hit.Count > 0)
                    {
                        if (hit["collider"] is Entity entity && entity.Health > 0 && entity != this)
                        {
                            AttackTarget = entity;
                            if (GlobalTransform.origin.DistanceTo(AttackTarget.GlobalTransform.origin) < AttackRange)
                            {
                                ChangeState(MinionState.Attack);
                            }
                            else
                            {
                                ChangeState(MinionState.Engage);
                            }
                        }
                        else
                        {
                            ChangeState(MinionState.Move);
                        }
                    }
                    return;
                } */


    }

    private void Dead()
    {
        GlobalTranslate(new Vector3(0, -1 * GetProcessDeltaTime(), 0));
    }
}
