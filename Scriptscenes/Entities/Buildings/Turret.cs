using Godot;
using System;

public enum TurretState
{
    Idle,
    Attack,
    Dead
}

public class Turret : Entity
{

    public TurretState CurrentTurretState;

    public override void _Ready()
    {
        base._Ready();
        ChangeTurretColor(AssignedTeam == TeamColor.Blue ? Colors.SteelBlue : Colors.OrangeRed);
    }

    private void ChangeTurretColor(Color color)
    {
        //TODO: Need to duplicate material and set it, because jsut creating the new one will replace it
        SpatialMaterial spatialMaterial = new SpatialMaterial();
        spatialMaterial.AlbedoColor = color;

        Model.GetNode<CSGMesh>("Turret/Bot").Set("material", spatialMaterial);
    }

    public override void _Process(float delta)
    {
        switch (CurrentTurretState)
        {
            case TurretState.Idle: Idle(); break;
            case TurretState.Attack: Attack(); break;
            case TurretState.Dead: Dead(); break;
        }
    }

    public void ChangeState(TurretState turretState)
    {
        ExitState(CurrentTurretState);
        CurrentTurretState = turretState;
        EnterState(CurrentTurretState);
    }

    public void EnterState(TurretState turretState)
    {
        switch (turretState)
        {
            case TurretState.Idle:
                break;
            case TurretState.Attack:
                break;
            case TurretState.Dead:
                PlayAnimation("Turret_Destroyed");
                OnDead();
                break;
        }
    }

    public void ExitState(TurretState turretState)
    {
        switch (turretState)
        {
            case TurretState.Idle:
                break;
            case TurretState.Attack:
                break;
            case TurretState.Dead:
                break;
        }
    }

    public void Idle()
    {
        if (Health <= 0)
        {
            ChangeState(TurretState.Dead);
            return;
        }
    }

    public void Attack()
    {

    }

    public void Dead()
    {

    }
}