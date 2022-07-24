using Godot;
using System;

public class Bomb : StaticBody2D
{
    private int _power = 5; // Used for testing

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
        newFlame.Init(_power, _power, _power, _power);
        newFlame.Position = Position;
        GetTree().Root.AddChild(newFlame);
        QueueFree();
    }

    private void _on_BombArea2D_BombIgnited(){
        Detonate();
    }

}
