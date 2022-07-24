using Godot;
using System;

public class MainMenu : MarginContainer
{
    // Called when the node enters the scene tree for the first time.
    private Label selector1;
    private Label selector2;
    private Label selector3;
    private Label selector4;
    private int _currentSelection = 0;

    public override void _Ready()
    {
        // Init select labels
        selector1 = GetNode<Label>("./CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer/HBoxContainer/Selector");
        selector2 = GetNode<Label>("./CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer2/HBoxContainer/Selector");
        selector3 = GetNode<Label>("./CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer3/HBoxContainer/Selector");
        selector4 = GetNode<Label>("./CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer4/HBoxContainer/Selector");
        SetCurrentSelection(_currentSelection);
    }

    public void Destroy(){
        QueueFree();
    }

    public void SetCurrentSelection(int index){
        selector1.Text = "";
        selector2.Text = "";
        selector3.Text = "";
        selector4.Text = "";
        if (_currentSelection == 0){
            selector1.Text = "-";
        }
        else if (_currentSelection == 1){
            selector2.Text = "-";
        }
        else if (_currentSelection == 2){
            selector3.Text = "-";
        }
        else {
            selector4.Text = "-";
        }
    }

    public void handleInput(string ui_action)
    {
        const int nElements = 4;
        if (ui_action ==("ui_down")){
            _currentSelection = (_currentSelection + 1) % nElements;
            SetCurrentSelection(_currentSelection);
        }
        else if (ui_action ==("ui_up")){
            _currentSelection = _currentSelection == 0 ? nElements-1 : _currentSelection - 1; 
            SetCurrentSelection(_currentSelection);
        }
    }
    public string ParseSelection(){
        if (_currentSelection == 0){
            return "new_game";
        }
        else if (_currentSelection == 1){
            return "options";
        }
        else if (_currentSelection == 2){
            return "dbg";
        }
        else if (_currentSelection == 3){
            return "quit";
        }
        else {
            return "invalid";
        }
    }
}
