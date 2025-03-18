using UnityEngine;
using System.Collections.Generic;

public class TesttMultiplayerManager : MonoBehaviour
{
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public Camera player1Camera;
    public Camera player2Camera;

    private List<PawnSpawnPoint> spawnPoints = new List<PawnSpawnPoint>();

    void Start()
    {
        // Find all available spawn points in the scene
        PawnSpawnPoint[] foundPoints = FindObjectsOfType<PawnSpawnPoint>();
        spawnPoints = new List<PawnSpawnPoint>(foundPoints);

        if (spawnPoints.Count < 2)
        {
            Debug.LogError("🚨 Not enough spawn points! Found: " + spawnPoints.Count);
            return;
        }

        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        // Randomly pick two different spawn points
        PawnSpawnPoint spawn1 = spawnPoints[Random.Range(0, spawnPoints.Count)];
        spawnPoints.Remove(spawn1);
        PawnSpawnPoint spawn2 = spawnPoints[Random.Range(0, spawnPoints.Count)];

        Debug.Log($"✅ Selected Spawn Points: {spawn1.name} & {spawn2.name}");

        // Spawn Player 1
        GameObject p1 = Instantiate(player1Prefab, spawn1.transform.position, Quaternion.identity);
        Player1Test controller1 = p1.GetComponent<Player1Test>();
        if (controller1 != null)
        {
            controller1.SetupCamera(player1Camera);
            Debug.Log($"✅ Player 1 spawned at {spawn1.name}");
        }

        // Spawn Player 2
        GameObject p2 = Instantiate(player2Prefab, spawn2.transform.position, Quaternion.identity);
        Player2Test controller2 = p2.GetComponent<Player2Test>();
        if (controller2 != null)
        {
            controller2.SetupCamera(player2Camera);
            Debug.Log($"✅ Player 2 spawned at {spawn2.name}");
        }
    }
}
