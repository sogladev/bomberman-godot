using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameManager : SimpleStateMachine
{
    [Signal]
    public delegate void GuiInput(InputEvent e);
    public override void _Ready()
    {
        base._Ready();
        List<GameManagerState> gameManageStates = GetNode<Node>("States").GetChildren().OfType<GameManagerState>().ToList();

        foreach(GameManagerState GMS in gameManageStates){
            GMS.GM = this;
        }

        // Hide menus until needed
        foreach(Control c in GetNode("../../Game/CanvasLayerMainMenu").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            c.Hide();
        }
        foreach(Control c in GetNode("../../Game/CanvasLayerGameOver").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            c.Hide();
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
