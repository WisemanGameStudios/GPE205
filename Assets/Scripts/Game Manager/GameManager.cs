using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Variables
    public Transform playerSpawn;

    // Declare GameManager instance
    public static GameManager instance;
    
    // List that holds our player(s)
    public List<PlayerController> players;

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
    }
    
    private void Start()
    {
        // Spawn as soon as game manager starts
        PlayerSpawn();
    }

    public void PlayerSpawn()
    {
        // Spawn player controller at 0,0,0 with 0 rotation
        GameObject newPlayer = Instantiate(playercontrolPrefab, Vector3.zero, Quaternion.identity);

        // Spawn pawn and attach it to controller 
        GameObject newPawn = Instantiate(TankPrefab, playerSpawn.position, playerSpawn.rotation) as GameObject;

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
    
    

    // gameObject Prefabs
    public GameObject playercontrolPrefab;
    public GameObject TankPrefab;
}

