using UnityEngine;

public abstract class ParentPawn : MonoBehaviour
{
    
    // Variables
    public float moveSpeed;
    
    // Movement Functions
    public abstract void MoveUp();
    public abstract void MoveDown();
    public abstract void RotateLeft();
    public abstract void RotateRight();

}
