using UnityEngine;

public class TankMover : Mover
{
    // Rigidbody Variable 
    private Rigidbody rb;
    
    // Start is called first
    public override void Start()
    {
        // Get the Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    public override void Move(Vector3 direction, float speed)
    {
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + moveVector);
    }
}
