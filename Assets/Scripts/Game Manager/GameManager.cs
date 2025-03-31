using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<PlayerController> players;

    public MapGenerator mapGenerator;
    public MultiplayerManager multiplayerManager;
    public int mapSeed;
    public bool isMapOfTheDay = false;
    public bool isRandomSeed = false;

    private PawnSpawnPoint[] pawnSpawnPoints;
    private AIPawnSpawn[] AIPawnSpawns;
    
    // Prefabs
    public GameObject AIControlPrefab;
    public GameObject playercontrolPrefab;
    public GameObject tankPrefab;
    public GameObject pacifistAIPrefab;
    public GameObject couchpotatoAIPrefab;
    public GameObject cowardAIPrefab;
    public GameObject jerkAIPrefab;
    public GameObject UIContainer; // UI to disable on game start
    
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevents duplicate GameManagers
        }
        
    }
    
    public void StartGame()
    {
        CleanupPreviousGame();
        GenerateMap();
        multiplayerManager.SpawnPlayers();
        SpawnAllAI();
    }

    private void CleanupPreviousGame()
    {
        // Destroy existing player controllers
        foreach (var player in FindObjectsByType<PlayerController>(FindObjectsSortMode.None))
        {
            Destroy(player.gameObject);
        }

        // Destroy existing pawns
        foreach (var pawn in FindObjectsByType<Pawn>(FindObjectsSortMode.None))
        {
            Destroy(pawn.gameObject);
        }

        // Destroy existing AI controllers
        foreach (var ai in FindObjectsByType<AIController>(FindObjectsSortMode.None))
        {
            Destroy(ai.gameObject);
        }

        // Destroy any existing Cameras
        foreach (var cam in FindObjectsByType<Camera>(FindObjectsSortMode.None))
        {
            Destroy(cam.gameObject);
        }

        // Destroy AudioListeners (optional if you're instantiating them on camera)
        foreach (var listener in FindObjectsByType<AudioListener>(FindObjectsSortMode.None))
        {
            Destroy(listener.gameObject);
        }
    }

    private void GenerateMap()
    {
        Debug.Log("Generating New Map...");

        if (mapGenerator != null)
        {
            mapGenerator.GenerateMap();
        }
    }

    public void GenerateSeed()
    {
        if (isMapOfTheDay)
        {
            mapSeed = DateToInt(DateTime.Now.Date);
        }
        else if (isRandomSeed)
        {
            mapSeed = Random.Range(1, int.MaxValue);
        }

        Debug.Log($"Generated Seed: {mapSeed}");
    }

    private int DateToInt(DateTime dateToUse)
    {
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }

    public void SpawnAI(GameObject aiPrefab)
{
    if (aiPrefab == null) return;

    AIPawnSpawns = FindObjectsByType<AIPawnSpawn>(FindObjectsSortMode.None);

    if (AIPawnSpawns != null && AIPawnSpawns.Length > 0)
    {
        // Try multiple times to avoid overlapping AI
        const int maxTries = 10;
        Vector3 chosenSpawn = Vector3.zero;
        Quaternion chosenRot = Quaternion.identity;
        bool foundClear = false;

        for (int i = 0; i < maxTries; i++)
        {
            GameObject spawnPoint = AIPawnSpawns[Random.Range(0, AIPawnSpawns.Length)].gameObject;
            Vector3 testPos = spawnPoint.transform.position;

            if (!Physics.CheckSphere(testPos, 2f)) // Check for overlaps
            {
                chosenSpawn = testPos;
                chosenRot = spawnPoint.transform.rotation;
                foundClear = true;
                break;
            }
        }

        if (!foundClear)
        {
            Debug.LogWarning("No clear AI spawn point found. Spawning anyway...");
            GameObject fallback = AIPawnSpawns[0].gameObject;
            chosenSpawn = fallback.transform.position;
            chosenRot = fallback.transform.rotation;
        }

        GameObject newControllerObj = Instantiate(AIControlPrefab, Vector3.zero, Quaternion.identity);
        GameObject newPawnObj = Instantiate(aiPrefab, chosenSpawn, chosenRot);

        if (newControllerObj == null || newPawnObj == null) return;

        AIController controller = newControllerObj.GetComponent<AIController>();
        Pawn pawn = newPawnObj.GetComponent<Pawn>();

        if (controller != null && pawn != null)
        {
            // Add noise system
            newPawnObj.AddComponent<NoiseMaker>();
            pawn.noiseMaker = newPawnObj.GetComponent<NoiseMaker>();
            pawn.noiseMakerVolume = 3;

            // Assign control
            controller.pawn = pawn;

            // Optional: Assign target from GameManager's player list
            if (players != null && players.Count > 0)
            {
                // Pick random player from GameManager player list
                var randomPlayer = players[Random.Range(0, players.Count)];
                controller.target = randomPlayer?.pawn?.gameObject;
            }
        }
    }
}
    private void SpawnAllAI()
    {
        SpawnAI(pacifistAIPrefab);
        SpawnAI(couchpotatoAIPrefab);
        SpawnAI(cowardAIPrefab);
        SpawnAI(jerkAIPrefab);
    }
}

   

