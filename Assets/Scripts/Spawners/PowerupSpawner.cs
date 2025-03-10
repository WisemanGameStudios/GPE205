using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    // Variables 
    public float spawnDelay;
    private float _nextSpawnTime;
    private Transform tf;
    
    // Game Object Variables 
    public GameObject powerupPrefab;
    private GameObject _spawnedPowerup;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _nextSpawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        // If it exists nothing spawns
        if (_spawnedPowerup == null)
        {
            // and it is time to spawn
            if (Time.time > _nextSpawnTime)
            {
                // Spawn it and set the next time 
                _spawnedPowerup = Instantiate(powerupPrefab, transform.position, Quaternion.identity) as GameObject; 
                _nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else
        {
            // Otherwise, object still exists, postpone spawn
            _nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
