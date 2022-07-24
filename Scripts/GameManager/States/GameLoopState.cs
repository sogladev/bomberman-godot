using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameLoopState : GameManagerState
{
    private PackedScene _packedSceneGame;
    private PackedScene _packedScenePlayer;
    private PackedScene _packedScenePrize;

    private bool _isDebug;

    private Player _player;

    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
        _packedSceneGame = ResourceLoader.Load<PackedScene>((string)message["map_resource"]);
        _packedScenePlayer = ResourceLoader.Load<PackedScene>((string)message["player_resource"]);
        _packedScenePrize = ResourceLoader.Load<PackedScene>((string)message["prize_resource"]);
        _isDebug = (bool)message["is_debug"];
        // Create Map
        Node2D newGame = _packedSceneGame.Instance() as Node2D;
        GetTree().Root.AddChild(newGame);
        // Spawn Player and then listen until... score, dies, prize
        _player = _packedScenePlayer.Instance() as Player;
        if (_isDebug){
            _player.Position = new Vector2(-750,-250);
        }
        else {
            _player.Position = new Vector2(16,16);
        }
        GetTree().Root.AddChild(_player);
        // Connect signals: died, prize
        _player.Connect("prize", this, "prize");
        _player.Connect("died", this, "died");
        // Spawn Prize
        Sprite prize = _packedScenePrize.Instance() as Sprite;
        if (_isDebug){
            prize.Position = new Vector2(-750+32+4,-250+32+4);
        }
        else{
            prize.Position = new Vector2(224+4, 448+4);
        }
        GetTree().Root.AddChild(prize);
    }

    private void died(){
       GD.Print("Received Signal: died");
        GM.ChangeState("GameOverState");
    }

    private void prize()
    {
        GD.Print("Received Signal: prize");
        GM.ChangeState("GameOverState");
   }

    public override void OnExit(string nextState)
    {
        base.OnExit(nextState);
    }

}
