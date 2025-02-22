using System;
using UnityEngine;

[System.Serializable]
public class PlayerController : Controller
{
    // Keyboard KeyCodes
    public KeyCode moveUp;
    public KeyCode moveLeft;
    public KeyCode moveDown;
    public KeyCode moveRight;
    
    // Start before first frame update 
    public override void Start()
    {
        // if GameManager exists
        if (GameManager.instance != null)
        {
            // if it tracks the players
            if (GameManager.instance.players != null)
            {
                // Add / Register to GameManager
                GameManager.instance.players.Add(this);
            }
            // Run Start function from parent class
            base.Start();
        }
    }

    public void OnDestroy()
    {
        // if GameManager exists 
        if (GameManager.instance != null)
        {
            // If it tracks the players 
            if (GameManager.instance.players != null)
            {
                // Remove / Deregister from GameManager
                GameManager.instance.players.Remove(this);
            }
        }
    }
    

    // Update Function 
    public override void Update()
    {
        // Inputs from Keyboard 
        ProcessInputs();
        
        // Update function called from parent class
        base.Update();
    }

    public override void ProcessInputs()
    {
        if (Input.GetKey(moveUp))
        {
            pawn.MoveUp();
        }

        if (Input.GetKey(moveLeft))
        {
            pawn.RotateLeft();
        }

        if (Input.GetKey(moveDown))
        {
            pawn.MoveDown();
        }

        if (Input.GetKey(moveRight))
        {
            pawn.RotateRight();
        }
    }
}
