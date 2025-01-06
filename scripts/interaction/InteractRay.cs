using Godot;
using System;
using System.Linq;


public partial class InteractRay : RayCast3D
{
	private Label Prompt;

	private Node currentObject;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Prompt = GetNode<Label>("Prompt");
		currentObject = null;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
		
		Prompt.Text = "Нет взаимодействия с инвентарными объектами";

		if(IsColliding()){
			currentObject = GetCollider() as Node;
			//currentObject.GetClass();
			
			if(currentObject.Name.Equals("inventory_object")){
				var inventory_object = currentObject as inventory_object;
				Prompt.Text = "label: " + inventory_object.GetLabel() + "\ndescription: " + inventory_object.GetDescription();
			}
			
		}
    }
}
