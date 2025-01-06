using Godot;
using System;

public partial class lightning_lamp : Node3D
{

	private AnimationPlayer animation;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animation = GetNode("AnimationPlayer") as AnimationPlayer;
		animation.Play("work_lamp");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
