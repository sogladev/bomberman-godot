using Godot;
using System;

public class FlameArea2D : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Signal]
     public delegate void Ignited();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        if (GetOverlappingAreas().Count > 0){
            EmitSignal(nameof(Ignited));
        }
    }
}
