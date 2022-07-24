using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameLoopState : GameManagerState
{
    private PackedScene _packedSceneGame;
    private PackedScene _packedScenePlayer;

    private bool _isDebug;

    private Player _player;

    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
        _packedSceneGame = ResourceLoader.Load<PackedScene>((string)message["map_resource"]);
        _packedScenePlayer = ResourceLoader.Load<PackedScene>((string)message["player_resource"]);
        _isDebug = ResourceLoader.Load<PackedScene>((bool)message["isDebug"]);
        // Create Map
        Node2D newGame = _packedSceneGame.Instance() as Node2D;
        GetTree().Root.AddChild(newGame);
        // Spawn Player and then listen until... score, dies, prize
        _player = _packedScenePlayer.Instance() as Player;
        _player.Position = new Vector2(-750,-250);
        GetTree().Root.AddChild(_player);
        // Connect signals: died, prize
        GetTree().Root.GetNode<Player>("./Player").Connect("prize", this, "prize");
        GetTree().Root.GetNode<Player>("./Player").Connect("died", this, "died");
    }

    private void died(){
        GD.Print("Received Signal: died");
    }

    private void prize()
    {
        GD.Print("Received Signal: prize");
   }

    public override void OnExit(string nextState)
    {
        base.OnExit(nextState);
    }

}
