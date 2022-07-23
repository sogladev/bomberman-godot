using Godot;
using System;

public class Bomb : StaticBody2D
{
    private int _power = 5;


    public void Init(int power){
        _power = power;
    }

    public override void _Ready()
    {
       // Timer timer = GetNode<Timer>("Detonate");
       // timer.Connect("timeout", this, "Detonate");
    }

    private void Detonate(){
        // Detonate
        QueueFree();
    }

}
