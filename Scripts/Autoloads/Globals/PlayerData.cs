using Godot;
using System;

namespace Globals
{
    public class PlayerData : Node
    {

        public static TeamColor AssignedTeam = TeamColor.Blue;

        public static PackedScene AssignedSmol = ResourceLoader.Load<PackedScene>("res://Scenes/Entities/Characters/Smols/Smol.tscn");

        public static Smol Player = null;
    }
}

