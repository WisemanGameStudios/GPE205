using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public float spawnDelay;
    private float _nextSpawnTime;
    
    public GameObject powerupPrefab;
    private GameObject _spawnedPowerup;

    void Start()
    {
        _nextSpawnTime = Time.time + spawnDelay;
    }

    void Update()
    {
        if (_spawnedPowerup == null && Time.time > _nextSpawnTime)
        {
            _spawnedPowerup = Instantiate(powerupPrefab, transform.position, Quaternion.identity);
            _nextSpawnTime = Time.time + spawnDelay;
        }
    }

    // Called by pickup when collected
    public void ClearSpawnedReference()
    {
        _spawnedPowerup = null;
        _nextSpawnTime = Time.time + spawnDelay;
    }
    
    Vector3 GetValidSpawnPoint(Vector3 center, float range)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = center + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out UnityEngine.AI.NavMeshHit hit, 2.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return center; // fallback
    }
}
 
