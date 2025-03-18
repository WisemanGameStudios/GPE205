
using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller
{
    public enum AIState
    {
        Patrol,
        Chase,
        Attack,
        Flee
    };
    
    // Public Variables 
    public AIState defaultState;
    public GameObject target;
    public float targetDistance;
    public float attackDistance;
    public float minattackDistance;
    public float fleeDistance;
    public float minFleeDistance;
    public float hearDistance;
    public float fieldOfView;
    
    
    // Private Variables 
    public Transform[] waypoints;
    private float _lastStateChangeTime;
    private int currentWaypoint = 0;
    protected NavMeshAgent navAgent;
    
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start(); 
        defaultState = AIState.Patrol;

        // Ensure NavMeshAgent Exists
        navAgent = GetComponent<NavMeshAgent>();
        
        if (navAgent == null)
        {
            Debug.LogError(gameObject.name + " is missing a NavMeshAgent! Adding one now...");
            navAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        // Tweak movement settings
        navAgent.speed = 3.5f;
        navAgent.acceleration = 8f;
        navAgent.angularSpeed = 120f;
        navAgent.stoppingDistance = 0.5f; 
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        
    }

    // Update is called once per frame
    public override void Update()
    {
        ProcessInputs(); 
        base.Update();
        
        
        if (defaultState == AIState.Patrol)
        {
            WanderRandomly();
        }
        else if (defaultState == AIState.Chase)
        {
            ChasingState();
        }
        else if (defaultState == AIState.Flee)
        {
            FleeingState();
        }
        
        if (navAgent.velocity.magnitude > 0.1f) // Rotate if moving
        {
            // Get the direction AI is moving towards
            Vector3 moveDirection = navAgent.velocity.normalized;

            //  Make AI face the movement direction gradually
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        
    }

    public override void ProcessInputs()
    {
        switch (defaultState)
        {
            case AIState.Patrol:
                // Do work for patrol
                if (!IsTargeted())
                {
                    TargetPlayerOne();
                }
                else
                {
                    WanderRandomly();
                }
                // Check for transitions
                if (IsPlayerDistanceLessThan(target, targetDistance))
                {
                    defaultState = AIState.Chase;
                }

                if (!IsPlayerDistanceLessThan(target, targetDistance))
                {
                    defaultState = AIState.Patrol;
                }
                break;
             case AIState.Chase:
                // Do work for Chase
                ChasingState();
               
                // Check for transiti/ons
                if (IsPlayerDistanceLessThan(target, attackDistance))
                {
                    ChangeState(AIState.Attack);
                }

                if (!IsPlayerDistanceLessThan(target, targetDistance) && !IsCanSee(target) && !IsCanHear(target))
                {
                    if (target == null)
                    {
                        ChangeState(AIState.Patrol);
                    }
                }

                if (IsPlayerDistanceLessThan(target, minattackDistance) || IsPlayerDistanceLessThan(target, minFleeDistance))
                {
                    ChangeState(AIState.Flee);
                }
                break;
            case AIState.Attack:
                // Do work for Attack
                AttackingState();
                // Check for transitions 
                if (IsPlayerDistanceLessThan(target, minFleeDistance))
                {
                    ChangeState(AIState.Flee);
                }
                if (!IsPlayerDistanceLessThan(target, targetDistance))
                {
                    ChangeState(AIState.Patrol);
                }
                break;
            case AIState.Flee:
                // Do work for flee
                FleeingState();
                
                // Check for transitions 
                
                if(!IsPlayerDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Patrol);
                }
                break;
        }
    }

    protected void ChasingState()
    {
        if (navAgent == null) 
        {
            Debug.LogError(gameObject.name + " NavMeshAgent is NULL in ChasingState!");
            return;
        }

        if (target == null) 
        {
            Debug.LogWarning(gameObject.name + " has no target to chase.");
            return;
        }

        navAgent.SetDestination(target.transform.position);
    }

    protected void AttackingState()
    {
        // Chase
        Seeking(target);
        
        // Attack
        pawn.Shoot();
    }

    protected void PatrollingState()
    {
        Debug.Log("Patrolling");
        
        // If we have enough waypoints in the list to move to a current waypoint 
        if (waypoints.Length > 0)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            }
            navAgent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    protected void FleeingState()
    {
        Debug.Log("Fleeing target");
        
        if (target == null) return;

        // Find the vector away from target
        Vector3 vectorToTarget = target.transform.position - transform.position;
        Vector3 vectorAwayFromTarget = -vectorToTarget.normalized;
        
        // Set the flee distance
        Vector3 fleeDestination = transform.position + (vectorAwayFromTarget * fleeDistance);
        
        // Flee from target
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeDestination, out hit, fleeDistance, NavMesh.AllAreas))
        {
            navAgent.SetDestination(hit.position);
        }
    }
    

    protected void WanderRandomly()
    {
        if (navAgent == null) 
        {
            Debug.LogError(gameObject.name + " NavMeshAgent is NULL in WanderRandomly!");
            return;
        }

        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            Vector3 randomPoint;
            if (GetRandomNavMeshPoint(transform.position, 10f, out randomPoint))
            {
                navAgent.SetDestination(randomPoint);
            }
        }
    }

// Get a random point on the NavMesh
    private bool GetRandomNavMeshPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 10; i++) // Try 10 times to find a valid point
        {
            Vector3 randomPos = center + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, range, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
    
        result = Vector3.zero;
        return false;
    }
    
    
    // AI Helper Function 

    protected void TargetNearestTank()
    {
        // List of all tanks
        Pawn[] allTanks = FindObjectsByType<Pawn>(FindObjectsSortMode.None);
        
        // Assume first tank is the closest
        Pawn closestTank = allTanks[0];
        float closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);
        
        // Iterate the tanks one at a time 
        foreach (Pawn tank in allTanks)
        {
            // if this tanks closer than the closest
            if (Vector3.Distance(pawn.transform.position, tank.transform.position) <= closestTankDistance)
            {
                // its the closest
                closestTank = tank;
                closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);
            }
        }
        
        // Target Closest Tank
        target = closestTank.gameObject;
    }
    
    

    protected bool IsPlayerDistanceLessThan(GameObject target, float distance)
    {
        // Prevents crashes if `target` or `pawn` is missing
        if (target == null || pawn == null) 
        {
            Debug.LogWarning(gameObject.name + " - IsPlayerDistanceLessThan() called with a null target or pawn.");
            return false;
        }

        return Vector3.Distance(pawn.transform.position, target.transform.position) < distance;
    }

    public virtual void ChangeState(AIState newState)
    {
        // Change the default state
        defaultState = newState;
        
        _lastStateChangeTime = Time.time;
    }

    public void Seeking(GameObject target)
    {
        if (pawn == null || target == null) return; // Prevents accessing destroyed objects

        Vector3 targetPosition = target.transform.position;
        targetPosition.y = pawn.transform.position.y; // Keep original height
        pawn.RotateTowardsTarget(targetPosition);
        pawn.MoveUp();
    }

    public void Seeking(Transform targetTransform)
    {
        //Seeking position of target transform 
        Seeking(targetTransform.gameObject);
    }

    public void Seeking(Pawn targetPawn)
    {
        // Seeking Pawn's Transform 
        Seeking(targetPawn.transform);
    }

    private void Seeking(Vector3 destination)
    {
        // Keep the AI's original Y position to avoid unnecessary movement in the Y axis
        destination.y = pawn.transform.position.y;

        // Rotate AI towards the flee destination
        pawn.RotateTowardsTarget(destination);

        // Move AI in that direction
        pawn.MoveUp();
    }

    public void TargetPlayerOne()
    {
        if (GameManager.instance == null || GameManager.instance.players == null || GameManager.instance.players.Count == 0)
        {
            Debug.LogWarning(gameObject.name + " - No players found in GameManager!");
            return;
        }

        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (var player in GameManager.instance.players)
        {
            if (player == null || player.pawn == null) continue; // Skip destroyed players

            float distance = Vector3.Distance(transform.position, player.pawn.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.pawn.gameObject;
            }
        }

        if (closestPlayer != null)
        {
            target = closestPlayer;
            Debug.Log(gameObject.name + " - Targeting Player: " + target.name);
        }
        else
        {
            Debug.LogWarning(gameObject.name + " - No valid players to target.");
        }
    }

    protected bool IsTargeted()
    {
        // Return true if we have target, false if not
        return (target != null);
    }

    protected bool IsCanHear(GameObject target)
    {
        Debug.Log("IsCanHear");
        // Get NoiseMaker from target
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();

        // if target doesn't have a noisemaker, it can't make noise
        if (noiseMaker == null)
        {
            return false;
        }
        
        // if target is not making any noise, it still cannot be heard
        if (noiseMaker.volumeDistance <= 0)
        {
            return false;
        }
        
        //  if target is making noise, add volume distance to the hearing distance
        float TotalDistance = noiseMaker.volumeDistance + hearDistance;
        
        // if the target distance is closer between the pawn at the target is closer
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= TotalDistance)
        {
             //  target can be heard
             return true;
        }
        else
        {
            //  we are to far away
            return false;
        }
    }

    protected bool IsCanSee(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null or has been destroyed.");
            return false;
        }

        Debug.Log("Searching for : " + target);

        // Find the agent vector to target
        Vector3 agentToTarget = target.transform.position - pawn.transform.position;

        // Find the angle between the distance our agent is facing and the target vector 
        float angleToTarget = Vector3.Angle(pawn.transform.forward, agentToTarget);

        // if angle is less than field of view
        if (angleToTarget < fieldOfView)
        {
            // Define the starting position to prevent self collision 
            Vector3 rayOrigin = pawn.transform.position + pawn.transform.forward * 0.6f;

            // Define ray's direction 
            Vector3 directionToTarget = agentToTarget.normalized;

            // Raycast in order to check if there's an obstacle
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, directionToTarget, out hit, targetDistance))
            {
                // Check if object hit is the target
                if (hit.collider.gameObject == target)
                {
                    Debug.Log("I see " + target.name);
                    return true;
                }
                else
                {
                    Debug.Log("Something is in my way");
                    return false;
                }
            }
        }
        // Out of Field of View 
        return false;
    }
}
