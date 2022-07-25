using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Spawns : Node2D
{
    private List<Area2D> _spawnAreas;
    private Random _random;
    private int _randIndex = 0;
    public int AmountOfSpawnAreas { get; private set; }

    public override void _Ready()
    {
            _random = new Random();
            _spawnAreas = GetChildren().OfType<Area2D>().ToList();
            Shuffle();
            AmountOfSpawnAreas = _spawnAreas.Count;
    }

    private void Shuffle(){
        _spawnAreas = _spawnAreas.OrderBy(item => _random.Next()).ToList();
    }

    private bool isAreaClearOfPlayers(Area2D areaToCheck){
        // Does not work well for inital spawns.
        // Limit calling to <= amount of Spawns when setting up map
        return areaToCheck.GetOverlappingAreas().Count <= 0;
    }

    private Area2D NextSpawnArea(){
        if (_randIndex >= AmountOfSpawnAreas){
            Shuffle();
            _randIndex = 0;
        }
        return _spawnAreas[_randIndex++];
    }

    public Vector2 nextValidSpawnPoint(int failCounter = 0)
    {
        if (failCounter > 100)
        {
            // Infinite recursive loop catch
            throw new ArgumentOutOfRangeException("Too many tries to find spawn point!");
        }
        Area2D areaToCheck = NextSpawnArea();
        if (isAreaClearOfPlayers(areaToCheck))
        {
            return areaToCheck.GetNode<Sprite>("Spawn").GlobalPosition;
        }
        else
        {
            return nextValidSpawnPoint(failCounter + 1);
        }
    }
}
