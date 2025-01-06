using Godot;
using System;

public partial class SprintingPlayerState : PlayerMovementState
{
    [Export]
    public float Speed { get; set; } = 10.0f;
    [Export]
    public float Acceleration { get; set; } = 0.5f;
    [Export]
    public float Deceleration { get; set; } = 1f;
    public override void Enter()
    {
    }

    public override void Update(double delta)
    {
        if (Input.IsActionJustReleased("sprint"))
        {
            EmitSignal(SignalName.Transition, "WalkingPlayerState");
        }
        
        if (Input.IsActionJustPressed("jump") && player.IsOnFloor())
        {
            EmitSignal(SignalName.Transition, "JumpingPlayerState");
        }
    }

    public override void PhysicsUpdate(double delta)
    {
        player.UpdateGravity(delta);
        player.UpdateInput(Speed, Acceleration, Deceleration);
        player.UpdateVelocity();
    }

}
