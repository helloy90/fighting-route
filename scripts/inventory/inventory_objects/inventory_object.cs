using Godot;
using System;

public partial class inventory_object : Area3D
{
	// private string type_object = "inventory_object";

	[Export]
	private string label = "";

	[Export]
	private string description = "";

	[Export]
	private Node parent;

	[Export]
	private bool remove_parent;

	[Export]
	private bool remove_area;


	public string GetLabel(){
		return label;
	}

	public string GetDescription(){
		return description;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
