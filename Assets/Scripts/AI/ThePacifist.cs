using UnityEngine;
using UnityEngine.AI;

public class ThePacifist : AIController
{
    private Vector3 lastPosition;
    private float stuckTime = 0f;
    private float stuckThreshold = 2.0f; // Time in seconds before AI picks a new direction

    public override void Start()
    {
        base.Start(); 
        defaultState = AIState.Patrol;
        lastPosition = transform.position; // Save initial position
    }

    public override void ProcessInputs()
    {
        if (IsStuck())
        {
            Debug.LogWarning(gameObject.name + " is stuck! Finding a new path.");
            WanderRandomly(); // Try moving to a random NavMesh location
        }
        else
        {
            PatrollingState(); // Continue patrolling
        }
    }

    private bool IsStuck()
    {
        // Check if AI hasn't moved significantly
        if (Vector3.Distance(transform.position, lastPosition) < 0.1f)
        {
            stuckTime += Time.deltaTime;
            if (stuckTime > stuckThreshold) // AI is stuck for too long
            {
                stuckTime = 0f; // Reset timer
                return true;
            }
        }
        else
        {
            stuckTime = 0f; // Reset timer if moving
            lastPosition = transform.position; // Update last position
        }
        return false;
    }
}
