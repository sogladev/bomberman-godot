using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameOverState : GameManagerState
{

    private GameOverMenu _menu;

    private bool _isVictory;


    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);

        _isVictory = (bool)message["is_victory"];
        _menu.SetTitle(_isVictory ? "You win!" : "You lose!");

        foreach(Control c in GetNode("../../../../Game/CanvasLayerGameOver").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            GD.Print("control ", c.Name);
            c.Show();
        }
    }


    public override void OnExit(string nextState)
    {
        base.OnExit(nextState);

        foreach(Control c in GetNode("../../../../Game/CanvasLayerGameOver").GetChild(0).GetChildren().OfType<Control>().ToList<Control>())
        {
            GD.Print("control ", c.Name);
            c.Hide();
        }
    }


    public override void _Ready(){
        base._Ready();
        _menu = GetNode<GameOverMenu>("../../../../Game/CanvasLayerGameOver/GameOverMenu");
    }

    public void ChangeToMainMenuState()
    {
        GM.ChangeState("MainMenuState"); // Keep settings
    }

    public override void UpdateState(float delta)
    {
        base.OnUpdate();
        if (Input.IsActionJustPressed("ui_down")){
            _menu.handleInput("ui_down");
        }
        else if (Input.IsActionJustPressed("ui_up")){
            _menu.handleInput("ui_up");
        }
        else if (Input.IsActionJustPressed("ui_accept")){
            string action = _menu.ParseSelection();
            if (action == "play_again"){
                ChangeToMainMenuState();
            }
            else if (action == "quit"){
                GetTree().Quit();
            }
        }
    }


}
