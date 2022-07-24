using Godot;
using System;

public class MainMenu : MarginContainer
{
    // Called when the node enters the scene tree for the first time.
    private Label selector1;
    private Label selector2;
    private Label selector3;
    private int _currentSelection = 0;


    private const string _SceneGameResource = "res://Scenes/Game.tscn";
    private PackedScene _packedSceneGame;
    public override void _Ready()
    {
        // Init select labels
        selector1 = GetNode<Label>("./CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer/HBoxContainer/Selector");
        selector2 = GetNode<Label>("./CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer2/HBoxContainer/Selector");
        selector3 = GetNode<Label>("./CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer3/HBoxContainer/Selector");
        SetCurrentSelection(_currentSelection);
        // Load game scene
        _packedSceneGame = ResourceLoader.Load<PackedScene>(_SceneGameResource);
    }

    public void SetCurrentSelection(int index){
        selector1.Text = "";
        selector2.Text = "";
        selector3.Text = "";
        if (_currentSelection == 0){
            selector1.Text = "-";
        }
        else if (_currentSelection == 1){
            selector2.Text = "-";
        }
        else {
            selector3.Text = "-";
        }
    }

    public override void _Process(float delta)
    {
        const int nElements = 3;
        if (Input.IsActionJustPressed("ui_down")){
            _currentSelection = (_currentSelection + 1) % nElements;
            SetCurrentSelection(_currentSelection);
        }
        else if (Input.IsActionJustPressed("ui_up")){
            _currentSelection = _currentSelection == 0 ? nElements-1 : _currentSelection - 1; 
            SetCurrentSelection(_currentSelection);
        }
        else if (Input.IsActionJustPressed("ui_accept")){
            HandleSelection();
        }
    }
    public void HandleSelection(){
        if (_currentSelection == 0){
            Node2D newGame = _packedSceneGame.Instance() as Node2D;
            GetTree().Root.AddChild(newGame);
            QueueFree();
        }
        else if (_currentSelection == 1){
        }
        else if (_currentSelection == 2){
            GetTree().Quit();
        }
    }
}
