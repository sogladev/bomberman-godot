using Godot;
using System;

public class Flame : StaticBody2D
{
    private int _directionalPowerLeft;
    private int _directionalPowerRight;
    private int _directionalPowerDown;
    private int _directionalPowerUp;

    private const string _FlameResource = "res://Nodes/Flame.tscn";
    private PackedScene _packedSceneFlame;


    public void Init(int left, int right, int up, int down)
    {
        _directionalPowerLeft = left;
        _directionalPowerRight = right;
        _directionalPowerUp = up;
        _directionalPowerDown = down;
    }

    public override void _Ready()
    {
        Timer timer_spread = GetNode<Timer>("Spread");
        Timer timer_fizzle = GetNode<Timer>("Fizzle");
        timer_spread.Connect("timeout", this, "Spread");
        timer_fizzle.Connect("timeout", this, "Fizzle");
        _packedSceneFlame = ResourceLoader.Load<PackedScene>(_FlameResource);
    }

    public void Fizzle()
    {
        QueueFree();
    }

    private void _on_FlameArea2D_FlameIgnited()
    {
        Fizzle();
    }

    private void Spread()
    {
        // Spread Left; -x,y
        if (_directionalPowerLeft != 0)
        {
            Flame newFlameLeft = _packedSceneFlame.Instance() as Flame;
            newFlameLeft.Init(left: _directionalPowerLeft - 1, right: 0, down: 0, up: 0);
            Vector2 leftFlamePosition = Position;
            leftFlamePosition.x -= 32;
            newFlameLeft.Position = leftFlamePosition;
            GetTree().Root.AddChild(newFlameLeft);
        }
        // Spread Right; -x,y
        if (_directionalPowerRight != 0)
        {
            Flame newFlameRight = _packedSceneFlame.Instance() as Flame;
            newFlameRight.Init(right: _directionalPowerRight - 1, left: 0, down: 0, up: 0);
            Vector2 rightFlamePosition = Position;
            rightFlamePosition.x += 32;
            newFlameRight.Position = rightFlamePosition;
            GetTree().Root.AddChild(newFlameRight);
        }
        // Spread Up; x,-y
        if (_directionalPowerUp != 0)
        {
            Flame newFlameUp = _packedSceneFlame.Instance() as Flame;
            newFlameUp.Init(up: _directionalPowerUp - 1, left: 0, down: 0, right: 0);
            Vector2 upFlamePosition = Position;
            upFlamePosition.y += 32;
            newFlameUp.Position = upFlamePosition;
            GetTree().Root.AddChild(newFlameUp);
        }
        // Spread Down; x,+y
        if (_directionalPowerDown != 0)
        {
            Flame newFlameDown = _packedSceneFlame.Instance() as Flame;
            newFlameDown.Init(down: _directionalPowerDown - 1, left: 0, up: 0, right: 0);
            Vector2 downFlamePosition = Position;
            downFlamePosition.y -= 32;
            newFlameDown.Position = downFlamePosition;
            GetTree().Root.AddChild(newFlameDown);
        }
    }

}
