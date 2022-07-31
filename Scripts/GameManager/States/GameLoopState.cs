using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameLoopState : GameManagerState
{
    private PackedScene _packedSceneGame;
    private PackedScene _packedScenePlayer;
    private PackedScene _packedSceneEnemy;
    private PackedScene _packedScenePrize;

    private List<Character> charactersInTheGame;

    private string _specialType;

    private List<string> _playerNames;
    private List<Color> _playerColors;

    private Player _player;
    private Node2D _newGame;
    Timer waitBeforeEndGame;
    private bool isVictory;

    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
        GM.audioManager.PlayMusic("battle1");
        _packedSceneGame = ResourceLoader.Load<PackedScene>((string)message["map_resource"]);
        _packedScenePlayer = ResourceLoader.Load<PackedScene>((string)message["player_resource"]);
        _packedSceneEnemy = ResourceLoader.Load<PackedScene>((string)message["enemy_resource"]);
        _packedScenePrize = ResourceLoader.Load<PackedScene>((string)message["prize_resource"]);
        _playerNames = (List<string>)message["player_names"];
        _playerColors = (List<Color>)message["player_colors"];
        _specialType = (string)message["special_type"];
        // Create Map
        charactersInTheGame = new List<Character>();
        _newGame = _packedSceneGame.Instance() as Node2D;
        GetTree().Root.AddChild(_newGame);
        // Spawn Player 
        _player = _packedScenePlayer.Instance() as Player;
        _player.Init(_playerNames[0]);
        // Set player if special state
        if (_specialType == "immortal")
        {
            _player.isImmortal = true;
        }
        else if (_specialType == "DBG")
        {
            _player.amountOfBombs = 8;
            _player.flamePowerUp = 5;
            _player.bombPowerUp = 10;
            _player.moveSpeed = 200;
            _player.spawnInvincibilityDuration = 5.0f;
        }
        _player.color = _playerColors[0];
        _player.Position = _newGame.GetNode<Spawns>("./Spawns").nextValidSpawnPoint();
        GD.Print("Player position generated: ", _player.Position);
        GetTree().Root.AddChild(_player);
        charactersInTheGame.Add(_player);
        // Connect signals: died, prize
        _player.Connect("collectedPrize", this, "collectedPrize");
        _player.Connect("characterDied", this, "playerDied");
        // Spawn 7 PlayerBots
        Enemy bot;
        for (int i = 1; i < 8; i++)
        {
            bot = _packedSceneEnemy.Instance() as Enemy;
            bot.Init(_playerNames[i]);
            bot.color = _playerColors[i];
            bot.Position = _newGame.GetNode<Spawns>("./Spawns").nextValidSpawnPoint();
            bot.Connect("characterDied", this, "enemyDied");
            GetTree().Root.AddChild(bot);
            charactersInTheGame.Add(bot);
        }
        // Respawn timer
        Timer respawn = GetNode<Timer>("Respawn");
        respawn.Connect("timeout", this, "RespawnDeadPlayers");
        waitBeforeEndGame = GetNode<Timer>("WaitBeforeGameOver");
        waitBeforeEndGame.Connect("timeout", this, "EndTheGame");
    }

    private void RespawnDeadPlayers()
    {
        foreach (Character character in charactersInTheGame)
        {
            if (character.isDead && character.isReadyToRespawn)
            {
                character.Position = _newGame.GetNode<Spawns>("./Spawns").nextValidSpawnPoint();
                character.Respawn();
            }
        }
    }

    private void EndTheGame()
    {
        GM.ChangeState("GameOverState",
        new Dictionary<string, object>(){
            {"is_victory", isVictory},
            {"player_colors", _playerColors},
            {"player_names", _playerNames},
    });

    }
    private void collectedPrize(string playerName)
    {
        charactersInTheGame[0].GetNode<AudioStreamPlayer>("./Sounds/SoundCollectPrize").Play();
        GD.Print("Received Signal: prize");
        waitBeforeEndGame.Start(1.0f);
        isVictory = true;
    }
    private void playerDied(string playerName)
    {
        GD.Print("Received Signal: died");
        waitBeforeEndGame.Start(1.0f);
        isVictory = false;
    }
    private void enemyDied(string enemyName)
    {
        GD.Print("Received Signal: botdied");
        foreach (Character character in charactersInTheGame)
        {
            if (enemyName == character.name)
            {
                character.Position = _newGame.GetNode<Spawns>("./SpawnsGraveyard").nextValidSpawnPoint();
            }
        }
    }

    public void ChangeToMainMenuState()
    {
        GM.ChangeState("MainMenuState",
        new Dictionary<string, object>(){
            {"map_resource", "res://Scenes/DBG.tscn"},
            {"player_resource", "res://Nodes/Player.tscn"},
            {"player_colors", _playerColors},
            {"player_names", _playerNames},
            {"enemy_resource", "res://Nodes/Enemy.tscn"},
            {"prize_resource", "res://Nodes/Prize.tscn"},
            {"special_type", "DBG"},
        });
    }

    public override void UpdateState(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            ChangeToMainMenuState();
        }
    }



    public override void OnExit(string nextState)
    {
        base.OnExit(nextState);
        // Remove timer
        GetNode<Timer>("Respawn").Disconnect("timeout", this, "RespawnDeadPlayers");
        waitBeforeEndGame.Disconnect("timeout", this, "EndTheGame");
        // Clean up. Game is node 0, clean up everything else
        var nodes = GetTree().Root.GetChildren();
        for (int i = 1; i < nodes.Count; i++)
        {
            ((Node)nodes[i]).QueueFree();
        }
    }

}
