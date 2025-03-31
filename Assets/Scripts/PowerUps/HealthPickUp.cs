using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    // Variables
    public float healthAmount = 50f;
    public float rotationSpeed = 45f;

    // Sound Effect
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    private PowerupSpawner spawner;

    private void Start()
    {
        spawner = GetComponentInParent<PowerupSpawner>(); // spawner tracking
    }

    private void Update()
    {
        // Rotate for visibility
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only respond to tanks/players with a Health component
        Health health = other.GetComponent<Health>();
        if (health != null && health.currentHealth < health.maxHealth)
        {
            Debug.Log($"{other.name} picked up health!");

            health.Heal(healthAmount);

            // Play effect
            if (pickupEffect)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            // Play sound
            if (pickupSound)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Tell the spawner to clear reference (optional)
            if (spawner != null)
                spawner.ClearSpawnedReference();

            Destroy(gameObject);
        }
    }
}