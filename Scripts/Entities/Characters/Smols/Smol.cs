using Godot;
using System;
using System.Collections;

public enum SmolState
{
	Idle,
	Move,
	Joke,
	Laugh,
	Dance,
	Engage,
	Attack,
	Dead
}

public class Smol : Character
{

	public SmolState CurrentSmolState;

	public Stack SmolStateStack = new Stack();

	[Export]
	public PackedScene[] PackedAbilities;

	public Ability[] Abilities = new Ability[4];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		var abilityButtonMappingStrings = new Godot.Collections.Array<string> { "ability_one", "ability_two", "ability_three", "ability_four" };
		var i = 0;
		foreach (var packedAbility in PackedAbilities)
		{
			Abilities[i] = packedAbility.Instance<Ability>();
			Abilities[i].Smol = this;
			Abilities[i].AbilityButtonMappingString = abilityButtonMappingStrings[i];
			Abilities[i].Connect(nameof(Ability.AbilitySelected), this, nameof(OnAbilitySelected));
			AddChild(Abilities[i]);
			i++;
		}

		ChangeState(SmolState.Idle);

		DebugManager.Add(nameof(CurrentSmolState), this, nameof(CurrentSmolStateToString), true);
	}

	public override void _Notification(int what)
	{
		// Handle notifications
		if (what == NotificationPredelete)
		{
			DebugManager.Remove(nameof(CurrentSmolState));
		}
	}

	public override void _Process(float delta)
	{
		switch (CurrentSmolState)
		{
			case SmolState.Idle: Idle(); break;
			case SmolState.Move: Move(); break;
			case SmolState.Joke: Joke(); break;
			case SmolState.Laugh: Laugh(); break;
			case SmolState.Dance: Dance(); break;
			case SmolState.Engage: Engage(); break;
			case SmolState.Attack: Attack(); break;
			case SmolState.Dead: Dead(); break;
		}
	}

	public void ChangeState(SmolState smolState)
	{
		ExitState(CurrentSmolState);
		CurrentSmolState = smolState;
		EnterState(CurrentSmolState);
	}

	public void EnterState(SmolState smolState)
	{
		switch (smolState)
		{
			case SmolState.Idle:
				PlayAnimation("Robot_Idle_Loop");
				break;
			case SmolState.Move:
				AttackTarget = null;

				var moveTarget = (Vector3)CursorManager.Instance.MouseRaycast()["position"];
				UpdateNavigationPath(moveTarget);

				// TODO: Add better visual
				CSGSphere sphere = new CSGSphere();
				sphere.Radius = 0.5f;
				GetParent().AddChild(sphere);
				sphere.GlobalTranslation = moveTarget;
				Timer timer = new Timer() { WaitTime = 0.15f, OneShot = true };
				timer.Connect("timeout", this, nameof(OnMoveVisualTimerTimeout), new Godot.Collections.Array(sphere));
				sphere.AddChild(timer);
				timer.Start();

				PlayAnimation("Robot_Running_Loop");
				break;
			case SmolState.Joke:
				StopAnimation();
				PlayAnimation("No");
				break;
			case SmolState.Laugh:
				StopAnimation();
				PlayAnimation("ThumbsUp");
				break;
			case SmolState.Dance:
				StopAnimation();
				PlayAnimation("Robot_Dance");
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

	public void ExitState(SmolState smolState)
	{
		switch (smolState)
		{
			case SmolState.Idle:
				break;
			case SmolState.Move:
				break;
			case SmolState.Joke:
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
			var hit = CursorManager.Instance.MouseRaycast();
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


		if (Input.IsActionJustPressed("joke"))
		{
			ChangeState(SmolState.Joke);
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

		if (Input.IsActionJustPressed("cancel"))
		{
			ChangeState(SmolState.Idle);
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
			var hit = CursorManager.Instance.MouseRaycast();
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


		if (Input.IsActionJustPressed("joke"))
		{
			ChangeState(SmolState.Joke);
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

		if (Input.IsActionJustPressed("cancel"))
		{
			ChangeState(SmolState.Idle);
			return;
		}
	}

	private void Joke()
	{
		if (Health <= 0)
		{
			ChangeState(SmolState.Dead);
			return;
		}

		if (Input.IsActionJustPressed("target"))
		{
			var hit = CursorManager.Instance.MouseRaycast();
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


		if (Input.IsActionJustPressed("joke"))
		{
			ChangeState(SmolState.Joke);
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

		if (Input.IsActionJustPressed("cancel"))
		{
			ChangeState(SmolState.Idle);
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
			var hit = CursorManager.Instance.MouseRaycast();
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


		if (Input.IsActionJustPressed("joke"))
		{
			ChangeState(SmolState.Joke);
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

		if (Input.IsActionJustPressed("cancel"))
		{
			ChangeState(SmolState.Idle);
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
			var hit = CursorManager.Instance.MouseRaycast();
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


		if (Input.IsActionJustPressed("joke"))
		{
			ChangeState(SmolState.Joke);
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

		if (Input.IsActionJustPressed("cancel"))
		{
			ChangeState(SmolState.Idle);
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
			var hit = CursorManager.Instance.MouseRaycast();
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

		if (Input.IsActionJustPressed("joke"))
		{
			ChangeState(SmolState.Joke);
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

		if (Input.IsActionJustPressed("cancel"))
		{
			ChangeState(SmolState.Idle);
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

		if (!EntitiesInDetectionArea.Contains(AttackTarget) || AttackTarget.Health <= 0)
		{

			ChangeState(SmolState.Idle);
			return;
		}

		if (!EntitiesInDetectionArea.Contains(AttackTarget))
		{
			ChangeState(SmolState.Engage);
			return;
		}

		if (Input.IsActionJustPressed("target"))
		{
			var hit = CursorManager.Instance.MouseRaycast();
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


		if (Input.IsActionJustPressed("joke"))
		{
			ChangeState(SmolState.Joke);
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

		if (Input.IsActionJustPressed("cancel"))
		{
			ChangeState(SmolState.Idle);
			return;
		}

		// Incase of buffs during attack
		UpdateAnimationPlaybackSpeed(AttackSpeed);
		Model.LookAt(AttackTarget.GlobalTransform.origin, Vector3.Up);
	}

	private void Dead()
	{
		if (Health > 0)
		{
			ChangeState(SmolState.Idle);
			return;
		}
	}


	public string CurrentSmolStateToString()
	{
		return CurrentSmolState.ToString();
	}

	private void OnMoveVisualTimerTimeout(CSGSphere sphere)
	{
		sphere.QueueFree();
	}

	private void OnAbilitySelected(Ability selectedAbility)
	{
		for (int i = 0; i < Abilities.Length; i++)
		{
			if (Abilities[i] != selectedAbility && Abilities[i].State == AbilityStates.Selected)
			{
				Abilities[i].ChangeState(AbilityStates.Available);
			}
		}
	}
}
