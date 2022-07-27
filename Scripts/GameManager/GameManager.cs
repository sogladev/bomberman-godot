using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameManager : SimpleStateMachine
{
    [Signal]
    public delegate void GuiInput(InputEvent e);

    public AudioManager audioManager;
    public override void _Ready()
    {
        base._Ready();
        List<GameManagerState> gameManageStates = GetNode<Node>("States").GetChildren().OfType<GameManagerState>().ToList();
        audioManager = GetNode<AudioManager>("./AudioManager");

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
            new Color("#ffffff"), // white
            new Color("#e6194B"), // red
            new Color("#f58231"), // orange
            new Color("#ffe119"), // yellow
            new Color("#3cb44b"), // green
            new Color("#4363d8"), // blue
            new Color("#911eb4"), // purple
            new Color("#f032e6"), // magenta
            new Color("#aaffc3"), // mint
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
