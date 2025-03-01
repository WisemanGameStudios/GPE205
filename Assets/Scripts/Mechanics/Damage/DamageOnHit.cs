using System;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    // Variables 
    public float damageDone;
    public Pawn pawn;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Makes sure the tank shell hits and damages the targets collider
    public void OnTriggerEnter(Collider other)
    {
        Health otherHealth = other.gameObject.GetComponent<Health>();

        if (otherHealth != null)
        {
            otherHealth.TakeDamage(damageDone, pawn);
        }
        
        Destroy(gameObject);
    }
}
