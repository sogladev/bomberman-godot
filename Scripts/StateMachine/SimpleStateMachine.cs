using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class SimpleStateMachine : Node
{
    [Signal] public delegate void PreStart();
    [Signal] public delegate void PostStart();
    [Signal] public delegate void PreExit();
    [Signal] public delegate void PostExit();
    public List<SimpleState> States;
    public string CurrenState;
    public string LastState;
    protected SimpleState state = null;
    public override void _Ready()
    {
        base._Ready();

        States = GetNode<Node>("States").GetChildren().OfType<SimpleState>().ToList();
    }

    private void SetState(SimpleState _state, Dictionary<string, object> message)
    {
        if (_state == null)
        {
            return;
        }
        if (state != null){
            EmitSignal(nameof(PreExit));
            state.OnExit(_state.GetType().ToString());
            EmitSignal(nameof(PostExit));
        }

        LastState = CurrenState;
        CurrenState = _state.GetType().ToString();

        state = _state;
        EmitSignal(nameof(PreStart));
        state.OnStart(message);
        EmitSignal(nameof(PostStart));
        state.OnUpdate();
    }

    public void ChangeState(string stateName, Dictionary<string, object> message = null){
        foreach(SimpleState _state in States){
            if(stateName == _state.GetType().ToString()){
                SetState(_state, message);
                return;
            }
        }
    }

    public override void _PhysicsProcess(float delta){
        if(state == null){
            return;
        }
        state.Updatestate(delta);
    }
}
