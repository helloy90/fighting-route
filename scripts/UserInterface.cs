using Godot;
using System;

public partial class UserInterface : Control
{
	private Player _player;

	private Label _interactionLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_interactionLabel = GetNode<Label>("interaction");
		_player = GetParent().GetChild(0).GetChild(1) as Player;
		_player.Interaction += getInteractionObject;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void getInteractionObject(string type, string label, string description){
		_interactionLabel.Text = type;
	}
}
