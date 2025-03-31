using UnityEngine;
using UnityEngine.AI;

public class AISpawner : MonoBehaviour
{
    public GameObject[] aiPrefabs;
    public int numberToSpawn = 5;
    public float spawnRadius = 10f;

    public float navMeshCheckDistance = 5f;

    void Start()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            SpawnOnNavMesh();
        }
    }

    void SpawnOnNavMesh()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPoint.y = transform.position.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, navMeshCheckDistance, NavMesh.AllAreas))
        {
            int index = Random.Range(0, aiPrefabs.Length);
            GameObject ai = Instantiate(aiPrefabs[index], hit.position, Quaternion.identity);
            Debug.Log("✅ AI spawned on NavMesh at " + hit.position);
        }
        else
        {
            Debug.LogWarning("⚠️ No NavMesh found near " + randomPoint);
        }
    }
}