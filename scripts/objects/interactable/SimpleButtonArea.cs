using Godot;
using System;

public partial class SimpleButtonArea : Area3D, Interactable
{
	[Export]
	private string _label;


	[Export]
	private string _description;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public string GetLabel()
    {
        return _label;
    }

    public string GetDescription()
    {
        return _description;
    }

    public void Interact()
    {
		
    }

    string Interactable.GetTypeObj()
    {
        return "button";
    }
}
