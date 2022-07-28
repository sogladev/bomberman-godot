using Godot;
using System;

public class MainMenu : MarginContainer
{
    private Label selector1;
    private Label selector2;
    private Label selector3;
    private int _currentSelection = 0;

    public override void _Ready()
    {
        // Init select labels
        selector1 = GetNode<Label>("./CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer/HBoxContainer/Selector");
        selector2 = GetNode<Label>("./CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer1/HBoxContainer/Selector");
        selector3 = GetNode<Label>("./CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer2/HBoxContainer/Selector");
        SetCurrentSelection(_currentSelection);
    }

    public void SetCurrentSelection(int index)
    {
        selector1.Text = "";
        selector2.Text = "";
        selector3.Text = "";
        if (_currentSelection == 0)
        {
            selector1.Text = "-";
        }
        else if (_currentSelection == 1)
        {
            selector2.Text = "-";
        }
        else 
        {
            selector3.Text = "-";
        }
    }

    public void handleInput(string ui_action)
    {
        const int nElements = 3;
        if (ui_action == ("ui_down"))
        {
            _currentSelection = (_currentSelection + 1) % nElements;
            SetCurrentSelection(_currentSelection);
        }
        else if (ui_action == ("ui_up"))
        {
            _currentSelection = _currentSelection == 0 ? nElements - 1 : _currentSelection - 1;
            SetCurrentSelection(_currentSelection);
        }
    }
    public string ParseSelection()
    {
        if (_currentSelection == 0)
        {
            return "new_game";
        }
        else if (_currentSelection == 1)
        {
            return "options";
        }
        else if (_currentSelection == 2)
        {
            return "dbg";
        }
        else
        {
            return "invalid";
        }
    }
}
