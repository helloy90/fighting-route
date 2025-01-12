using Godot;
using Microsoft.VisualBasic;
using System;

[GlobalClass]
public partial class PlayerMovementState : State
{

    public Player player;

    public override /*async*/ void _Ready()
    {
        // await ToSignal(Owner, SignalName.Ready);
        // player = Owner as Player;
    }
}