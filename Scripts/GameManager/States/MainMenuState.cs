using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class MainMenuState : GameManagerState
{
    private bool isColorSelectorVisible = false;
    private MainMenu _menu;
    private Node2D _menuAnimation;
    private string _menuAnimationResource = "res://Nodes/Menus/MainMenuAnimation.tscn";

    private PackedScene _packedSceneMenuAnimation;

    private PackedScene _packedScenePlayerBot;

    private ColorSelector _colorSelector;

    private List<string> _playerNames;
    private List<Color> _playerColors;

    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
        _playerNames = (List<string>)message["player_names"];
        _playerColors = (List<Color>)message["player_colors"];

        GM.audioManager.PlayMusic("main_menu");

        foreach (Control c in GetNode("../../../../Game/CanvasLayerMainMenu").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            c.Show();
        }

        // Connect to colorSelector
        _colorSelector = GetNode<ColorSelector>("../../../../Game/CanvasLayerColorSelector/VBoxContainer/ColorSelector");
        _colorSelector.Connect("ColorSelected", this, nameof(_on_ColorSelector_ColorSelected));
        // Add menu animation
        _menu = GetNode<MainMenu>("../../../../Game/CanvasLayerMainMenu/MainMenu");
        _packedSceneMenuAnimation = ResourceLoader.Load<PackedScene>(_menuAnimationResource);
        _menuAnimation = _packedSceneMenuAnimation.Instance() as Node2D;
        _menuAnimation.GetNode<Player>("./Player").color = _playerColors[0];
        AddChild(_menuAnimation);
        // Spawn bots
        _packedScenePlayerBot = ResourceLoader.Load<PackedScene>("res://Nodes/PlayerBot.tscn");
        PlayerBot bot;
        for (int i = 1; i < 9; i++)
        {
            bot = _packedScenePlayerBot.Instance() as PlayerBot;
            bot.Init($"{i}_{_playerNames[i]}");
            bot.color = _playerColors[i];
            bot.Position = _menuAnimation.GetNode<Spawns>("./Spawns").nextValidSpawnPoint();
            //            bot.Connect("playerDied", this, "botDied");
            _menuAnimation.AddChild(bot);
            //          playersInTheGame.Add(bot);
        }
    }


    public override void OnExit(string nextState)
    {
        base.OnExit(nextState);

        foreach (Control c in GetNode("../../../../Game/CanvasLayerMainMenu").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            c.Hide();
        }

        // Toggle with show options?
        foreach (Control c in GetNode("../../../../Game/CanvasLayerColorSelector").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            c.Hide();
        }
        isColorSelectorVisible = false;

        _colorSelector.Disconnect("ColorSelected", this, nameof(_on_ColorSelector_ColorSelected));

        _menuAnimation.QueueFree();
    }

    public void ChangeToDebugLoopState()
    {
        GM.ChangeState("GameLoopState",
        new Dictionary<string, object>(){
            {"map_resource", "res://Scenes/DBG.tscn"},
            {"player_resource", "res://Nodes/Player.tscn"},
            {"player_colors", _playerColors},
            {"player_names", _playerNames},
            {"bot_resource", "res://Nodes/PlayerBot.tscn"},
            {"prize_resource", "res://Nodes/Prize.tscn"},
            {"special_type", "DBG"},
        });
    }
    public void ChangeToGameLoopState()
    {
        GM.ChangeState("GameLoopState",
        new Dictionary<string, object>(){
            {"map_resource", "res://Scenes/Demo.tscn"},
            {"player_resource", "res://Nodes/Player.tscn"},
            {"player_colors", _playerColors},
            {"player_names", _playerNames},
            {"bot_resource", "res://Nodes/PlayerBot.tscn"},
            {"prize_resource", "res://Nodes/Prize.tscn"},
            {"special_type", "none"},
        });
    }


    private void _on_ColorSelector_ColorSelected(){
        GD.Print("MainMenu received signal color");
        _menuAnimation.GetNode<Player>("./Player")
            .SetColor(_colorSelector.color);
        _playerColors[0] = _colorSelector.color;
    }

    public override void UpdateState(float delta)
    {
        base.OnUpdate();
        if (Input.IsActionJustPressed("ui_down"))
        {
            GD.Print("down");
            _menu.handleInput("ui_down");
        }
        else if (Input.IsActionJustPressed("ui_up"))
        {
            GD.Print("up");
            _menu.handleInput("ui_up");
        }
        else if (Input.IsActionJustPressed("ui_accept"))
        {
            GD.Print("select");
            string action = _menu.ParseSelection();
            if (action == "new_game")
            {
                ChangeToGameLoopState();
            }
            else if (action == "options")
            {
                GD.Print("Options");
                // Show ColorSelector
                foreach (Control c in GetNode("../../../../Game/CanvasLayerColorSelector").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
                {
                    if (isColorSelectorVisible) {c.Hide();} else {c.Show();};
                }
                isColorSelectorVisible = !isColorSelectorVisible;
            }
            else if (action == "dbg")
            {
                ChangeToDebugLoopState();
            }
            else if (action == "quit")
            {
                GetTree().Quit();
            }
        }
    }


}
