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
        new Color(1, 1, 1, 1), // white
        new Color(0.86f, 0.06f, 0.09f, 1), // red
        new Color(0.95f, 0.93f, 0.15f, 1), // yellow
        //new Color(0.03f, 0.4f, 0.69f, 1), // blue
        new Color(0.04f, 0.96f, 0.89f, 1), // cyan
        new Color(0.16f, 0.98f, 0.26f, 1), // green
        //new Color(0.79f, 0.79f, 0.0f, 1), // gold
        new Color(0.99f, 0.3f, 0.99f, 1), // pink
        new Color(0.67f, 0.84f, 0.89f, 1), // lightblue
        new Color(0.59f, 0.6f, 0.95f, 1), // violet blue
        new Color(0.52f, 1, 0.62f, 1), // mint green
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
