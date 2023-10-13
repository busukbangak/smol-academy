using Globals;
using Godot;
using System;

public class EntityOverlay : Control
{
    private Entity _currentEntity;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {


        if (_currentEntity == null) return;

        if (_currentEntity.Health <= 0)
        {
            Visible = false;
            _currentEntity = null;
            return;
        }



        GetNode<Label>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/AttackDamageContainer/Label").Text = _currentEntity.AttackDamage.ToString();
        GetNode<Label>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/AttackRangeContainer/Label").Text = _currentEntity.AttackRange.ToString();
        GetNode<Label>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/AttackSpeedContainer/Label").Text = _currentEntity.AttackSpeed.ToString();


        if (_currentEntity is Character)
        {
            GetNode<HBoxContainer>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/MovementSpeedContainer").Visible = true;

            if (_currentEntity is Minion minion)
            {
                GetNode<Label>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/MovementSpeedContainer/Label").Text = minion.MoveSpeed.ToString();
            }
            else if (_currentEntity is CannonMinion cannonMinion)
            {
                GetNode<Label>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/MovementSpeedContainer/Label").Text = cannonMinion.MoveSpeed.ToString();
            }

        }
        else
        {
            GetNode<HBoxContainer>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/MovementSpeedContainer").Visible = false;
        }

        GetNode<ProgressBar>("MarginContainer/HBoxContainer/Avatar/VBoxContainer/Healthbar").MaxValue = _currentEntity.MaxHealth;
        GetNode<ProgressBar>("MarginContainer/HBoxContainer/Avatar/VBoxContainer/Healthbar").Value = _currentEntity.Health;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("select"))
        {
            if (CursorManager.Instance.IsHovereringEntity)
            {
                if (CursorManager.Instance.HoveredObject["collider"] is Smol)
                {
                    _currentEntity = null;
                    Visible = false;
                }
                else
                {
                    _currentEntity = (Entity)CursorManager.Instance.HoveredObject["collider"];
                    Visible = true;
                }
            }
            else
            {
                _currentEntity = null;
                Visible = false;
            }
        }

        if (Input.IsActionJustPressed("target"))
        {
            if (CursorManager.Instance.IsHovereringEntity)
            {
                if (!(CursorManager.Instance.HoveredObject["collider"] is Smol) && (CursorManager.Instance.HoveredObject["collider"] as Entity).AssignedTeam != PlayerData.Player.AssignedTeam)
                {
                    _currentEntity = (Entity)CursorManager.Instance.HoveredObject["collider"];
                    Visible = true;
                }
            }
        }
    }

}
