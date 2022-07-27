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
        "#ffffff", // white
        "#e6194B", // red
        "#f58231", // orange
        "#ffe119", // yellow
        "#3cb44b", // green
        "#4363d8", // blue
        "#911eb4", // purple
        "#f032e6", // magenta
        "#aaffc3", // mint
        "#ffb5b5", // light pink
        "#ffe478", // ok
        "#ed46e5", // ok
        "#f0e09e", // ok
        "#f2a65e", // ok
        "#ff6b97", // ok
        "#ce0e15", // ok
        "#cfff70", // ok
        "#b837ea", // ok
        "#8fde5d", // ok
        "#83d0cd", // ok
        "#00b6de", // dark blue
        "#00b6de", // ok
        "#28fb43", // greeng
        "#28fb43", // ok
        "#4da6ff", // ok
        "#66ffe3", // ok
        "#322947",
        "#272736", 
        "#323e4f",
        "#3ca370",
        "#3d6e70",
        "#3e2347", 
        "#422445",
        "#43434f",
        "#473b78",
        "#4b5bab",
        "#57294b", 
        "#5a265e",
        "#5e315b",
        "#606070",
        "#73275c",
        "#7e7e8f",
        "#80366b",
        "#8c3f5d",
        "#964253",
        "#b0305c",
        "#ba6156",
        "#bd4882",
        "#c2c2d1",
        "#e36956",
        "#eb564b",
        "#ff9166",
        "#ffb570",
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
