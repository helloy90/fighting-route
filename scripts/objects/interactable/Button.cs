using Godot;
using System;

public partial class Button : Area3D, Interactable
{
	[Export]
	private string _name;

	[Export]
	private string _description;

    [Export]
    private string _interfaceText;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public string GetInterfaceText()
    {
        return _interfaceText;
    }

    public string GetDescription()
    {
        return _description;
    }

    public void PlayAnimation(string animationName)
    {
    }

    public void Interact()
    {
        var showInfo = GetNode<Label>("TestInfo");
        showInfo.Text = "Вы нажали кнопку. Молодцы!!!";
    }

    public string GetLabel()
    {
        return _name;
    }

}
