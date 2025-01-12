using Godot;
using System;

public partial class SprintingPlayerState : PlayerMovementState
{
    [Export]
    public float SpeedMultiplier { get; set; } = 2.0f;
    [Export]
    public float Acceleration { get; set; } = 0.5f;
    [Export]
    public float Deceleration { get; set; } = 1f;
    public override void Enter()
    {
    }

    public override void Update(double delta)
    {
    }

    public override void PhysicsUpdate(double delta)
    {
    }
}
