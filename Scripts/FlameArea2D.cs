using Godot;
using System;

public class FlameArea2D : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Signal]
     public delegate void Ignited();

    [Signal]
     public delegate void isNoOverlap();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (GetOverlappingAreas().Count > 0){
            EmitSignal(nameof(Ignited));
        }
        else {
            EmitSignal(nameof(isNoOverlap));
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (GetOverlappingAreas().Count > 0){
            EmitSignal(nameof(Ignited));
        }
    }
}
