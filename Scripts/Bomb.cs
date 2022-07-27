using Godot;
using System;

public class Bomb : StaticBody2D
{
//    [Signal] public delegate void Detonate(Vector2 position);

    private int _power = 5; // Set initial for testing. Ignored for release

    private const string _FlameResource = "res://Nodes/Flame.tscn";
    private PackedScene _packedSceneFlame;
    // Sounds
    protected AudioStreamPlayer2D _soundFuse2D;
    protected AudioStreamPlayer2D _soundDetonate2D;

    public bool isDetonated = false;

    public void Init(int power)
    {
        _power = power;
    }

    public override void _Ready()
    {
        _packedSceneFlame = ResourceLoader.Load<PackedScene>(_FlameResource);
        Timer timer_detonate = GetNode<Timer>("Detonate");
        timer_detonate.Connect("timeout", this, "Detonate");
        Timer timer_PlayerCollision = GetNode<Timer>("PlayerCollision");
        timer_PlayerCollision.Connect("timeout", this, "SetPlayerCollision");
        _soundDetonate2D = GetNode<AudioStreamPlayer2D>("./SoundDetonate2D");
        _soundFuse2D = GetNode<AudioStreamPlayer2D>("./SoundFuse2D");
        _soundFuse2D.Position = Position;
        _soundFuse2D.Play();
    }

    private void SetPlayerCollision(){
        CollisionMask = 2; // Player collision mask
    }

    private void Detonate()
    {
        if (isDetonated){return;}
        Flame newFlame = _packedSceneFlame.Instance() as Flame;
        newFlame.Init(_power, _power, _power, _power);
        newFlame.Position = Position;
        GetTree().Root.AddChild(newFlame);
        _soundDetonate2D.GlobalPosition = Position;
        _soundDetonate2D.Play();
        Position = new Vector2(9999,9999); // hack to move collision offscreen
        isDetonated = true;
    }
    private void _on_SoundDetonate2D_finished(){
        QueueFree();
    }

    private void _on_BombArea2D_BombIgnited(){
        Detonate();
    }
}
