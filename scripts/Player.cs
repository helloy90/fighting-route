using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Signal]
	public delegate void InteractionEventHandler(string type, string label, string description, string prompt);

	[Export(PropertyHint.Range, "0,15,or_greater,suffix:m/s\u00b2")]
	private float Gravity { get; set; } = 12f; // zero for default engine value, manual input for more than 12
	[Export(PropertyHint.None, "suffix:m/s")]
	private Vector3 MaxGravityVector { get; set; } = new Vector3(25, 25, 25); // for clamping gravity speed (simulate air resistance)

	[Export(PropertyHint.None, "suffix:m/s")]
	private float BaseSpeed { get; set; } = 10f;
	[Export(PropertyHint.None, "suffix:m/s\u00b2")]
	private float Acceleration { get; set; } = 10f;
	[Export(PropertyHint.None, "suffix:m/s\u00b2")]
	private float Deceleration { get; set; } = 10f;
	// [Export]
	// private int AmountOfDashes { get; set; } = 2;
	[Export(PropertyHint.None, "suffix:s")]
	private float DashCooldownTime { get; set; } = 1f;
	[Export(PropertyHint.None, "suffix:s")]
	private float InDashTime { get; set; } = 0.1f; // seconds, always less than dash cooldown
	[Export]
	private float DashSpeedMultipliter { get; set; } = 3f;
	[Export]
	private float JumpStrength { get; set; } = 6f;
	[Export(PropertyHint.Range, "0.1,1.0,0.01")]
	private float InputInAirMultiplier { get; set; } = 0.5f;
	[Export(PropertyHint.Range, "-90,90,degrees")]
	private float TiltLowerLimitDegrees { get; set; } = -90;
	[Export(PropertyHint.Range, "-90,90,degrees")]
	private float TiltUpperLimitDegrees { get; set; } = 90;

	[Export]
	public float MouseSensitivity { get; set; } = 0.3f;

	[Export]
	private Node3D CameraController;

	// maybe reconsider properties later
	public float CurrentSpeed => BaseSpeed * _currentSpeedMultiplier;
	private float _currentSpeedMultiplier;
	private float _currentAccelerationMultiplier;
	public float CurrentAcceleration => Acceleration * _currentAccelerationMultiplier;

	private float _inDashTime;
	private float _dashCooldownTime;
	// private int _currentDashesAmount;

	private float _tiltLowerLimit;
	private float _tiltUpperLimit;

	private Vector3 _mainMovement;
	private Vector3 _gravityDirection;
	private float _gravityValue;

	private Vector3 _dashDirection;

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
		_inDashTime = 0;
		_dashCooldownTime = 0;
		// _currentDashesAmount = 0;

		_tiltLowerLimit = Mathf.DegToRad(TiltLowerLimitDegrees);
		_tiltUpperLimit = Mathf.DegToRad(TiltUpperLimitDegrees);

		_mainMovement = Vector3.Zero;
		_gravityDirection = Vector3.Zero;
		_gravityValue = Mathf.IsZeroApprox(Gravity) ? (float)ProjectSettings.GetSetting("physics/3d/default_gravity") : Gravity;
		_dashDirection = Vector3.Zero;

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
			_gravityDirection = _gravityDirection.Clamp(-MaxGravityVector, MaxGravityVector);
		}

		if (Input.IsActionJustPressed("jump") && !_isInAir)
		{
			_jumped = true;
			_isInAir = true;
			_gravityDirection = Vector3.Up * JumpStrength;
		}

		if (Mathf.IsZeroApprox(direction.Length()))
		{
			if (!_isInAir)
			{
				_mainMovement = _mainMovement.Lerp(Vector3.Zero, Deceleration * (float)delta);
			}
		}
		else
		{
			_mainMovement = _mainMovement.Lerp(direction * CurrentSpeed, Acceleration * (_isInAir ? InputInAirMultiplier : 1) * (float)delta);
		}

		// // Dash ------------------------------------
		// if (Input.IsActionJustPressed("dash") && Mathf.IsZeroApprox(_dashCooldownTime))
		// {
		// 	GD.Print("dash begin");
		// 	_dashCooldownTime = DashCooldownTime;
		// 	_inDashTime = 0;

		// 	_currentSpeedMultiplier = DashSpeedMultipliter;

		// 	if (Mathf.IsZeroApprox(direction.Length()))
		// 	{
		// 		_dashDirection = (Basis * Vector3.Forward).Normalized();
		// 	}
		// 	else
		// 	{
		// 		_dashDirection = direction;
		// 	}
		// }

		// if (!Mathf.IsEqualApprox(_inDashTime, InDashTime))
		// {
		// 	_inDashTime = Mathf.Clamp(_inDashTime + (float)delta, 0, InDashTime);
		// 	_mainMovement = _dashDirection * CurrentSpeed;
		// }
		// else if (Mathf.IsEqualApprox(_inDashTime, InDashTime))
		// {
		// 	_currentSpeedMultiplier = 1;
		// 	_mainMovement = _dashDirection * CurrentSpeed;
		// 	GD.Print("dash end");
		// }

		// if (!Mathf.IsZeroApprox(_dashCooldownTime))
		// {
		// 	_dashCooldownTime = Mathf.Clamp(_dashCooldownTime - (float)delta, 0, DashCooldownTime);
		// }
		// GD.Print(Velocity, " - velocity");
		// // Dash finish -----------------------------

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
			}
			else
			{
				if (_send_interaction)
				{
					_send_interaction = false;
					EmitSignal(SignalName.Interaction, "", "", "", "");
				}

			}
		}
	}
}
