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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
    }
    

}
