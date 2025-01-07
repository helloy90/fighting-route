using Godot;
using System;

public partial class JumpingPlayerState : PlayerMovementState
{
	[Export]
	public float SpeedMultiplier { get; set; } = 1.0f;
	[Export]
	public float Acceleration { get; set; } = 0.5f;
	[Export]
	public float Deceleration { get; set; } = 1f;
	[Export]
	public float JumpStrength { get; set; } = 4.5f;
	[Export(PropertyHint.Range, "0.1,1.0,0.01")]
	public float InputMultiplier { get; set; } = 0.85f;

	public override void Enter()
	{
		player.Jump(JumpStrength);
	}

	public override void Update(double delta)
	{

	}

	public override void PhysicsUpdate(double delta)
	{
		player.UpdateGravity(delta);
		player.UpdateInput(SpeedMultiplier * InputMultiplier, Acceleration, Deceleration);
		player.UpdateVelocity();

		if (player.IsOnFloor())
		{
			EmitSignal(SignalName.Transition, "IdlePlayerState");
		}
	}
}
