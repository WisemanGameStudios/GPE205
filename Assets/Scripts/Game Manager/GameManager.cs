using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
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
            UIContainer.SetActive(false); // Hide UI when the map starts generating
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
        return dateToUse.Year * 10000 + dateToUse.Month * 100 + dateToUse.Day;
    }

    void CreatePlayer(GameObject prefab, Vector3 spawnPosition, Camera cam, int playerNumber)
    {
        GameObject playerObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
        PlayerController controller = playerObj.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.playerCamera = cam;
            controller.playerNumber = playerNumber;
            players.Add(controller);
        }
    }

    public void RegisterPlayer(PlayerController player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    public void UnregisterPlayer(PlayerController player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }
    }

    private void SpawnAI(GameObject aiPrefab)
    {
        AIPawnSpawns = FindObjectsByType<AIPawnSpawn>(FindObjectsSortMode.None);

        if (AIPawnSpawns != null && AIPawnSpawns.Length > 0)
        {
            GameObject spawnPoint = AIPawnSpawns[Random.Range(0, AIPawnSpawns.Length)].gameObject;
            GameObject newPlayer = Instantiate(AIControlPrefab, Vector3.zero, Quaternion.identity);
            GameObject newPawn = Instantiate(aiPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

            AIController newController = newPlayer.GetComponent<AIController>();
            Pawn newPlayerPawn = newPawn.GetComponent<Pawn>();

            newPawn.AddComponent<NoiseMaker>();
            newPlayerPawn.noiseMaker = newPawn.GetComponent<NoiseMaker>();
            newPlayerPawn.noiseMakerVolume = 3;

            newController.pawn = newPlayerPawn;
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
}
   

