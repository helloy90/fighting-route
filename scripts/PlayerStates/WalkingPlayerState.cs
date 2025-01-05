using Godot;
using System;

public partial class WalkingPlayerState : PlayerMovementState
{
	[Export]
	public float Speed { get; set; } = 5.0f;
	[Export]
	public float Acceleration { get; set; } = 0.5f;
	[Export]
	public float Deceleration { get; set; } = 1f;
	public override void Enter()
	{
	}

	public override void Update(double delta)
	{
		if (player.Velocity.Length() < 0.0001f)
		{
			EmitSignal(SignalName.Transition, "IdlePlayerState");
		}

		if (Input.IsActionPressed("sprint") && player.IsOnFloor())
		{
			EmitSignal(SignalName.Transition, "SprintingPlayerState");
		}
	}

	public override void PhysicsUpdate(double delta)
	{
		player.UpdateGravity(delta);
		player.UpdateInput(Speed, Acceleration, Deceleration);
		player.UpdateVelocity();
	}

}
