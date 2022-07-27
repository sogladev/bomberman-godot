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

    private string _specialType;


    private List<string> _playerNames;
    private List<Color> _playerColors;


    private Player _player;
    private Node2D _newGame;

    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
        _packedSceneGame = ResourceLoader.Load<PackedScene>((string)message["map_resource"]);
        _packedScenePlayer = ResourceLoader.Load<PackedScene>((string)message["player_resource"]);
        _packedScenePlayerBot = ResourceLoader.Load<PackedScene>((string)message["bot_resource"]);
        _packedScenePrize = ResourceLoader.Load<PackedScene>((string)message["prize_resource"]);
        _playerNames = (List<string>)message["player_names"];
        _playerColors = (List<Color>)message["player_colors"];
        _specialType = (string)message["special_type"];
        // Create Map
        playersInTheGame = new List<Player>();
        _newGame = _packedSceneGame.Instance() as Node2D;
        GetTree().Root.AddChild(_newGame);
        // Spawn Player 
        _player = _packedScenePlayer.Instance() as Player;
        _player.Init(_playerNames[0]);
        // Set player if special state
        if (_specialType == "immortal"){
            _player.isImmortal = true;
        }
        else if (_specialType == "DBG"){
            _player.amountOfBombs = 8;
            _player.flamePowerUp = 5;
            _player.bombPowerUp = 10;
            _player.moveSpeed = 100;
        }
        _player.color = _playerColors[0];
        _player.Position = _newGame.GetNode<Spawns>("./Spawns").nextValidSpawnPoint();
        GD.Print("Player position generated: ", _player.Position);
        GetTree().Root.AddChild(_player);
        playersInTheGame.Add(_player);
        // Connect signals: died, prize
        _player.Connect("collectedPrize", this, "collectedPrize");
        _player.Connect("playerDied", this, "playerDied");
        // Spawn 7 PlayerBots
        PlayerBot bot;
        for (int i = 1; i < 8; i++)
        {
            bot = _packedScenePlayerBot.Instance() as PlayerBot;
            bot.Init(_playerNames[i]);
            bot.color = _playerColors[i];
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

    public override void UpdateState(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            GM.ChangeState("MainMenuState");
        }
    }


    private void collectedPrize(string playerName)
    {
        GD.Print("Received Signal: prize");
        GM.ChangeState("GameOverState",
        new Dictionary<string, object>(){
            {"is_victory", true},
            {"player_colors", _playerColors},
            {"player_names", _playerNames},
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
