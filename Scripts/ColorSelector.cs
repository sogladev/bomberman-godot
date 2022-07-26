using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class ColorSelector : GridContainer
{
    public Color color = new Color(1,1,1,1);
    private string _ColorSwatchResource = "res://Nodes/Menus/ColorSwatch.tscn";
    private PackedScene _packedSceneColorSwatch;
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


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _packedSceneColorSwatch = ResourceLoader.Load<PackedScene>(_ColorSwatchResource);
        ColorSwatch newColorSwatch;
        foreach (string color in colors){
            newColorSwatch = _packedSceneColorSwatch.Instance() as ColorSwatch;
            newColorSwatch.colorInit = new Color(color);
            AddChild(newColorSwatch);
            newColorSwatch.Owner = GetTree().EditedSceneRoot;
            //newColorSwatch.Connect("ColorSelected", this, "_on_ColorSwatch_pressed");
        }
        //Already connected through UI?
        //foreach (ColorSwatch swatch in GetChildren()){
        //    swatch.Connect("ColorSelected", this, "_on_ColorSwatch_pressed");
        //}
    }

    private void _on_ColorSwatch_pressed(Color color){
        GD.Print("New color selected: ", color);
        this.color = color;
    }
}
