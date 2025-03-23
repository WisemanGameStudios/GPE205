using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HealthPickup : MonoBehaviour
{
    
    public float healthAmount = 50f;
    public float rotationSpeed = 45f;

    
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    private void Update()
    {
        // Rotate for visibility
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"HealthPickup triggered by: {other.name}");

        // Try to get a Health component from the object
        Health health = other.GetComponent<Health>();
        if (health != null && health.currentHealth < health.maxHealth)
        {
            // Heal the tank
            health.Heal(healthAmount);

            // Spawn visual effect
            if (pickupEffect)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            // Play sound (if AudioSource is present)
            if (pickupSound)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Destroy the pickup
            Destroy(gameObject);
        }
    }
}