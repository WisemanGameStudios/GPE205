using UnityEngine;
using System.Linq;
public class RoomZone : MonoBehaviour
{
    public Transform[] patrolPoints;

    public Transform[] GetPatrolPoints()
    {
        return GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
    }
    
    public Vector3 GetRandomPointWithin()
    {
        Vector3 center = transform.position;
        Vector3 size = GetComponent<Collider>().bounds.extents;
        return center + new Vector3(
            Random.Range(-size.x, size.x),
            0,
            Random.Range(-size.z, size.z)
        );
    }
}
