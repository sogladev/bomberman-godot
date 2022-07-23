using Godot;
using System;
using System.Collections.Generic;

public class Powerup : StaticBody2D
{
    private List<string> _PowerUpTextures = new List<string>{
        "res://Textures/Powerups/BombPowerup.png",
        "res://Textures/Powerups/FlamePowerup.png",
        "res://Textures/Powerups/SpeedPowerup.png",
         };
    private List<string> _PowerUpNames = new List<string>{
        "Powerup_bomb",
        "Powerup_flame",
        "Powerup_speed",
         };

    private bool _isInvincible = true;
    private Sprite _sprite;
    private bool _isFlickerOn = true;
    private bool _isShouldFlicker = false;

    public string typeOfPowerUp;
    public override void _Ready()
    {
        _sprite = (Sprite)this.GetNode("./Sprite");
        // Set random PowerUp type and texture
        Random random = new Random();
        int index = random.Next(_PowerUpTextures.Count);
        typeOfPowerUp = _PowerUpNames[index];
        Texture img = (Texture)GD.Load(_PowerUpTextures[index]);
        _sprite.Texture = img;
        // Setup timers
        Timer expire = GetNode<Timer>("Expire");
        expire.Connect("timeout", this, "Expire");
        Timer timer_about_to_expire = GetNode<Timer>("ExpireAboutTo");
        timer_about_to_expire.Connect("timeout", this, "TurnOnFlickerCloseToExpiration");
        Timer timer_invincibility = GetNode<Timer>("Invincibility");
        timer_invincibility.Connect("timeout", this, "RemoveInvincibility");
        Timer timer_flicker = GetNode<Timer>("Flicker");
        timer_flicker.Connect("timeout", this, "Flicker");
    }


    private void Ignite(){
        if ( _isInvincible ){
            return;
        }
        Expire();
    }

    void _on_PowerUpArea2D_Ignited(){
        Ignite();
    }

    private void Expire()
    {
        QueueFree();
    }

    private void TurnOnFlickerCloseToExpiration(){
        _isShouldFlicker = true;
    }

    private void RemoveInvincibility(){
        _isInvincible = false;
    }

    private void Flicker()
    {
        if (_isShouldFlicker)
        {
            if (_isFlickerOn)
            {
                _sprite.Modulate = new Color(1, 1, 1, 0.3f);
                _isFlickerOn = false;
            }
            else
            {
                _sprite.Modulate = new Color(1, 1, 1, 1f);
                _isFlickerOn = true;
            }
        }
        else
        {
            if (!_isFlickerOn)
            {
                _isFlickerOn = true;
                _sprite.Modulate = new Color(1, 1, 1, 1f);
            }
        }
    }
}
