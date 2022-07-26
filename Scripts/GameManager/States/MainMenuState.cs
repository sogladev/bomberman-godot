using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class MainMenuState : GameManagerState
{
    private MainMenu _menu;
    private Node2D _menuAnimation;
    private string _menuAnimationResource = "res://Nodes/Menus/MainMenuAnimation.tscn";

    private PackedScene _packedSceneMenuAnimation;

    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);

        foreach(Control c in GetNode("../../../../Game/CanvasLayerMainMenu").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            GD.Print("control ", c.Name);
            c.Show();
        }

        _menu = GetNode<MainMenu>("../../../../Game/CanvasLayerMainMenu/MainMenu");
        _packedSceneMenuAnimation = ResourceLoader.Load<PackedScene>(_menuAnimationResource);
        _menuAnimation = _packedSceneMenuAnimation.Instance() as Node2D;
        AddChild(_menuAnimation);
    }

    public override void OnExit(string nextState)
    {
        base.OnExit(nextState);

        foreach(Control c in GetNode("../../../../Game/CanvasLayerMainMenu").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            GD.Print("control ", c.Name);
            c.Hide();
        }

        _menuAnimation.QueueFree();
    }

    public void ChangeToDebugLoopState()
    {
        GM.ChangeState("GameLoopState",
        new Dictionary<string, object>(){
            {"map_resource", "res://Scenes/DBG.tscn"}, 
            {"player_resource", "res://Nodes/Player.tscn"},
            {"bot_resource", "res://Nodes/PlayerBot.tscn"},
            {"prize_resource", "res://Nodes/Prize.tscn"},
            {"special_type", "DBG"},
// TODO: Give color value of player
        });
    }
    public void ChangeToGameLoopState()
    {
        GM.ChangeState("GameLoopState",
        new Dictionary<string, object>(){
            {"map_resource", "res://Scenes/Demo.tscn"},
            {"player_resource", "res://Nodes/Player.tscn"},
            {"bot_resource", "res://Nodes/PlayerBot.tscn"},
            {"prize_resource", "res://Nodes/Prize.tscn"},
            {"special_type", "immortal"},
// TODO: Give color value of player
        });
    }

    public override void UpdateState(float delta)
    {
        base.OnUpdate();
        if (Input.IsActionJustPressed("ui_down")){
            GD.Print("down");
            _menu.handleInput("ui_down");
        }
        else if (Input.IsActionJustPressed("ui_up")){
            GD.Print("up");
            _menu.handleInput("ui_up");
        }
        else if (Input.IsActionJustPressed("ui_accept")){
            GD.Print("select");
            string action = _menu.ParseSelection();
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
