using Godot;
using System;

public class Bomb : StaticBody2D
{
    private PackedScene _packedScene;
    private int _power = 5;

    private const string _FlameResource = "res://Flame.tscn";

    private PackedScene _packedSceneFlame; 


    public void Init(int power){
        _power = power;
    }

    public override void _Ready()
    {
        Timer timer = GetNode<Timer>("Detonate");
        timer.Connect("timeout", this, "Detonate");
        _packedSceneFlame = ResourceLoader.Load<PackedScene>(_FlameResource);
    }

    private void Detonate(){
        // Detonate
        Flame newFlame = _packedSceneFlame.Instance() as Flame;
        //newFlame.Init(4,4,4,4);
        //newFlame.Position = Position;
        GetTree().Root.AddChild(newFlame);
        QueueFree();
    }

}
