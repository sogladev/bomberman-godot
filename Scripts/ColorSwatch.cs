using Godot;
using System;

[Tool]
public class ColorSwatch : Button
{
    public Color colorInit;
    public override void _Ready()
    {
       GetNode<ColorRect>("./ColorRect").Color = colorInit;
    }
}
