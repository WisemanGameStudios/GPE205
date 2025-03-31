using UnityEngine;

public class TankShoot : Shooter
{
    // Variables
    public Transform firePoint;

    public override void Start()
    {
    }

    public override void Update()
    {
    }

    
    // Sets up the shell to spawn when the Shoot function is called 
    public override void Shoot(GameObject Shell ,float fireForce, float damageDone, float lifeTime)
    {
       GameObject newShell = Instantiate(Shell, firePoint.position, firePoint.rotation) as GameObject;
       
       
       DamageOnHit doh = newShell.GetComponent<DamageOnHit>();
       if (doh != null)
       {
           doh.damageDone = damageDone;
           doh.pawn = GetComponent<Pawn>();
       }
       
       Rigidbody rb = newShell.GetComponent<Rigidbody>();
       if (rb != null)
       {
           rb.AddForce(firePoint.forward * fireForce);
       }
       
       Destroy(newShell, lifeTime);
    }
}

