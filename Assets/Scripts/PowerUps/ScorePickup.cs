using System;
using UnityEngine;

public class ScorePickup : MonoBehaviour
{
    // Score points for this pickup
    public int ScoreValue = 100;

    public void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player)
        {
            // Remove Pickup
            Destroy(gameObject);
        }
    }
}


