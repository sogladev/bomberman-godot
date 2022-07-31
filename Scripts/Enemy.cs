using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{
    public bool StateIdle;
    public bool StatePlaceBomb;
    public string StatePlaceMoveDirection;

    private AudioStreamPlayer2D _soundHurtBot2D;
    private AudioStreamPlayer2D _soundDieBot2D;

    public override void _Ready()
    {
        base._Ready();
        _soundHurtBot2D = GetNode<AudioStreamPlayer2D>("./Sounds/SoundHurtBot2D");
        _soundDieBot2D = GetNode<AudioStreamPlayer2D>("./Sounds/SoundDieBot2D");
    }

    public override void PlaySound(string sound){
        switch (sound)
        {
            case "SoundHurt":
                GetNode<AudioStreamPlayer2D>("./Sounds/SoundHurtBot2D").Play();
                break;
            case "Die":
                GetNode<AudioStreamPlayer2D>("./Sounds/SoundDieBot2D").Play();
                break;
            default:
                break;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!StateIdle){
            _movement.x = 0;
            _movement.y = 0;
            _direction = _movement.Normalized();
            var possibleCollision = MoveAndCollide(_direction * moveSpeed * delta);

            // Update Sound2D position. WorldPos does not change
           _soundHurtBot2D.GlobalPosition = GlobalPosition;
            _soundDieBot2D.GlobalPosition = GlobalPosition;

            // Update animation tree based on direction
            string animStateAnimation = _direction == Vector2.Zero ? "Idle" : "Walk";
            _animStateMachine.Travel(animStateAnimation);
            if (animStateAnimation == "Walk"){
                _animTree.Set("parameters/Idle/blend_position", _direction);
                _animTree.Set("parameters/Walk/blend_position", _direction);
            }

            // Check collisions
            if (possibleCollision != null){
                KinematicCollision2D collision = (KinematicCollision2D)possibleCollision;
                Node collider = (Node)collision.Collider;
                if (collider.Name.StartsWith("@Flame")){
                    if (!(_isInvincible)){
                        HitByFire();
                    }
                }
            }
        }
        if (StatePlaceBomb){
            if (_TryPlaceBomb()){
                // Bomb placed
                StatePlaceBomb = false;
            }
        }

    }
}
