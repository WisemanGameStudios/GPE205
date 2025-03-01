using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    // Shooting Variables 
    public float fireForce;
    public float fireRate;
    public float damageDone;
    public float shellLife;
    
    
    // Movement Variables 
    public float moveSpeed;
    public float turnSpeed;
    
    // Game Mechanic Variables 
    public Mover mover;
    public Shooter shooter;
    
    // Game Object Variables 
    public GameObject shellPrefab;
    
   public virtual void Start()
    {
        mover = GetComponent<Mover>();
    }
    
    // Movement Functions
    public abstract void MoveUp();
    public abstract void MoveDown();
    public abstract void RotateLeft();
    public abstract void RotateRight();
}
