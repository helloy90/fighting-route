using Godot;
using System;
using System.ComponentModel.Design.Serialization;

public partial class Player : CharacterBody3D
{
	[Signal]
	public delegate void InteractionEventHandler(string type, string label, string description, string prompt);

	[Export]
	private float BaseSpeed { get; set; } = 10f;
	[Export]
	private float SprintMultiplier { get; set; } = 1.7f;
	[Export]
	private float Acceleration { get; set; } = 10f;
	[Export]
	private float Deceleration { get; set; } = 10f;
	[Export]
	private float JumpStrength { get; set; } = 4.5f;
	[Export(PropertyHint.Range, "0.1,1.0,0.01")]
	private float InputInJumpMultiplier { get; set; } = 0.5f;
	[Export]
	private float TiltLowerLimitDegrees { get; set; } = -90;
	[Export]
	private float TiltUpperLimitDegrees { get; set; } = 90;

	[Export]
	public float MouseSensitivity { get; set; } = 0.3f;

	[Export]
	private Node3D CameraController;

	// maybe reconsider properties later
	public float CurrentSpeed => BaseSpeed * _currentSpeedMultiplier;
	private float _currentSpeedMultiplier;

	private float _tiltLowerLimit;
	private float _tiltUpperLimit;

	private Vector3 _mainMovement;
	private Vector3 _gravityDirection;
	private float _gravityValue;

	// Rotation and tilt from mouse event
	private Vector2 _rotationInput;
	private Vector3 _mouseRotation;
	private Vector3 _cameraRotation;
	private Vector3 _playerRotation;

	private RayCast3D _detector;
	private bool _send_interaction = false;

	private bool _updateCamera;
	private bool _jumped;
	private bool _isInAir;

	public override void _Ready()
	{
		_currentSpeedMultiplier = 1;

		_tiltLowerLimit = Mathf.DegToRad(TiltLowerLimitDegrees);
		_tiltUpperLimit = Mathf.DegToRad(TiltUpperLimitDegrees);

		_mainMovement = Vector3.Zero;
		_gravityDirection = Vector3.Zero;
		_gravityValue = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

		_rotationInput = Vector2.Zero;
		_mouseRotation = Vector3.Zero;
		_cameraRotation = Vector3.Zero;
		_playerRotation = Vector3.Zero;

		_updateCamera = false;
		_jumped = false;
		_isInAir = false;

		_detector = GetNode("HEAD").GetNode<RayCast3D>("Detector");
	}

	public override void _Process(double delta)
	{
		if (_updateCamera)
		{
			UpdateCamera(delta);
			_updateCamera = false;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		_updateCamera = true;

		interactionProcessing(delta);

		UpdateMovement(delta);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.MouseMode != Input.MouseModeEnum.Captured)
		{
			return;
		}

		if (@event is InputEventMouseMotion mouseMotion)
		{
			_rotationInput = -mouseMotion.Relative * MouseSensitivity;
		}
	}
	private void UpdateCamera(double delta)
	{
		_mouseRotation.X += (float)(_rotationInput.Y * delta);
		_mouseRotation.X = Mathf.Clamp(_mouseRotation.X, _tiltLowerLimit, _tiltUpperLimit);
		_mouseRotation.Y += (float)(_rotationInput.X * delta);

		_playerRotation = Vector3.Zero with { Y = _mouseRotation.Y };

		_cameraRotation = Vector3.Zero with { X = _mouseRotation.X };

		CameraController.Transform = CameraController.Transform with { Basis = Basis.FromEuler(_cameraRotation) };
		GlobalTransform = GlobalTransform with { Basis = Basis.FromEuler(_playerRotation) };
		CameraController.Rotation = CameraController.Rotation with { Z = 0.0f };

		_rotationInput = Vector2.Zero;
	}

	private void UpdateMovement(double delta)
	{
		var input = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		var direction = (Transform.Basis * new Vector3(input.X, 0, input.Y)).Normalized();

		if (IsOnFloor())
		{
			_isInAir = false;
			_jumped = false;
			_gravityDirection = Vector3.Zero;
		}
		else
		{
			_isInAir = true;
			_gravityDirection += Vector3.Down * _gravityValue * (float)delta;
		}

		if (Input.IsActionJustPressed("jump") && !_isInAir)
		{
			_jumped = true;
			_isInAir = true;
			_gravityDirection = Vector3.Up * JumpStrength;
		}

		if (Input.IsActionPressed("sprint") && !_isInAir) {
			_currentSpeedMultiplier = SprintMultiplier;
		}
		if (Input.IsActionJustReleased("sprint")) {
			_currentSpeedMultiplier = 1;
		}

		if (direction.Length() > 1e-9)
		{
			_mainMovement = _mainMovement.Lerp(direction * CurrentSpeed, Acceleration * (_isInAir ? InputInJumpMultiplier : 1) * (float)delta);
		}
		else
		{
			if (!_isInAir)
			{
				_mainMovement = _mainMovement.Lerp(Vector3.Zero, Deceleration * (float)delta);
			}
		}

		// Final update 
		Velocity = _mainMovement + _gravityDirection;
		MoveAndSlide();
	}

	private void interactionProcessing(double delta)
	{
		if (_detector.IsColliding())
		{
			var obj = _detector.GetCollider();
			if (obj is Interactable)
			{
				var interactObj = obj as Interactable;
				EmitSignal(SignalName.Interaction, interactObj.GetTypeObj(), interactObj.GetLabel(), interactObj.GetDescription(), interactObj.GetPrompt());
				if (Input.IsActionJustPressed("interact"))
				{
					interactObj.Interact();
				}
				_send_interaction = true;
			} else {
				if(_send_interaction){
					_send_interaction = false;
					EmitSignal(SignalName.Interaction, "", "", "", "");
				}
				
			}
		}
	}
}
