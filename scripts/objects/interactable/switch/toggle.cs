using Godot;
using System;
using System.Diagnostics;

public partial class toggle : Node3D
{
	[Signal]
	public delegate void ToggleEventHandler(int key);

	[Export]
	private int key;

	[Export]
	private string _label;

	[Export]
	private string _description;

	[Export]
	private string _prompt;

	private string _typeObj = "toggle";
	private AnimationPlayer _animation;

	private bool _stateToggle = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animation = GetNode("body").GetNode<AnimationPlayer>("animation");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Switch(){
		EmitSignal(SignalName.Toggle, key);

		if(_stateToggle){
			_stateToggle = false;
			_animation.Play("OFF");
		} else {
			_stateToggle = true;
			_animation.Play("ON");
		}
	}

	public string GetLabel(){
		return _label;
	}

	public string GetDescription(){
		return _description;
	}

	public string GetTypeObj(){
		return _typeObj;
	}

	public string GetPrompt(){
		return _prompt;
	}
}
