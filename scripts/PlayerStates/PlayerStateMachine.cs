using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerStateMachine : Node
{
	[Export]
	public State currentState;
	private Dictionary<string, State> _states = new() { };
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		foreach (var child in GetChildren())
		{
			if (child is State state)
			{
				_states[child.Name] = state;
				state.Transition += OnChildTransition;
			}
			else
			{
				GD.PushWarning("State machine contains incompatible child node!");
			}
		}
		await ToSignal(Owner, SignalName.Ready);
		currentState.Enter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		currentState.Update(delta);
		// GD.Print("Current state - ", currentState.Name);
	}

	public override void _PhysicsProcess(double delta)
	{
		currentState.PhysicsUpdate(delta);
	}

	private void OnChildTransition(string newStateName)
	{
		var newState = _states[newStateName];
		if (newState != null)
		{
			if (newState != currentState)
			{
				GD.Print(currentState.Name, " - current state, ", newState.Name, " - new state");
				currentState.Exit();
				newState.Enter();
				currentState = newState;
			}
		}
		else
		{
			GD.PushError("State ", newStateName, " does not exit!");
		}
	}
}
