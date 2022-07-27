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

        foreach (GameManagerState GMS in gameManageStates)
        {
            GMS.GM = this;
        }

        // Hide menus until needed
        foreach (Control c in GetNode("../../Game/CanvasLayerMainMenu").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            c.Hide();
        }
        foreach (Control c in GetNode("../../Game/CanvasLayerGameOverMenu").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            c.Hide();
        }
        foreach (Control c in GetNode("../../Game/CanvasLayerColorSelector").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            c.Hide();
        }

        // Set default values
        //TODO: Change to structure to keeps track of index, name, color, score
        List<string> _playerNames = new List<string>(){ // Generate later
        "playerA", "playerB", "playerC", "playerD", "playerE",
        "playerF", "playerG", "playerH", "playerI",
        };
        List<Color> _playerColors = new List<Color>(){
            new Color("#ffffff"),
            new Color("#cfff70"),
            new Color("#66ffe3"),
            new Color("#00b6de"),
            new Color("#ffe478"),
            new Color("#ed46e5"),
            new Color("#ff6b97"),
            new Color("#ffb5b5"),
            new Color("#00b6de"),
        };


        ChangeState("MainMenuState",
        new Dictionary<string, object>(){
            {"map_resource", "res://Scenes/DBG.tscn"},
            {"player_resource", "res://Nodes/Player.tscn"},
            {"player_colors", _playerColors},
            {"player_names", _playerNames},
            {"bot_resource", "res://Nodes/PlayerBot.tscn"},
            {"prize_resource", "res://Nodes/Prize.tscn"},
            {"special_type", "DBG"},
        });

        Connect(nameof(PostExit), this, nameof(StatePostExit));
        Connect(nameof(PreStart), this, nameof(StatePreStart));
    }

    private void StatePostExit()
    {
        //
    }

    private void StatePreStart()
    {
        //
    }


}
