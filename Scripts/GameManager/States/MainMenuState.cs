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

    private PackedScene _packedScenePlayerBot;

    private ColorSelector _colorSelector;

    private List<string> _playerNames = new List<string>(){ // Generate later
        "playerA", "playerB", "playerC", "playerD", "playerE",
        "playerF", "playerG", "playerH", "playerI",
    };
    private List<Color> _playerColors = new List<Color>(){
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
    //TODO: Change to structure to keeps track of index, name, color, score

    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);

        foreach (Control c in GetNode("../../../../Game/CanvasLayerMainMenu").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            GD.Print("control ", c.Name);
            c.Show();
        }
        // Toggle with show options?
        foreach (Control c in GetNode("../../../../Game/CanvasLayerColorSelector").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            GD.Print("control ", c.Name);
            c.Show();
        }

        // Connect to colorSelector
        _colorSelector = GetNode<ColorSelector>("../../../../Game/CanvasLayerColorSelector/ColorSelector");
        _colorSelector.Connect("ColorSelected", this, nameof(_on_ColorSelector_ColorSelected));
        // Add menu animation
        _menu = GetNode<MainMenu>("../../../../Game/CanvasLayerMainMenu/MainMenu");
        _packedSceneMenuAnimation = ResourceLoader.Load<PackedScene>(_menuAnimationResource);
        _menuAnimation = _packedSceneMenuAnimation.Instance() as Node2D;
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
            GD.Print("control ", c.Name);
            c.Hide();
        }

        // Toggle with show options?
        foreach (Control c in GetNode("../../../../Game/CanvasLayerColorSelector").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            GD.Print("control ", c.Name);
            c.Hide();
        }

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
            {"special_type", "immortal"},
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
