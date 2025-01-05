using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public float MouseSensitivity { get; set; } = 0.3f;

	[Export]
	public float TiltLowerLimit { get; set; } = Mathf.DegToRad(-90);
	[Export]
	public float TiltUpperLimit { get; set; } = Mathf.DegToRad(90);

	[Export]
	public Node3D CameraController;

	private Vector3 _targetVelocity;
	private float _gravity;
	// Rotation and tilt from mouse event
	private Vector2 _rotationInput;
	private Vector3 _mouseRotation;
	private Vector3 _cameraRotation;
	private Vector3 _playerRotation;

	public override void _Ready()
	{
		_targetVelocity = Vector3.Zero;
		_gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateCamera(delta);

		// TODO: move jump logic to an appopriate state
		// if (Input.IsActionPressed("jump") && IsOnFloor())
		// {
		// 	_targetVelocity.Y = JumpVelocity;
		// }
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
		_mouseRotation.X = Mathf.Clamp(_mouseRotation.X, TiltLowerLimit, TiltUpperLimit);
		_mouseRotation.Y += (float)(_rotationInput.X * delta);

		_playerRotation = Vector3.Zero with { Y = _mouseRotation.Y };

		_cameraRotation = Vector3.Zero with { X = _mouseRotation.X };

		CameraController.Transform = CameraController.Transform with { Basis = Basis.FromEuler(_cameraRotation) };
		GlobalTransform = GlobalTransform with { Basis = Basis.FromEuler(_playerRotation) };
		CameraController.Rotation = CameraController.Rotation with { Z = 0.0f };

		_rotationInput = Vector2.Zero;
	}

	public void UpdateGravity(double delta)
	{
		_targetVelocity.Y -= (float)(_gravity * delta);
	}

	public void UpdateInput(float speed, float acceleration, float deceleration) // maybe change float -> double later
	{
		var inputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		var direction = (Transform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y)).Normalized();

		if (direction.Length() > 1e-9)
		{
			_targetVelocity.X = Mathf.Lerp(_targetVelocity.X, direction.X * speed, acceleration);
			_targetVelocity.Z = Mathf.Lerp(_targetVelocity.Z, direction.Z * speed, acceleration);
		}
		else
		{
			_targetVelocity.X = Mathf.MoveToward(_targetVelocity.X, 0, deceleration);
			_targetVelocity.Z = Mathf.MoveToward(_targetVelocity.Z, 0, deceleration);
		}
	}

	// Final velocity update (call after everything else)
	public void UpdateVelocity()
	{
		Velocity = _targetVelocity;
		MoveAndSlide();
	}
}