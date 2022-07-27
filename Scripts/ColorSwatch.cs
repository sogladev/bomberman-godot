using Godot;
using System;

[Tool]
public class ColorSwatch : Button
{
    public Color color = new Color(1, 1, 1, 1);
    private ColorRect _colorRect;

    public void SetColor(Color _color)
    {
        if (_colorRect == null)
        {
            return;
        }
        _colorRect.Color = _color;
        color = _color;
    }
    public override void _Ready()
    {
        _colorRect = GetNode<ColorRect>("./ColorRect");
        SetColor(color);
    }
}
