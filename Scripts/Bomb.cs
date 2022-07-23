using Godot;
using System;

public class Bomb : StaticBody2D
{
    private int _power = 5;

    private const string _FlameResource = "res://Nodes/Flame.tscn";
    private PackedScene _packedSceneFlame;

    public void Init(int power)
    {
        _power = power;
    }

    public override void _Ready()
    {
        _packedSceneFlame = ResourceLoader.Load<PackedScene>(_FlameResource);
        Timer timer = GetNode<Timer>("Detonate");
        timer.Connect("timeout", this, "Detonate");
    }

    private void Detonate()
    {
        // Detonate
        Flame newFlame = _packedSceneFlame.Instance() as Flame;
        newFlame.Init(5, 5, 5, 5);
        newFlame.Position = Position;
        GetTree().Root.AddChild(newFlame);
        QueueFree();
    }

}
