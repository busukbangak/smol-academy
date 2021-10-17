using Godot;
using System;

public class Nexus : Entity
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
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

        Model.GetNode<CSGMesh>("Nexus/CSGMesh").Set("material", spatialMaterial);
    }

}
