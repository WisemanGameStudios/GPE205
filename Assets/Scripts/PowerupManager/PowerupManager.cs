using System;
using UnityEngine;
using UnityEngine.Android;
using System.Collections.Generic;

[System.Serializable]
public class PowerupManager : MonoBehaviour
{
    
    public List<Powerup> powerups;
    private List<Powerup> removePowerupsQueue;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerups = new List<Powerup>();
        removePowerupsQueue = new List<Powerup>();
    }

    // Update is called once per frame
   void Update()
    {
       DecrementPowerupTimers(); 
    }

    private void LateUpdate()
    {
        ApplyRemovePowerupsQueue();
    }

    public void Add(Powerup powerupToAdd)
    {
        // Apply Powerup
        powerupToAdd.Apply(this);
        
        // save it to the list
        powerups.Add(powerupToAdd);
    }

    public void Remove(Powerup powerupToRemove)
    {
        //  Remove Powerup
        powerupToRemove.Remove(this);
        
        // Add to remove queue
        removePowerupsQueue.Add(powerupToRemove);
    }
    
    public void DecrementPowerupTimers()
    {
        // One-at-a-time, put each object in "powerups" into the variable "powerup" and do the loop body on it
        foreach (Powerup powerup in powerups) {
            if (!powerup.isPermanent)
            {
                // Subtract the time it took to draw the frame from the duration
                powerup.duration -= Time.deltaTime;
                // If time is up, we want to remove this powerup
                if (powerup.duration <= 0) 
                {
                    Remove(powerup);
                }
            }
        }
    }

    private void ApplyRemovePowerupsQueue()
    {
        // we are not iterating through power ups, remove items in our temporary list
        foreach (Powerup powerup in removePowerupsQueue)
        {
            powerups.Remove(powerup);
        }
        
        // Reset our temporary list
        removePowerupsQueue.Clear();
    }
    
    
}
