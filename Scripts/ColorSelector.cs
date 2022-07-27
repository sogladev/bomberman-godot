using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ColorSelector : GridContainer
{
    [Signal]
    public delegate void ColorSelected();
    private List<string> colors = new List<string>(){
        "#5e315b",
        "#8c3f5d",
        "#ba6156",
        "#f2a65e",
        "#ffe478",
        "#cfff70",
        "#8fde5d",
        "#3ca370",
        "#3d6e70",
        "#323e4f",
        "#322947",
        "#473b78",
        "#4b5bab",
        "#4da6ff",
        "#66ffe3",
        "#ffffeb",
        "#c2c2d1",
        "#7e7e8f",
        "#606070",
        "#43434f",
        "#272736",
        "#3e2347",
        "#57294b",
        "#964253",
        "#e36956",
        "#ffb570",
        "#ff9166",
        "#eb564b",
        "#b0305c",
        "#73275c",
        "#422445",
        "#5a265e",
        "#80366b",
        "#bd4882",
        "#ff6b97",
        "#ffb5b5",
    };
    public Color color = new Color(1, 1, 1, 1);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ColorSwatch swatch;
        List<ColorSwatch> swatches = GetChildren().OfType<ColorSwatch>().ToList();
        Color color;
        for (int i = 0; i < swatches.Count(); i++)
        {
            swatch = swatches[i];
            color = new Color(colors[i]);
            swatch.SetColor(color);
            swatch.Connect("pressed", this, "_on_ColorSwatch_pressed", new Godot.Collections.Array { color });
        }
    }

    private void _on_ColorSwatch_pressed(Color color)
    {
        GD.Print("Signal received: ColorSwatch_pressed", color);
        this.color = color;
        EmitSignal(nameof(ColorSelected));
        GD.Print("Signal sent: ColorSelected", color);
    }
}
