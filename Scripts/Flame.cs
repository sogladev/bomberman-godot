using Godot;
using System;

public class Flame : StaticBody2D
{
    private int _directionalPowerLeft;
    private int _directionalPowerRight;
    private int _directionalPowerDown;
    private int _directionalPowerUp;

    public void Init(int left, int right, int up, int down){
        _directionalPowerLeft = left;
        _directionalPowerRight = right;
        _directionalPowerUp = up;
        _directionalPowerDown = down;
    }

    public override void _Ready()
    {
    //    Timer timer_spread = GetNode<Timer>("Spread");
    //    Timer timer_fizzle = GetNode<Timer>("Fizzle");
    //    //timer_spread.Connect("timeout", this, "Spread");
    //    timer_fizzle.Connect("timeout", this, "Fizzle");
    //    //_packedSceneFlame = GD.Load<PackedScene>(_FlameResource);
    }


    private void Fizzle(){
//        QueueFree();
    }

    private void Spread(){
        // Spread Left; -x,y
        //if (_directionalPowerLeft != 0){
        //    GD.Print("New spread");
        //   // Flame newFlameLeft = (Flame)(_packedScene.Instance());
        //   // Vector2 leftFlamePosition = Position;
        //   // leftFlamePosition.x -= 30;
        //   // newFlameLeft.Position = leftFlamePosition;
        //   // GetTree().Root.AddChild(newFlameLeft);
        //}
        //else{
        //    GD.Print("End spread");
        //}
    }
}
