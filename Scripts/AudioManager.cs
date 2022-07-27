using Godot;
using System;

public class AudioManager : Node
{
    public AudioStreamPlayer MusicBackground;
    private string _MusicMainMenuResource = "res://Sounds/main_menu.ogg";
    private string _MusicBattle1Resource = "res://Sounds/battle1.ogg";
    private string _MusicBattle2Resource = "res://Sounds/battle2.ogg";
    private bool isMainMenuMusicPlaying = false;
    public override void _Ready()
    {
        MusicBackground = GetNode<AudioStreamPlayer>("./MusicBackground");
    }

    public void PlayMusic(string music){
        if (music == "battle1"){
            MusicBackground.Stop();
            MusicBackground.Stream = ResourceLoader.Load<AudioStreamOGGVorbis>(_MusicBattle1Resource);
            isMainMenuMusicPlaying = false;
        }
        else if (music == "main_menu"){
            if ( isMainMenuMusicPlaying ) {return;}
            MusicBackground.Stop();
            MusicBackground.Stream = ResourceLoader.Load<AudioStreamOGGVorbis>(_MusicMainMenuResource);
            isMainMenuMusicPlaying = true;
        }
        MusicBackground.Play();
    }
}
