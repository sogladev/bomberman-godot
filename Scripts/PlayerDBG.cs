using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerDBG : Player
{
    public override void _Ready()
    {
        base._Ready();
        // Set custom color
        _amountOfBombs = 8;
        flamePowerUp = 5;
        bombPowerUp = 10;
        moveSpeed = 100;
        SetColor(new Color(0.16f, 0.98f, 0.26f, 1));
    }
}
