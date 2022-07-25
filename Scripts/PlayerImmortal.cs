using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerImmortal : Player
{

    public override void _Ready()
    {
        base._Ready();
        // Set custom color
        _timer_invincibility.Disconnect("timeout", this, "RemoveInvincibility");
        SetColor(new Color(0.69f, 0.69f, 0.0f, 1));
    }
}
