using NUnit.Framework.Constraints;
using Unity.VisualScripting;
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
    public float waypontStopDistance;
    public float fleeDistance;
    public float minFleeDistance;
    public float hearDistance;
    public float fieldOfView;
    
    
    // Private Variables 
    public Transform[] waypoints;
    private float _lastStateChangeTime;
    private int currentWaypoint = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
       defaultState = AIState.Patrol;
    }

    // Update is called once per frame
    public override void Update()
    {
        ProcessInputs(); 
        base.Update();
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
                    PatrollingState();
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
        // Seeking after targets
        Debug.Log("Chasing target");
        Seeking(target);
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
        if (waypoints.Length > currentWaypoint)
        {
            // Then seek that waypoint 
            Seeking(waypoints[currentWaypoint]);
            
            // If we are close enough, increment to next waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypontStopDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            RestartPatrol();
        }
    }

    protected void FleeingState()
    {
        Debug.Log("Fleeing target");
        // Find the vector away from the target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;
        Vector3 vectorAwayFromTarget = -vectorToTarget.normalized; 
        
        // Calculate flee destination
        Vector3 fleeDestination = pawn.transform.position + (vectorAwayFromTarget * fleeDistance);

        // Move AI to flee destination
        Seeking(fleeDestination);
    }
    

    protected void RestartPatrol()
    {
        currentWaypoint = 0;
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
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void ChangeState(AIState newState)
    {
        // Change the default state
        defaultState = newState;
        
        _lastStateChangeTime = Time.time;
    }

    public void Seeking(GameObject target)
    {
        Vector3 targetPosition = target.transform.position;

        // Keep the AI's original Y position
        targetPosition.y = pawn.transform.position.y;
        
        // Rotate Towards target
        pawn.RotateTowardsTarget(targetPosition);
        
        // Move to target
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
        // If game manager exists 
        if (GameManager.instance != null)
        {
            // and if the players array exists 
            if (GameManager.instance.players != null)
            {
                // And players are in it
                if (GameManager.instance.players.Count > 0)
                {
                    // Then target the pawn of the first player 
                    target = GameManager.instance.players[0].pawn.gameObject;
                }
            }
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
