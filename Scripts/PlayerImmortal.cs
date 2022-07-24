using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerImmortal : Player
{

    private void RemoveInvincibility(){
        _isInvincible = true;
    }

    private void HitByFire(){
        _isInvincible = true;
        _timer_invincibility.Start(5.0f);
    }

    public override void _Ready()
    {
        base._Ready();
        // Set custom color
        _sprite.Modulate = new Color(0.69f, 0.69f, 0.0f, 1);
    }
}
