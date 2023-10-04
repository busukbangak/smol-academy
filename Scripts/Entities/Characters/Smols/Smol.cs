using Godot;
using System;
using System.Collections;

public enum SmolState
{
	Idle,
	Move,
	Talk,
	Laugh,
	Dance,
	Engage,
	Attack,
	Dead,
	AbilityOne,
	AbilityTwo,
	AbilityThree,
	AbilityFour
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

		var i = 0;
		foreach (var packedAbility in PackedAbilities)
		{
			Abilities[i] = packedAbility.Instance<Ability>();
			Abilities[i].Init(this);
			i++;
		}

		ChangeState(SmolState.Idle);

		DebugManager.Add("Smol state", this, "CurrentSmolStateToString", true);
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
			case SmolState.AbilityOne: AbilityOne(); break;
			case SmolState.AbilityTwo: AbilityTwo(); break;
			case SmolState.AbilityThree: AbilityThree(); break;
			case SmolState.AbilityFour: AbilityFour(); break;
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
				UpdateNavigationPath((Vector3)Utilities.MouseRaycast(GetViewport().GetCamera())["position"]);

				// TODO: Add better visual
				CSGSphere sphere = new CSGSphere();
				sphere.Radius = 0.5f;
				GetParent().AddChild(sphere);
				sphere.GlobalTranslation = (Vector3)Utilities.MouseRaycast(GetViewport().GetCamera())["position"];
				Timer timer = new Timer() { WaitTime = 0.15f, OneShot = true };
				timer.Connect("timeout", this, nameof(OnMoveVisualTimerTimeout), new Godot.Collections.Array(sphere));
				sphere.AddChild(timer);
				timer.Start();

				PlayAnimation("Robot_Running_Loop");
				break;
			case SmolState.Talk:
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
			case SmolState.AbilityOne:
				Abilities[0].Cast();
				break;
			case SmolState.AbilityTwo:
				Abilities[1].Cast();
				break;
			case SmolState.AbilityThree:
				Abilities[2].Cast();
				break;
			case SmolState.AbilityFour:
				Abilities[3].Cast();
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
			case SmolState.AbilityOne:
			case SmolState.AbilityTwo:
			case SmolState.AbilityThree:
			case SmolState.AbilityFour:
				UpdateNavigationPath();
				break;
		}
	}

	public void PushState(SmolState smolState)
	{
		SmolStateStack.Push(CurrentSmolState);
		SmolStateStack.Push(smolState);

		CurrentSmolState = smolState;
		EnterState(CurrentSmolState);
	}

	public void PopState()
	{
		SmolStateStack.Pop();
		ExitState(CurrentSmolState);
		CurrentSmolState = (SmolState)SmolStateStack.Pop();
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

		if (Input.IsActionJustPressed("ability_one"))
		{
			PushState(SmolState.AbilityOne);
			return;
		}

		if (Input.IsActionJustPressed("ability_two"))
		{
			PushState(SmolState.AbilityTwo);
			return;
		}

		if (Input.IsActionJustPressed("ability_three"))
		{
			PushState(SmolState.AbilityThree);
			return;
		}

		if (Input.IsActionJustPressed("ability_four"))
		{
			PushState(SmolState.AbilityFour);
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

		if (Input.IsActionJustPressed("ability_one"))
		{
			PushState(SmolState.AbilityOne);
			return;
		}

		if (Input.IsActionJustPressed("ability_two"))
		{
			PushState(SmolState.AbilityTwo);
			return;
		}

		if (Input.IsActionJustPressed("ability_three"))
		{
			PushState(SmolState.AbilityThree);
			return;
		}

		if (Input.IsActionJustPressed("ability_four"))
		{
			PushState(SmolState.AbilityFour);
			return;
		}

		if (Input.IsActionJustPressed("cancel"))
		{
			ChangeState(SmolState.Idle);
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

		if (Input.IsActionJustPressed("ability_one"))
		{
			PushState(SmolState.AbilityOne);
			return;
		}

		if (Input.IsActionJustPressed("ability_two"))
		{
			PushState(SmolState.AbilityTwo);
			return;
		}

		if (Input.IsActionJustPressed("ability_three"))
		{
			PushState(SmolState.AbilityThree);
			return;
		}

		if (Input.IsActionJustPressed("ability_four"))
		{
			PushState(SmolState.AbilityFour);
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

		if (Input.IsActionJustPressed("ability_one"))
		{
			PushState(SmolState.AbilityOne);
			return;
		}

		if (Input.IsActionJustPressed("ability_two"))
		{
			PushState(SmolState.AbilityTwo);
			return;
		}

		if (Input.IsActionJustPressed("ability_three"))
		{
			PushState(SmolState.AbilityThree);
			return;
		}

		if (Input.IsActionJustPressed("ability_four"))
		{
			PushState(SmolState.AbilityFour);
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

		if (Input.IsActionJustPressed("ability_one"))
		{
			PushState(SmolState.AbilityOne);
			return;
		}

		if (Input.IsActionJustPressed("ability_two"))
		{
			PushState(SmolState.AbilityTwo);
			return;
		}

		if (Input.IsActionJustPressed("ability_three"))
		{
			PushState(SmolState.AbilityThree);
			return;
		}

		if (Input.IsActionJustPressed("ability_four"))
		{
			PushState(SmolState.AbilityFour);
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

		if (Input.IsActionJustPressed("ability_one"))
		{
			PushState(SmolState.AbilityOne);
			return;
		}

		if (Input.IsActionJustPressed("ability_two"))
		{
			PushState(SmolState.AbilityTwo);
			return;
		}

		if (Input.IsActionJustPressed("ability_three"))
		{
			PushState(SmolState.AbilityThree);
			return;
		}

		if (Input.IsActionJustPressed("ability_four"))
		{
			PushState(SmolState.AbilityFour);
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

		if (Input.IsActionJustPressed("ability_one"))
		{
			PushState(SmolState.AbilityOne);
			return;
		}

		if (Input.IsActionJustPressed("ability_two"))
		{
			PushState(SmolState.AbilityTwo);
			return;
		}

		if (Input.IsActionJustPressed("ability_three"))
		{
			PushState(SmolState.AbilityThree);
			return;
		}

		if (Input.IsActionJustPressed("ability_four"))
		{
			PushState(SmolState.AbilityFour);
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

	private void AbilityOne()
	{
		PopState();
	}

	private void AbilityTwo()
	{
		PopState();
	}

	private void AbilityThree()
	{
		PopState();
	}

	private void AbilityFour()
	{
		PopState();
	}

	public string CurrentSmolStateToString()
	{
		return CurrentSmolState.ToString();
	}

	private void OnMoveVisualTimerTimeout(CSGSphere sphere)
	{
		sphere.QueueFree();
	}
}
