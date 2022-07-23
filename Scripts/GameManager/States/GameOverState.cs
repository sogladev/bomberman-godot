using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameOverState : GameManagerState
{
    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
    }

    public override void OnExit(string nextState){
        base.OnExit(nextState);
    }
}
