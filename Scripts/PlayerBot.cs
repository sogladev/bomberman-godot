using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerBot : Player
{
    public bool StateIdle;
    public bool StatePlaceBomb;
    public string StatePlaceMoveDirection;
    // Setup player that can be directed with states
    public override void _Ready()
    {
        base._Ready();
        //Set custom color
        //SetColor(new Color(0.24f,0.68f,0.91f));
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!StateIdle){
            _movement.x = 0;
            _movement.y = 0;
            _direction = _movement.Normalized();
            var possibleCollision = MoveAndCollide(_direction * moveSpeed * delta);

            // Update animation tree based on direction
            string animStateAnimation = _direction == Vector2.Zero ? "Idle" : "Walk";
            _animStateMachine.Travel(animStateAnimation);
            if (animStateAnimation == "Walk"){
                _animTree.Set("parameters/Idle/blend_position", _direction);
                _animTree.Set("parameters/Walk/blend_position", _direction);
            }
        }
        if (StatePlaceBomb){
            PlaceBomb();
        }

    }
}
