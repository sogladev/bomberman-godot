using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameLoopState : GameManagerState
{
    private PackedScene _packedSceneGame;
    private PackedScene _packedScenePlayer;
    private PackedScene _packedScenePlayerBot;
    private PackedScene _packedScenePrize;

    private List<Player> playersInTheGame;

    private bool _isDebug;

    private Player _player;
    private Node2D _newGame;

    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
        _packedSceneGame = ResourceLoader.Load<PackedScene>((string)message["map_resource"]);
        _packedScenePlayer = ResourceLoader.Load<PackedScene>((string)message["player_resource"]);
        _packedScenePlayerBot = ResourceLoader.Load<PackedScene>((string)message["bot_resource"]);
        _packedScenePrize = ResourceLoader.Load<PackedScene>((string)message["prize_resource"]);
        _isDebug = (bool)message["is_debug"];
        // Create Map
        playersInTheGame = new List<Player>();
        _newGame = _packedSceneGame.Instance() as Node2D;
        GetTree().Root.AddChild(_newGame);
        // Spawn Player and then listen until... score, dies, prize
        _player = _packedScenePlayer.Instance() as Player;
        _player.Init("playerName");
        _player.Position = _newGame.GetNode<Spawns>("./Spawns").nextValidSpawnPoint();
        GD.Print("Player position generated: ", _player.Position);
        GetTree().Root.AddChild(_player);
        playersInTheGame.Add(_player);
        // Connect signals: died, prize
        _player.Connect("collectedPrize", this, "collectedPrize");
        _player.Connect("playerDied", this, "playerDied");
        // Spawn 7 PlayerBots
        PlayerBot bot;
        for (int i = 0; i < 7; i++)
        {
            bot = _packedScenePlayerBot.Instance() as PlayerBot;
            bot.Init($"Bot{i}");
            bot.Position = _newGame.GetNode<Spawns>("./Spawns").nextValidSpawnPoint();
            bot.Connect("playerDied", this, "botDied");
            GetTree().Root.AddChild(bot);
            playersInTheGame.Add(bot);
        }
        // Spawn Prize
        // Manually spawn for now.
        //Sprite prize = _packedScenePrize.Instance() as Sprite;
        //if (_isDebug)
        //{
        //    prize.Position = new Vector2(-750 + 32 + 4, -250 + 32 + 4);
        //}
        //else
        //{
        //    prize.Position = new Vector2(224 + 4, 448 + 4);
        //}
        //GetTree().Root.AddChild(prize);
        // Respawn timer

        Timer respawn = GetNode<Timer>("Respawn");
        respawn.Connect("timeout", this, "RespawnDeadPlayers");
    }

    private void RespawnDeadPlayers()
    {
            foreach (Player player in playersInTheGame)
            {
                if (player.isDead && player.isReadyToRespawn)
                {
                    player.Position = _newGame.GetNode<Spawns>("./Spawns").nextValidSpawnPoint();
                    player.Respawn();
                }
            }
    }
    private void playerDied(string playerName)
    {
        GD.Print("Received Signal: died");
        GM.ChangeState("GameOverState",
        new Dictionary<string, object>(){
            {"is_victory", false},
    });
    }
    private void botDied(string botName)
    {
        GD.Print("Received Signal: botdied");
        foreach (Player player in playersInTheGame)
        {
            if (botName == player.name)
            {
                player.Position = _newGame.GetNode<Spawns>("./SpawnsGraveyard").nextValidSpawnPoint();
            }
        }
    }


    private void collectedPrize(string playerName)
    {
        GD.Print("Received Signal: prize");
        GM.ChangeState("GameOverState",
        new Dictionary<string, object>(){
            {"is_victory", true},
    });
    }

    public override void OnExit(string nextState)
    {
        base.OnExit(nextState);
        // Remove timer
        GetNode<Timer>("Respawn").Disconnect("timeout", this, "RespawnDeadPlayers");
        // Clean up. Game is node 0, clean up everything else
        var nodes = GetTree().Root.GetChildren();
        for (int i = 1; i < nodes.Count; i++)
        {
            ((Node)nodes[i]).QueueFree();
        }
    }

}
