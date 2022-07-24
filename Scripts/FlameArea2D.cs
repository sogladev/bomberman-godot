using Godot;
using System;

public class FlameArea2D : Area2D
{
    [Signal]
     public delegate void FlameIgnited();

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        if (GetOverlappingAreas().Count > 0){
            EmitSignal(nameof(FlameIgnited));
        }
    }
}
