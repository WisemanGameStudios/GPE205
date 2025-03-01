
using UnityEngine;
using UnityEngine.Audio;

public class TankPawn : Pawn
{
    
    // Call before first frame update
    public override void Start()
    {
        base.Start();
    }
    
    
    // Movement Functions
    public override void MoveUp()
    {
        mover.Move(transform.forward, moveSpeed);
    }

    public override void MoveDown()
    {
        mover.Move(transform.forward, -moveSpeed);
    }

    public override void RotateRight()
    {
        mover.Rotate(turnSpeed);
    }

    public override void RotateLeft()
    {
        mover.Rotate(-turnSpeed);
    }
}

