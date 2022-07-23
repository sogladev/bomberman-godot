using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameManager : SimpleStateMachine
{
    public override void _Ready()
    {
        base._Ready();
        List<GameManagerState> gameManageStates = GetNode<Node>("States").GetChildren().OfType<GameManagerState>().ToList();

        foreach(GameManagerState GMS in gameManageStates){
            GMS.GM = this;
        }

        ChangeState(gameManageStates[0].Name);

        Connect(nameof(PostExit), this, nameof(StatePostExit));
        Connect(nameof(PreStart), this, nameof(StatePreStart));
    }

    private void StatePostExit(){
        //
    }

    private void StatePreStart(){
        //
    }


}
