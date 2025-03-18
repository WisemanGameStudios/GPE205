using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    // Variables 
    public float noiseMakerVolume;
    public PlayerController controller;
    
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
    public NoiseMaker noiseMaker;
    
    // Game Object Variables 
    public GameObject shellPrefab;
    
    // Start and Update Functions
   public virtual void Start()
    {
        mover = GetComponent<Mover>();
        shooter = GetComponent<Shooter>();
        noiseMaker = GetComponent<NoiseMaker>();
        controller = GetComponent<PlayerController>();
    }

    public virtual void Update()
    {
        
    }

    public void MoveTo(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
    }
    
    // Movement Functions
    public abstract void MoveUp();
    public abstract void MoveDown();
    public abstract void RotateLeft();
    public abstract void RotateRight();
    public abstract void Shoot();
    public abstract void RotateTowardsTarget(Vector3 targetPosition);
    public abstract void MakeNoise();
    public abstract void StopNoise();
}
