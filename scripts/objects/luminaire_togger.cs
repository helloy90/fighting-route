using Godot;
using System;

public partial class luminaire_togger : Node3D
{

	[Export]
	public int _key;
	private OmniLight3D _light;
	private bool _stateLight = false;
	

	public override void _Ready()
	{
		_light = GetNode<OmniLight3D>("light");
	}

	public override void _Process(double delta)
	{
	}

	public void switchLight(int input_key){
		if(input_key == _key){
			if(_stateLight){
				_stateLight = false;
				_light.LightEnergy = 0.0f;
			} else {
				_stateLight = true;
				_light.LightEnergy = 1.5f;
			}
		}
	}
}
