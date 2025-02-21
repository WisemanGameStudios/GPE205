using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    // Pawn Variable
    public ParentPawn pawn;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
    
    // Child overrides process inputs 
    public abstract void ProcessInputs();
}
