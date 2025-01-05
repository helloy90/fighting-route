using Godot;
using System;

[GlobalClass]
public partial class State : Node
{
	[Signal]
	public delegate void TransitionEventHandler(String newStateName);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void Enter()
	{

	}

	public virtual void Exit()
	{

	}

	public virtual void Update(double delta)
	{

	}

	public virtual void PhysicsUpdate(double delta)
	{

	}


}
