using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Speed { get; set; } = 5.0f;

	[Export]
	public float JumpVelocity { get; set; } = 10f;


	[Export]
	public float RotationSpeed { get; set; } = 0.005f;

	private Vector3 _targetVelocity;

	private float _gravity;

	public override void _Ready()
	{
		_targetVelocity = Vector3.Zero;
		_gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	}

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1.0f;
		}
		if (Input.IsActionPressed("move_backward"))
		{
			direction.Z += 1.0f;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 1.0f;
		}

		if (Input.IsActionPressed("jump") && IsOnFloor())
		{
			_targetVelocity.Y = JumpVelocity;
		}

		if (direction.Length() > 1e-9)
		{
			direction = direction.Rotated(Vector3.Up, Rotation.Y).Normalized();
			_targetVelocity.X = direction.X * Speed;
			_targetVelocity.Z = direction.Z * Speed;
		}
		else
		{
			_targetVelocity.X = Mathf.MoveToward(_targetVelocity.X, 0, Speed / 2);
			_targetVelocity.Z = Mathf.MoveToward(_targetVelocity.Z, 0, Speed / 2);
		}

		// Vertical velocity
		if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
		{
			_targetVelocity.Y -= _gravity * (float)delta;
		}

		// Moving the character
		Velocity = _targetVelocity;
		MoveAndSlide();
		// for (int i = 0; i < GetSlideCollisionCount(); i++)
		// {
		// 	var collision = GetSlideCollision(i);
		// 	GD.Print("I collided with ", collision, ", total collisions - ", GetSlideCollisionCount());
		// }
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			var newRotation = Rotation;

			newRotation.Y -= mouseMotion.Relative.X * RotationSpeed;

			Rotation = newRotation;

			var newHeadRotation = GetNode<Node3D>("Head").Rotation;

			newHeadRotation.X = Mathf.Clamp(newHeadRotation.X - mouseMotion.Relative.Y * RotationSpeed, -Mathf.Pi / 2, Mathf.Pi / 2);

			GetNode<Node3D>("Head").Rotation = newHeadRotation;
		}
	}
}
