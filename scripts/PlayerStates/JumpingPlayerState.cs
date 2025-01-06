using Godot;
using System;

public partial class JumpingPlayerState : PlayerMovementState
{
	[Export]
	public float Speed { get; set; } = 5.0f;
	[Export]
	public float Acceleration { get; set; } = 0.5f;
	[Export]
	public float Deceleration { get; set; } = 1f;
	[Export]
	public float JumpVelocity { get; set; } = 4.5f;
	[Export(PropertyHint.Range, "0.1,1.0,0.01")]
	public float InputMultiplier { get; set; } = 0.85f;

	public override void Enter()
	{
		player.ReplaceVelocity(new Vector3(0, JumpVelocity, 0));
	}

	public override void Update(double delta)
	{
		if (player.IsOnFloor()) {
			EmitSignal(SignalName.Transition, "IdlePlayerState");
		}
	}

	public override void PhysicsUpdate(double delta)
	{
		player.UpdateGravity(delta);
		player.UpdateInput(Speed * InputMultiplier, Acceleration, Deceleration);
		player.UpdateVelocity();
	}
}
