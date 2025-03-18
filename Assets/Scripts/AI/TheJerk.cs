using UnityEngine;
using UnityEngine.AI;

public class TheJerk : AIController
{
    public override void Start()
    {
        base.Start(); // Call the base class Start() method
        TargetPlayerOne(); // Automatically select the nearest target when spawned

        // EnsureTheJerk has NavMeshAgent
        if (navAgent == null)
        {
            Debug.LogError(gameObject.name + " is missing a NavMeshAgent!");
            return;
        }

        // Immediately start chasing if a target exists
        if (target != null)
        {
            ChangeState(AIState.Chase);
        }
    }

    public override void ProcessInputs()
    {
        if (target == null)
        {
            TargetPlayerOne(); // Try to find the player again
        }

        if (target != null && navAgent != null)
        {
            navAgent.SetDestination(target.transform.position); // Move toward the player
        }
    }
}