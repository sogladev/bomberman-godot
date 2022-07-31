using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Player : Character
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        PlaySound("SoundSpawn");
        GetNode<Listener2D>("./Listener2D").MakeCurrent();
    }

    public override void PlaySound(string sound){
        switch (sound)
        {
            case "RemoveInvincibility":
                GetNode<AudioStreamPlayer>("./Sounds/SoundRemoveInvincibility").Play();
                break;
            case "SoundSpawn":
                GetNode<AudioStreamPlayer>("./Sounds/SoundSpawn").Play();
                break;
            case "SoundHurt":
                GetNode<AudioStreamPlayer>("./Sounds/SoundHurtPlayer").Play();
                break;
            case "Die":
                GetNode<AudioStreamPlayer>("./Sounds/SoundDiePlayer").Play();
                break;
            case "Powerup_bomb":
                GetNode<AudioStreamPlayer>("./Sounds/SoundPowerUpBomb").Play();
                break;
            case "Powerup_flame":
                GetNode<AudioStreamPlayer>("./Sounds/SoundPowerUpFirePower").Play();
                break;
            case "Powerup_speed":
                GetNode<AudioStreamPlayer>("./Sounds/SoundPowerUpSpeed").Play();
                break;
            case "Powerup_shield":
                GetNode<AudioStreamPlayer>("./Sounds/SoundPowerUpShield").Play();
                break;
            default:
                break;
        }
    }
}
