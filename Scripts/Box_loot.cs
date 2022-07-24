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

    private const string _PowerUpResource = "res://Nodes/PowerUp.tscn";
    private PackedScene _packedScenePowerUp;

    public override void _Ready()
    {
        // Set random box texture 
        Random random = new Random();
        int index = random.Next(_BoxTextures.Count);
        Texture img = (Texture)GD.Load(_BoxTextures[index]);
        ((Sprite)this.GetNode("./Sprite")).Texture = img;

        // Load scene to spawn powerUp
        _packedScenePowerUp = ResourceLoader.Load<PackedScene>(_PowerUpResource);
    }


    private void _on_BoxArea2D_BoxIgnited(){
        Ignite();
    }

    private void Ignite(){
        // Spawn powerUp and destroy
        PowerUp newPowerUp = _packedScenePowerUp.Instance() as PowerUp;
        newPowerUp.Position = Position;
        GetTree().Root.AddChild(newPowerUp);
        QueueFree();
    }
}
