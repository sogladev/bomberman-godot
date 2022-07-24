using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class MainMenuState : GameManagerState
{


    private MainMenu _mainMenu;


    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);

        foreach(Control c in GetNode("../../../../Game/CanvasLayer").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            GD.Print("control ", c.Name);
            c.Show();
        }
    }


    public override void OnExit(string nextState)
    {
        base.OnExit(nextState);

        //_mainMenu.Destroy();
        foreach(Control c in GetNode("../../../../Game/CanvasLayer").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            GD.Print("control ", c.Name);
            c.Hide();
        }
    }


    public override void _Ready(){
        base._Ready();
        _mainMenu = GetNode<MainMenu>("../../../../Game/CanvasLayer/MainMenu");
    }

    public void ChangeToDebugLoopState()
    {
        GM.ChangeState("GameLoopState",
        new Dictionary<string, object>(){
            {"map_resource", "res://Scenes/DBG.tscn"}, 
            {"player_resource", "res://Nodes/Player.tscn"},
            {"isDebug", true},
// TODO: Give color value of player
        });
    }
    public void ChangeToGameLoopState()
    {
        GM.ChangeState("GameLoopState",
        new Dictionary<string, object>(){
            {"map_resource", "res://Scenes/Demo.tscn"},
            {"player_resource", "res://Nodes/Player.tscn"},
            {"isDebug", false},
// TODO: Give color value of player
        });
    }

    public override void UpdateState(float delta)
    {
        base.OnUpdate();
        const int nElements = 4;
        if (Input.IsActionJustPressed("ui_down")){
            GD.Print("down");
            _mainMenu.handleInput("ui_down");
        }
        else if (Input.IsActionJustPressed("ui_up")){
            GD.Print("up");
            _mainMenu.handleInput("ui_up");
        }
        else if (Input.IsActionJustPressed("ui_accept")){
            GD.Print("select");
            string action = _mainMenu.ParseSelection();
            if (action == "new_game"){
                ChangeToGameLoopState();
            }
            else if (action == "options"){
                GD.Print("Options");
            }
            else if (action == "dbg"){
                ChangeToDebugLoopState();
            }
            else if (action == "quit"){
                GetTree().Quit();
            }
        }
    }


}
