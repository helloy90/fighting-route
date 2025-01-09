using Godot;
using System;

public partial class toggleInteractable : Area3D, Interactable
{
	private toggle _toggle;
	public override void _Ready()
	{
		_toggle = GetParent() as toggle;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public string GetTypeObj()
    {
        return _toggle.GetTypeObj();
    }

    public string GetLabel()
    {
        return _toggle.GetLabel();
    }

    public string GetDescription()
    {
        return _toggle.GetDescription();
    }

    public void Interact()
    {
        _toggle.Switch();
    }

    public string GetPrompt()
    {
        return _toggle.GetPrompt();
    }
}
