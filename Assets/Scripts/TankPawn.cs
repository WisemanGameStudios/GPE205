
using UnityEngine;

public class TankPawn : ParentPawn
{
    // Mover Variable 
    public Mover mover;

    // Call before first frame update
    void Start()
    {
        mover = GetComponent<Mover>();
    }

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
        transform.Rotate(0, -moveSpeed * Time.deltaTime, 0);
    }

    public override void RotateLeft()
    {
        transform.Rotate(0, moveSpeed * Time.deltaTime, 0);
    }
}

