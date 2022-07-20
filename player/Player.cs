using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public int moveSpeed = 250;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Player ready!");
    }

    public override void _PhysicsProcess(float delta) {
        Vector2 motion = new Vector2();
        motion.x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        motion.y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");
        MoveAndCollide(motion.Normalized() * moveSpeed * delta);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
