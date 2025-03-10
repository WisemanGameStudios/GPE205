using System;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    //Variables
    public HealthPowerUp powerup;
    public float rotateSpeed;

    private void Update()
    {
        // Rotate the pick up
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        // Variable to store objects PowerupController
        PowerupManager pm = other.gameObject.GetComponent<PowerupManager>();
        
        // If other object has a PowerupController
        if (pm)
        {
            // Add the powerup
            pm.Add(powerup);
            
            // Destroy the pickup
            Destroy(gameObject);
        }
    }
}
