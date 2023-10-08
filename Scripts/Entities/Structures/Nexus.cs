using Godot;
using System;

public enum NexusState
{
    Idle,
    Dead
}

public class Nexus : Entity
{

    public NexusState CurrentNexusState;

    public override void _Ready()
    {
        base._Ready();
        ChangeNexusColor(AssignedTeam == TeamColor.Blue ? Colors.SteelBlue : Colors.OrangeRed);
    }

    private void ChangeNexusColor(Color color)
    {
        //TODO: Need to duplicate material and set it, because jsut creating the new one will replace it
        SpatialMaterial spatialMaterial = new SpatialMaterial();
        spatialMaterial.AlbedoColor = color;

        Model.GetNode<CSGMesh>("Nexus/Bot").Set("material", spatialMaterial);
    }

    public override void _Process(float delta)
    {
        switch (CurrentNexusState)
        {
            case NexusState.Idle: Idle(); break;
            case NexusState.Dead: Dead(); break;
        }
    }

    public void ChangeState(NexusState nexusState)
    {
        ExitState(CurrentNexusState);
        CurrentNexusState = nexusState;
        EnterState(CurrentNexusState);
    }

    public void EnterState(NexusState nexusState)
    {
        switch (nexusState)
        {
            case NexusState.Idle:
                break;
            case NexusState.Dead:
                OnDead();
                PlayAnimation("Nexus_Destroyed");
                break;
        }
    }

    public void ExitState(NexusState nexusState)
    {
        switch (nexusState)
        {
            case NexusState.Idle:
                break;
            case NexusState.Dead:
                break;
        }
    }

    public void Idle()
    {
        if (Health <= 0)
        {
            ChangeState(NexusState.Dead);
            return;
        }
    }

    public void Dead()
    {
        return;
    }

}
