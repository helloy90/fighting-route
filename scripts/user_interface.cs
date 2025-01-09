using Godot;
using System;

public partial class user_interface : Control
{

	private Label _interactionLabel;
	private Label _typeWeaponLabel;
	private ProgressBar _hp;
	private Label _damageLabel;
	private Label _promptLabel;


	public override void _Ready()
	{
		_typeWeaponLabel = GetNode("main_info_panel").GetNode("weapon_panel").GetNode<Label>("type_weapon");
		_damageLabel = GetNode("main_info_panel").GetNode("damage_panel").GetNode<Label>("value");
		_hp = GetNode("main_info_panel").GetNode("hp_panel").GetNode<ProgressBar>("hp");

		_interactionLabel = GetNode("interaction_panel").GetNode<Label>("interaction");
		_promptLabel = GetNode("prompt_panel").GetNode<Label>("prompt");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void outputInfoInteractableObj(string type, string label, string description, string prompt){
		_interactionLabel.Text = label;
		_promptLabel.Text = prompt;
	}
}
