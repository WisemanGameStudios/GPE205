using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public void BakeNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface not found on " + gameObject.name);
        }
    }
}
