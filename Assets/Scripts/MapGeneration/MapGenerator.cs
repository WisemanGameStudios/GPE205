using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation; 

public class MapGenerator : MonoBehaviour
{
    //Variables
    public GameObject[] gridPrefabs;
    public int rows;
    public int cols;
    public float roomWidth = 50.0f;
    public float roomHeight = 50.0f;
    
    // Private Variables
    private Room[,] grid;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject RandomRoomPrefab () {
        return gridPrefabs[Random.Range(0, gridPrefabs.Length)];
    }
    
    public void GenerateMap() {
        
        // Get seed from our GameManager
        int mapSpeed = GameManager.Instance.mapSeed;
        Random.InitState(mapSpeed);
        Debug.Log("Map Seed: " + mapSpeed);
        
        // Start the Grid
        grid = new Room[cols, rows];

        // List to store all NavMeshSurfaces
        List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
        
        // For each grid row...
        for (int currentRow = 0; currentRow < rows; currentRow++) {
            
            // for each column in that row
            for (int currentCol = 0; currentCol < cols ; currentCol++) {
                
                // Figure out the location. 
                float xPosition = roomWidth * currentCol;
                float zPosition = roomHeight * currentRow;
                Vector3 newPosition =  new Vector3 (xPosition, 0.0f, zPosition);
                            
                // Create a new grid at the appropriate location
                GameObject tempRoomObj = Instantiate (RandomRoomPrefab(),  newPosition, Quaternion.identity) as GameObject;
                
                // Give it a meaningful name
                tempRoomObj.name = "Room_"+currentCol+","+currentRow;

                // Get the room object
                Room tempRoom = tempRoomObj.GetComponent<Room>();
                
                // Ensure each room has a NavMeshSurface
                NavMeshSurface navMeshSurface = tempRoomObj.GetComponent<NavMeshSurface>();
                if (navMeshSurface == null)
                {
                    navMeshSurface = tempRoomObj.AddComponent<NavMeshSurface>();
                    navMeshSurface.collectObjects = CollectObjects.Children;
                }
                surfaces.Add(navMeshSurface); // Store for later baking
                
                // Open the doors
                // If we are on the bottom row, open the north door
                if (currentRow == 0) 
                {
                    tempRoom.doorNorth.SetActive(false);
                } 
                else if ( currentRow == rows-1 )
                {
                    // Otherwise, if we are on the top row, open the south door
                    tempRoom.doorSouth.SetActive(false);
                }
                else {
                    // Otherwise, we are in the middle, so open both doors
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }  
                
                // Open the doors
                // If we are on the bottom row, open the north door
                if (currentCol == 0) 
                {
                    tempRoom.doorEast.SetActive(false);
                } 
                else if ( currentCol == cols-1 )
                {
                    // Otherwise, if we are on the top row, open the south door
                    tempRoom.doorWest.SetActive(false);
                }
                else {
                    // Otherwise, we are in the middle, so open both doors
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }  

                // Save it to the grid array
                grid[currentCol,currentRow] = tempRoom;
            }
        }
    }
}
