using UnityEngine;
using UnityEngine.AI;

public class TheCouchPotato : AIController
{
    public override void Start()
    {
        base.Start(); // Ensure base AI initialization
        defaultState = AIState.Patrol; // Default to Patrol

        // Ensure TheCouchPotato has a NavMeshAgent
        if (navAgent == null)
        {
            Debug.LogError(gameObject.name + " is missing a NavMeshAgent!");
            return;
        }
    }

    public override void ProcessInputs()
    {
        if (target == null)
        {
            TargetPlayerOne(); // Try to find a player
        }

        if (target != null)
        {
            
            ChangeState(AIState.Attack);
            AttackTarget(); // Use NavMesh to move toward target and attack
        }
        else
        {
            ChangeState(AIState.Patrol);
            WanderRandomly(); // If no target, just patrol
        }
    }

    private void AttackTarget()
    {
        if (target == null || navAgent == null) return;

        // Move toward the target for an attack
        navAgent.SetDestination(target.transform.position);

        // If within attack range, perform an attack
        if (IsPlayerDistanceLessThan(target, attackDistance))
        {
            Debug.Log(gameObject.name + " is attacking!");
            pawn.Shoot(); // Perform attack
        }
    }
}
