using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameLoopState : GameManagerState
{
    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
    }

    public override void OnExit(string nextState){
        base.OnExit(nextState);
    }

    public override void _Ready()
    {
        base._Ready();
    }


    private void on_died(){
        GD.Print("Player died!");
    }
}
