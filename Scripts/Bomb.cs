using Godot;
using System;

public class Bomb : StaticBody2D
{
    private int _power = 5; // Set initial for testing. Ignored for release

    private const string _FlameResource = "res://Nodes/Flame.tscn";
    private PackedScene _packedSceneFlame;

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
    }

    private void SetPlayerCollision(){
        CollisionMask = 2; // Player collision mask
    }

    private void Detonate()
    {
        Flame newFlame = _packedSceneFlame.Instance() as Flame;
        newFlame.Init(_power, _power, _power, _power);
        newFlame.Position = Position;
        GetTree().Root.AddChild(newFlame);
        QueueFree();
    }

    private void _on_BombArea2D_BombIgnited(){
        Detonate();
    }
}
