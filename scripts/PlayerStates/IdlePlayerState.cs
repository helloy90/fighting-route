using Godot;
using System;

public partial class IdlePlayerState : PlayerMovementState
{
	// For starting movement, maybe change logic later
	[Export]
	public float SpeedMultiplier { get; set; } = 5.0f;
	[Export]
	public float Acceleration { get; set; } = 0.5f;
	[Export]
	public float Deceleration { get; set; } = 1f;
	public override void Update(double delta)
	{

	}

	public override void PhysicsUpdate(double delta)
	{
		player.UpdateGravity(delta);
		player.UpdateInput(SpeedMultiplier, Acceleration, Deceleration);
		player.UpdateVelocity();

		if (player.Velocity.Length() > 0.0001f)
		{
			EmitSignal(SignalName.Transition, "WalkingPlayerState");
		}

		if (Input.IsActionJustPressed("jump") && player.IsOnFloor())
		{
			EmitSignal(SignalName.Transition, "JumpingPlayerState");
		}
	}
}
