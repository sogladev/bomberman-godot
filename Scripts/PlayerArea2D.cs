using Godot;
using System;

public class PlayerArea2D : Area2D
{
    [Signal]
     public delegate void PickedUpPowerUp(string typeOfPowerUp);

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        var overlappingAreas = GetOverlappingAreas();
        if (overlappingAreas.Count > 0){
            foreach(PowerUp powerUp in overlappingAreas){
                string typeOfPowerUp = powerUp.typeOfPowerUp;
                EmitSignal(nameof(PickedUpPowerUp), typeOfPowerUp);
            }
        }
    }
}
