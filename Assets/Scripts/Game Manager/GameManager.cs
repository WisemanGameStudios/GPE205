using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // Public Variables
    public MapGenerator mapGenerator;
    
    // Seed Settings 
    public int mapSeed; // Default manual seed
    public bool isMapOfTheDay = false; // "Map of the Day" toggle
    public bool isRandomSeed = false; // Fully random map toggle
    
    // Private Variable 
    private PawnSpawnPoint[] pawnSpawnPoints;
    private AIPawnSpawn[] aiSpawnPoints;
    
    // Declare GameManager instance
    public static GameManager instance;
    
    // List that holds our player(s)
    public List<PlayerController> players;
    
    // gameObject Prefabs
    public GameObject AIControlPrefab;
    public GameObject playercontrolPrefab;
    public GameObject tankPrefab;
    public GameObject aiPrefab;
    public GameObject pacifistAIPrefab;
    public GameObject couchpotatoAIPrefab;
    public GameObject cowardAIPrefab;
    public GameObject jerkAIPrefab;
    

    // Awake function called when object is first created
    private void Awake()
    {
        
        // if the instance is nonexistent 
        if (instance == null)
        {
            // create the instance
            instance = this;

            // Don't destroy object when new scene loads
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If instance is already existent
            Destroy(gameObject);
        }
        
        // Generate seed based on user selection
        GenerateSeed();
    }
    
    private void Start()
    {
        if (mapGenerator != null)
        {
            mapGenerator.GenerateMap();
        }
        
        // Generate seed based on user selection
        GenerateSeed();
        
        // Spawn as soon as game manager starts
        PlayerSpawn();
        
        // Spawn multiple AI Units when game starts
        AISpawn();
        PacifistAISpawn();
        CouchPotatoAISpawn();
        CowardAISpawn();
        JerkAISpawn();
    }

    public void GenerateSeed()
    {
        if (isMapOfTheDay)
        {
            // Year/Month/Day as the seed
            mapSeed = DateToInt(DateTime.Now.Date);
        }
        else if (isRandomSeed)
        {
            // Use time to generate a completely random seed
            mapSeed = DateToInt(DateTime.Now);
        }
        
        // Log the seed for debugging
        Debug.Log("GameManager Seed: " + mapSeed);
    }

    public int DateToInt(DateTime dateToUse)
    {
        return dateToUse.Year + dateToUse.Month + dateToUse.Day +
               dateToUse.Hour + dateToUse.Minute + dateToUse.Second +
               dateToUse.Millisecond;
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
                GameObject newPawn = Instantiate(tankPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;

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
    
    
    public void AISpawn()
    {
        // Find AI Spawn points
        aiSpawnPoints = FindObjectsByType<AIPawnSpawn>(FindObjectsSortMode.None);

        if (aiSpawnPoints != null)
        {
            if (aiSpawnPoints.Length > 0)
            {
                GameObject spawnPoint = aiSpawnPoints[Random.Range(0, aiSpawnPoints.Length)].gameObject;
                
                // Spawn player controller at 0,0,0 with 0 rotation
                GameObject newPlayer = Instantiate(AIControlPrefab, Vector3.zero, Quaternion.identity);

                // Spawn pawn and attach it to controller 
                GameObject newPawn = Instantiate(aiPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;

                // get the pawn and player controller component
                AIController newController = newPlayer.GetComponent<AIController>();
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
    
    // Spawn the Pacifist AI
    public void PacifistAISpawn()
    {
        // Find AI Spawn points
        aiSpawnPoints = FindObjectsByType<AIPawnSpawn>(FindObjectsSortMode.None);

        if (aiSpawnPoints != null)
        {
            if (aiSpawnPoints.Length > 0)
            {
                GameObject spawnPoint = aiSpawnPoints[Random.Range(0, aiSpawnPoints.Length)].gameObject;
                
                // Spawn player controller at 0,0,0 with 0 rotation
                GameObject newPlayer = Instantiate(AIControlPrefab, Vector3.zero, Quaternion.identity);

                // Spawn pawn and attach it to controller 
                GameObject newPawn = Instantiate(pacifistAIPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;

                // get the pawn and player controller component
                AIController newController = newPlayer.GetComponent<AIController>();
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
    
    // Spawn The Couch Potato AI
    public void CouchPotatoAISpawn()
    {
        // Find AI Spawn points
        aiSpawnPoints = FindObjectsByType<AIPawnSpawn>(FindObjectsSortMode.None);

        if (aiSpawnPoints != null)
        {
            if (aiSpawnPoints.Length > 0)
            {
                GameObject spawnPoint = aiSpawnPoints[Random.Range(0, aiSpawnPoints.Length)].gameObject;
                
                // Spawn player controller at 0,0,0 with 0 rotation
                GameObject newPlayer = Instantiate(AIControlPrefab, Vector3.zero, Quaternion.identity);

                // Spawn pawn and attach it to controller 
                GameObject newPawn = Instantiate(couchpotatoAIPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;

                // get the pawn and player controller component
                AIController newController = newPlayer.GetComponent<AIController>();
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
    
    // Spawn The Coward 
    public void CowardAISpawn()
    {
        // Find AI Spawn points
        aiSpawnPoints = FindObjectsByType<AIPawnSpawn>(FindObjectsSortMode.None);

        if (aiSpawnPoints != null)
        {
            if (aiSpawnPoints.Length > 0)
            {
                GameObject spawnPoint = aiSpawnPoints[Random.Range(0, aiSpawnPoints.Length)].gameObject;
                
                // Spawn player controller at 0,0,0 with 0 rotation
                GameObject newPlayer = Instantiate(AIControlPrefab, Vector3.zero, Quaternion.identity);

                // Spawn pawn and attach it to controller 
                GameObject newPawn = Instantiate(cowardAIPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;

                // get the pawn and player controller component
                AIController newController = newPlayer.GetComponent<AIController>();
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
    
    // Spawn The Jerk AI
    public void JerkAISpawn()
    {
        // Find AI Spawn points
        aiSpawnPoints = FindObjectsByType<AIPawnSpawn>(FindObjectsSortMode.None);

        if (aiSpawnPoints != null)
        {
            if (aiSpawnPoints.Length > 0)
            {
                GameObject spawnPoint = aiSpawnPoints[Random.Range(0, aiSpawnPoints.Length)].gameObject;
                
                // Spawn player controller at 0,0,0 with 0 rotation
                GameObject newPlayer = Instantiate(AIControlPrefab, Vector3.zero, Quaternion.identity);

                // Spawn pawn and attach it to controller 
                GameObject newPawn = Instantiate(jerkAIPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;

                // get the pawn and player controller component
                AIController newController = newPlayer.GetComponent<AIController>();
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

