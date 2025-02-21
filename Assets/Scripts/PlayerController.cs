using UnityEngine;

public class PlayerController : Controller
{
    // Keyboard KeyCodes
    public KeyCode moveUp;
    public KeyCode moveLeft;
    public KeyCode moveDown;
    public KeyCode moveRight;
    
    // Start Function
    public override void Start()
    {
        base.Start();
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
