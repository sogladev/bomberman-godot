using Godot;
using System;

public class PlayerArea2D : Area2D
{
    [Signal]
     public delegate void PickedUpPowerUp(string typeOfPowerUp);

    [Signal]
     public delegate void PickedUpPrize();

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        var overlappingAreas = GetOverlappingAreas();
        if (overlappingAreas.Count > 0){
            foreach(Area2D area in overlappingAreas){
                if (area is PowerUp){
                    string typeOfPowerUp = ((PowerUp)area).typeOfPowerUp;
                    EmitSignal(nameof(PickedUpPowerUp), typeOfPowerUp);
                }
                else {
                    EmitSignal(nameof(PickedUpPrize));
                }
            }
        }
    }
}
