using System;
using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
   
    // Variables 
    public abstract void Start();
    public abstract void Update();
    public abstract void Shoot(GameObject Shell, float fireForce, float damageDone, float lifeTime);
}
