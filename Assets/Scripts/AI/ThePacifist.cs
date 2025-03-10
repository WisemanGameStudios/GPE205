using UnityEngine;

public class ThePacifist : AIController
{
    public override void Start()
    {
        base.Start(); // Ensure base AI initialization
        defaultState = AIState.Patrol; // Ensure it starts in Patrol mode
    }

    public override void ProcessInputs()
    {
        PatrollingState(); // Continue patrolling
    }
}
