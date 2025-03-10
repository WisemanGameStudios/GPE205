using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Powerup
{
    // Variables
    public float duration;
    public bool isPermanent;
    
    public abstract void Apply(PowerupManager target);
    public abstract void Remove(PowerupManager target);
}
