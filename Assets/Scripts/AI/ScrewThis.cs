using UnityEngine;
using UnityEngine.AI;

public class ScrewThis : AIController
{
    public override void Start()
    {
        base.Start(); // Ensure base AI initialization
        defaultState = AIState.Patrol; // Start in Patrol mode

        // Ensure ScrewThis has a NavMeshAgent
        if (navAgent == null)
        {
            Debug.LogError(gameObject.name + " is missing a NavMeshAgent!");
        }
    }

    public override void ProcessInputs()
    {
        if (target == null)
        {
            TargetPlayerOne(); // Try to find the player if no target exists
        }

        if (target != null && IsPlayerDistanceLessThan(target, targetDistance))
        {
            Debug.Log("Screw this! I'm out of here!");
            ChangeState(AIState.Flee);
            FleeFromTarget(); // Use NavMesh to move away
        }
        else
        {
            ChangeState(AIState.Patrol);
            WanderRandomly(); // Ensure AI continues patrolling
        }
    }

    private void FleeFromTarget()
    {
        if (target == null || navAgent == null) return;

        // Calculate a flee direction away from the target
        Vector3 fleeDirection = (transform.position - target.transform.position).normalized;
        Vector3 fleeDestination = transform.position + (fleeDirection * fleeDistance);

        // Ensure AI stays on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeDestination, out hit, fleeDistance, NavMesh.AllAreas))
        {
            navAgent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning(gameObject.name + " couldn't find a valid flee position!");
        }
    }
}
