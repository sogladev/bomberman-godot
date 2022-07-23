using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class MainMenuState : GameManagerState
{
    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
    }

    public override void OnExit(string nextState){
        base.OnExit(nextState);
    }

    public void OnPressedStartButton()
    {
        GM.ChangeState("GameLoopState");
    }
}
