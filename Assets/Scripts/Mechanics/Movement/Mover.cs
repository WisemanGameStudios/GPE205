using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    
    public abstract void Start();
    public abstract void Move(Vector3 direction, float speed);

    public void Rotate(float turnSpeed)
    {
        // Rotate tank along the Y-Axis
        transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
    }
}
