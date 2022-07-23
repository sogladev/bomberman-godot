using Godot;
using System;
using System.Collections.Generic;

public class Box_loot : StaticBody2D
{
    private List<string> _BoxTextures = new List<string>{
        "res://Textures/Boxes/box.png",
        "res://Textures/Boxes/boxAlt.png",
        "res://Textures/Boxes/boxEmpty.png",
         };
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Set random box texture 
        Random random = new Random();
        int index = random.Next(_BoxTextures.Count);
        Texture img = (Texture)GD.Load(_BoxTextures[index]);
        ((Sprite)this.GetNode("./Sprite")).SetTexture(img);
    }
}
