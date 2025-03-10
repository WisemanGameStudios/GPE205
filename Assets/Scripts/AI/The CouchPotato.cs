using UnityEngine;

public class TheCouchPotato : AIController
{
    public override void Start()
    {
        base.Start(); // Ensure base AI initialization
        defaultState = AIState.Patrol; // Default to Patrol
        
    }

    public override void ProcessInputs()
    {
        ChangeState(AIState.Attack);
    }
}
