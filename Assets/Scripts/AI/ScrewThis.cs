using UnityEngine;

public class ScrewThis : AIController
{
    public override void Start()
    {
        base.Start(); // Ensure base AI initialization
        defaultState = AIState.Patrol; // Start in Patrol mode
    }

    public override void ProcessInputs()
    {
       if (target != null && IsPlayerDistanceLessThan(target, targetDistance))
        {
            Debug.Log("Screw this! I'm out of here!");
            ChangeState(AIState.Flee);
        }
        else
        {
            ChangeState(AIState.Patrol); // Default back to patrol when safe
        }
    }
}
