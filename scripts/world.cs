using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class world : Node3D
{
	private Node3D _currentLoc;
	
	private Dictionary<string, string> _locations = new Dictionary<string, string>();
	public override void _Ready()
	{
		_locations["main"] = "res://levels/main.tscn";
		var loc = (PackedScene)ResourceLoader.Load(_locations["main"]);
		_currentLoc = (Node3D)loc.Instantiate();
		AddChild(_currentLoc);
		_currentLoc.Visible = true;
		
	}

	public override void _Process(double delta)
	{
	}
}
