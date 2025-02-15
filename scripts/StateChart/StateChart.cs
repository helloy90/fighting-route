using Godot;
using System;
using System.Collections.Generic;

public partial class StateChart : Node
{
    private State _rootState;
    private Queue<Transition>[] _queuedTransitions;
    private bool _transitionInProgress;

    
}
