using Godot;
using System;

public partial class IdlePlayerState : State
{
    [Export]
    public Player player;
    public override void Update(double delta) {
        if (player.Velocity.Length() > 0.0001f) {
            EmitSignal(SignalName.Transition, "WalkingPlayerState");
        }
    }
}
