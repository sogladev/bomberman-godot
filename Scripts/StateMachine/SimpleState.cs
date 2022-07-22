using Godot;
using System;
using System.Collections.Generic;

public class SimpleState : Node{
    private bool hasBeenInitialized = false;
    private bool onUpdateHasFired = false;
    // State Events
    [Signal] public delegate void StateStart();
    [Signal] public delegate void StateUpdated();
    [Signal] public delegate void StateExited();

    public virtual void OnStart(Dictionary<string, object> message){
        EmitSignal(nameof(StateStart));
        hasBeenInitialized = true;
    }

    public virtual void OnUpdate(){
        if (!hasBeenInitialized){
            return;
        }
        EmitSignal(nameof(StateUpdated));
        onUpdateHasFired = true;
    }
}
