using UnityEngine;

public class AIController : Controller
{
    public enum AIState { Patrol, Chase, Attack, Flee }

    public AIState currentState = AIState.Patrol;
    public GameObject target;
    public float roamInterval = 2f;
    public float moveSpeed = 3f;
    public float detectionRadius = 10f;
    public float attackDistance = 5f;
    public float fleeDistance = 3f;

    protected float nextRoamTime;
    protected Vector3 roamTarget;
    protected RoomZone currentZone;
    protected Rigidbody rb;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        AssignNearestRoomZone();
        PickNewRoamPoint();
    }

    public override void Update()
    {
        base.Update();
        UpdateState();
        CorrectFlip();
    }

    public override void ProcessInputs()
    {
        return;
    }
    protected void UpdateState()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                RoamRandomly();
                break;
            case AIState.Chase:
                ChaseTarget();
                break;
            case AIState.Attack:
                AttackTarget();
                break;
            case AIState.Flee:
                FleeFromTarget();
                break;
        }
    }

    protected void RoamRandomly()
    {
        if (Time.time >= nextRoamTime)
        {
            PickNewRoamPoint();
        }

        if (Vector3.Distance(transform.position, roamTarget) > 1f)
        {
            pawn.RotateTowardsTarget(roamTarget);
            pawn.MoveUp();
        }
    }

    protected void PickNewRoamPoint()
    {
        if (currentZone != null)
        {
            roamTarget = currentZone.GetRandomPointWithin();
            nextRoamTime = Time.time + roamInterval;
        }
    }

    protected void ChaseTarget()
    {
        if (target == null) return;
        pawn.RotateTowardsTarget(target.transform.position);
        pawn.MoveUp();
    }

    protected void AttackTarget()
    {
        if (target == null) return;
        pawn.RotateTowardsTarget(target.transform.position);
        pawn.Shoot();
    }

    protected void FleeFromTarget()
    {
        if (target == null) return;
        Vector3 fleeDir = (transform.position - target.transform.position).normalized;
        Vector3 fleePos = transform.position + fleeDir * fleeDistance;
        pawn.RotateTowardsTarget(fleePos);
        pawn.MoveUp();
    }

    protected void AssignNearestRoomZone()
    {
        RoomZone[] allZones = FindObjectsOfType<RoomZone>();
        float closest = Mathf.Infinity;

        foreach (var zone in allZones)
        {
            float dist = Vector3.Distance(transform.position, zone.transform.position);
            if (dist < closest)
            {
                currentZone = zone;
                closest = dist;
            }
        }

        if (currentZone == null)
            Debug.LogWarning($"{gameObject.name} could not find any RoomZone.");
    }

    protected void CorrectFlip()
    {
        if (Mathf.Abs(transform.up.y) < 0.5f && rb != null)
        {
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}