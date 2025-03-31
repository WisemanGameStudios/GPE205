using UnityEngine;
using System.Linq;
public class WaypointGroup : MonoBehaviour
{
    public Transform[] GetWaypoints()
    {
        return GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
    }
}
