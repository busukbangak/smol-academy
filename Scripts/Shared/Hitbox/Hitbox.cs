using Godot;
using System;

public class Hitbox : Area
{
    [Export]
    public float Damage;

    public Entity Attacker;

    public virtual void OnHitboxAreaEntered(Area area) { }
}
