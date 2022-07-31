using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : KinematicBody2D
{
    [Signal]
    public delegate void collectedPrize(string playerName);

    [Signal]
    public delegate void playerDied(string playerName);

    public bool isDead = false;
    public bool isReadyToRespawn;

    public int moveSpeed = 50;
    private int _moveSpeedLimit = 320;
    private int _health = 100;

    protected AnimationTree _animTree;
    protected AnimationNodeStateMachinePlayback _animStateMachine;
    protected Vector2 _movement = new Vector2();
    protected Vector2 _direction = new Vector2();

    private const string _BombResource = "res://Nodes/Bomb.tscn";
    private PackedScene _packedSceneBomb;

    public int amountOfBombs = 1;
    public int bombPowerUp = 0;
    public int flamePowerUp = 0;
    public int speedPowerUpValue = 20; // How many units should add per powerUp
    public int flamePowerUpValue = 1;
    public int bombPowerUpValue = 1;
    private bool isJustLoaded = true;

    protected bool _isInvincible = true;
    protected AnimatedSprite _sprite;
    private bool _isFlickerOn = true;

    public bool isImmortal = false; // if set Die() does nothing 
    public bool isCollectPrize = true; // if not set Prize() does nothing

    protected Timer _timer_invincibility;
    private Timer _timer_ready_to_respawn;
    private Timer _timer_collision_with_fire;

    public string name = "default";

    public Color color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public float spawnInvincibilityDuration = 15.0f;


    public void Init(string name)
    {
        this.name = name;
    }

    public void SetColor(Color color)
    {
        _sprite.Modulate = color;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animTree = (AnimationTree)GetNode("AnimationTree");
        _animStateMachine = (AnimationNodeStateMachinePlayback)_animTree.Get("parameters/playback");
        _packedSceneBomb = ResourceLoader.Load<PackedScene>(_BombResource);
        // Regenerate timer
        Timer timer_reload = GetNode<Timer>("Regenerate");
        timer_reload.Connect("timeout", this, "Regenerate");
        // Timer to stop placing bomb right after loading
        Timer timer_is_just_loaded = GetNode<Timer>("JustLoaded");
        timer_is_just_loaded.Connect("timeout", this, "TurnOffIsJustLoaded");
        // Timer collision with fire
        _timer_collision_with_fire = GetNode<Timer>("FireCollision");
        _timer_collision_with_fire.Connect("timeout", this, "TurnOffCollisionWithFire");
        // Invincibility timers
        _timer_invincibility = GetNode<Timer>("Invincibility");
        _timer_invincibility.Connect("timeout", this, "RemoveInvincibility");
        Timer timer_invincibility_flicker = GetNode<Timer>("InvincibilityFlicker");
        timer_invincibility_flicker.Connect("timeout", this, "Flicker");
        //Respawn Timer
        _timer_ready_to_respawn = GetNode<Timer>("Respawn");
        _timer_ready_to_respawn.Connect("timeout", this, "SetReadyToRespawn");
        _timer_ready_to_respawn.Start(10.0f);
        // 
        _sprite = (AnimatedSprite)this.GetNode("./AnimatedSprite");
        SetColor(color);
        // Apply spawn invincibility 
        ApplyInvincibility(spawnInvincibilityDuration);
    }

    private void TurnOffIsJustLoaded()
    {
        isJustLoaded = false;
    }
    private void TurnOffCollisionWithFire()
    {
        // Remove collision with Flame
        CollisionMask = 1 + 0 + 64 + 128; //  Walls, Flame, Box, Unbreakable wall
    }

    private void Regenerate()
    {
        if (amountOfBombs < (1 + (bombPowerUp * bombPowerUpValue)))
        {
            amountOfBombs++;
        }
        if (_health < 100)
        {
            _health += 20;
        }
    }

    private void Flicker()
    {
        if (_isInvincible)
        {
            if (_isFlickerOn)
            {
                _sprite.Modulate = new Color(_sprite.Modulate.r, _sprite.Modulate.g, _sprite.Modulate.b, 0.5f);
                _isFlickerOn = false;
            }
            else
            {
                _sprite.Modulate = new Color(_sprite.Modulate.r, _sprite.Modulate.g, _sprite.Modulate.b, 1f);
                _isFlickerOn = true;
            }
        }
        else
        {
            if (!_isFlickerOn)
            {
                _isFlickerOn = true;
                _sprite.Modulate = new Color(_sprite.Modulate.r, _sprite.Modulate.g, _sprite.Modulate.b, 1f);
            }
        }
    }

    public void ApplyInvincibility(float duration)
    {
        _isInvincible = true;
        // Set timer to turn off fire collision
        _timer_collision_with_fire.Start(1.5f);
        _timer_invincibility.Start(duration);
    }

    protected void RemoveInvincibility()
    {
        // Add collision with Flame
        CollisionMask = 1 + 4 + 64 + 128; //  Walls, Flame, Box, Unbreakable wall
        _isInvincible = false;
        PlaySound("RemoveInvincibility");
    }

    private void SetReadyToRespawn()
    {
        isReadyToRespawn = true;
    }

    private void Die()
    {
        if (isImmortal) { return; };
        if (isDead) { return; }; // Avoid dying twice
        isDead = true;
        _isInvincible = true;
        isReadyToRespawn = false;
        EmitSignal(nameof(playerDied), name);
        // PlayDeath animation
        PlaySound("Die");
        _timer_ready_to_respawn.Start(10.0f);
    }

    public void Respawn()
    {
        isDead = false;
        ApplyInvincibility(15.0f);
        PlaySound("SoundSpawn");
    }

    protected void HitByFire()
    {
        _health = _health - 90;
        if (!isImmortal && (_health <= 0))
        {
            Die();
        }
        else
        {
            PlaySound("SoundHurt");
            ApplyInvincibility(5.0f);
        }
    }

    private void Prize()
    {
        if (!isCollectPrize) { return; };
        _isInvincible = true;
        GD.Print("Emit collectedPrize");
        EmitSignal("collectedPrize", name);
        isCollectPrize = false; // avoid emitting twice
    }


    private void _on_PlayerArea2D_PickedUpPrize()
    {
        Prize();
    }

    public abstract void PlaySound(string sound);

    private void _on_PlayerArea2D_PickedUpPowerUp(string typeOfPowerUp)
    {
        switch (typeOfPowerUp)
        {
            case "Powerup_bomb":
                amountOfBombs++;
                bombPowerUp++;
                break;
            case "Powerup_flame":
                flamePowerUp++;
                break;
            case "Powerup_speed":
                moveSpeed = Math.Min(
                    moveSpeed + speedPowerUpValue,
                    _moveSpeedLimit
                );
                break;
            case "Powerup_shield":
                ApplyInvincibility(10.0f);
                break;
            default:
                break;
        }
        PlaySound(typeOfPowerUp);
    }

    // TODO:Send signal to game manager to spawn bombs
    protected bool _TryPlaceBomb()
    {
        if (amountOfBombs <= 0) { return false; };
        if (isJustLoaded) { return false; };
        Bomb newBomb = _packedSceneBomb.Instance() as Bomb;
        newBomb.Init(1 + (flamePowerUp * flamePowerUpValue));
        // Change position to center
        Vector2 centeredPosition = new Vector2();
        centeredPosition.x = (float)(Math.Round(Position.x / 32) * 32);
        centeredPosition.y = (float)(Math.Round(Position.y / 32) * 32);
        // Check if there already is a Bomb on center position
        List<Bomb> bombs = GetTree().Root.GetChildren().OfType<Bomb>().ToList();
        foreach (Bomb bomb in bombs)
        {
            if (bomb.Position == centeredPosition)
            {
                return false;
            }
        }
        newBomb.Position = centeredPosition;
        GetTree().Root.AddChild(newBomb);
        amountOfBombs--;
        return true;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (isDead) { return; }
        _movement.x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        _movement.y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");
        _direction = _movement.Normalized();
        var possibleCollision = MoveAndCollide(_direction * moveSpeed * delta);

        // Update animation tree based on direction
        string animStateAnimation = _direction == Vector2.Zero ? "Idle" : "Walk";
        _animStateMachine.Travel(animStateAnimation);
        if (animStateAnimation == "Walk")
        {
            _animTree.Set("parameters/Idle/blend_position", _direction);
            _animTree.Set("parameters/Walk/blend_position", _direction);
        }

        // Check collisions
        if (possibleCollision != null)
        {
            KinematicCollision2D collision = (KinematicCollision2D)possibleCollision;
            Node collider = (Node)collision.Collider;
            if (collider.Name.StartsWith("@Flame"))
            {
                if (!(_isInvincible))
                {
                    HitByFire();
                }
            }
        }

        // Try place bomb if key pressed or held down
        if (Input.IsActionPressed("place_bomb"))
        {
            _TryPlaceBomb();
        }
    }
}
