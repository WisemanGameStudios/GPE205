using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<PlayerController> players;

    
    public MapGenerator mapGenerator;
    public int mapSeed;
    public bool isMapOfTheDay = false;
    public bool isRandomSeed = false;
    

    private PawnSpawnPoint[] pawnSpawnPoints;
    private AIPawnSpawn[] AIPawnSpawns;
    
    public GameObject AIControlPrefab;
    public GameObject playercontrolPrefab;
    public GameObject tankPrefab;
    public GameObject aiPrefab;
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
        }
        else
        {
            Destroy(gameObject); // Prevents duplicate GameManagers
        }
    }

    public void StartGame()
    {
        GenerateMap();
        PlayerSpawn();
        SpawnAllAI();
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
        // Add our date up and return it
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }
    
    public void SpawnAI(GameObject aiPrefab)
    {
        if (aiPrefab == null) return;

        AIPawnSpawns = FindObjectsByType<AIPawnSpawn>(FindObjectsSortMode.None);

        if (AIPawnSpawns != null && AIPawnSpawns.Length > 0)
        {
            GameObject spawnPoint = AIPawnSpawns[Random.Range(0, AIPawnSpawns.Length)].gameObject;
            GameObject newPlayer = Instantiate(AIControlPrefab, Vector3.zero, Quaternion.identity);
            GameObject newPawn = Instantiate(aiPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

            if (newPlayer == null || newPawn == null) return;

            AIController newController = newPlayer.GetComponent<AIController>();
            Pawn newPlayerPawn = newPawn.GetComponent<Pawn>();

            if (newController != null && newPlayerPawn != null)
            {
                newPawn.AddComponent<NoiseMaker>();
                newPlayerPawn.noiseMaker = newPawn.GetComponent<NoiseMaker>();
                newPlayerPawn.noiseMakerVolume = 3;

                newController.pawn = newPlayerPawn;
            }
        }
    }
    
    public void PlayerSpawn()
    {

        pawnSpawnPoints = FindObjectsByType<PawnSpawnPoint>(FindObjectsSortMode.None);

        if (pawnSpawnPoints != null)
        {
            if (pawnSpawnPoints.Length > 0)
            {
                GameObject spawnPoint = pawnSpawnPoints[Random.Range(0, pawnSpawnPoints.Length)].gameObject;

                // Spawn player controller at 0,0,0 with 0 rotation
                GameObject newPlayer = Instantiate(playercontrolPrefab, Vector3.zero, Quaternion.identity);

                // Spawn pawn and attach it to controller 
                GameObject newPawn =
                    Instantiate(tankPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;

                // get the pawn and player controller component
                Controller newController = newPlayer.GetComponent<Controller>();
                Pawn newPlayerPawn = newPawn.GetComponent<Pawn>();

                // Get the noise maker component 
                newPawn.AddComponent<NoiseMaker>();
                newPlayerPawn.noiseMaker = newPawn.GetComponent<NoiseMaker>();
                newPlayerPawn.noiseMakerVolume = 3;

                // Create Controller 
                newController.pawn = newPlayerPawn;
            }
        }
    }

    private void SpawnAllAI()
    {
        SpawnAI(aiPrefab);
        SpawnAI(pacifistAIPrefab);
        SpawnAI(couchpotatoAIPrefab);
        SpawnAI(cowardAIPrefab);
        SpawnAI(jerkAIPrefab);
    }
}
   

