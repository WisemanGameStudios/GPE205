
using System;
using UnityEngine;
using UnityEngine.Audio;

public class TankPawn : Pawn
{
    // Variables 
    private float timerDelay;
    private float nextShotTime;
    
    
    // Call before first frame update
    public override void Start()
    {
        timerDelay = 1 / fireRate;
        
        nextShotTime = Time.time + nextShotTime;
        
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    
    // Movement Functions
    public override void MoveUp()
    {
        mover.Move(transform.forward, moveSpeed);
    }

    public override void MoveDown()
    {
        mover.Move(transform.forward, -moveSpeed);
    }

    public override void RotateRight()
    {
        mover.Rotate(turnSpeed);
    }

    public override void RotateLeft()
    {
        mover.Rotate(-turnSpeed);
    }

    public override void Shoot()
    {
        if (Time.time >= nextShotTime)
        {
            shooter.Shoot(shellPrefab, fireForce, damageDone, shellLife); 
            nextShotTime = Time.time + timerDelay;
        }
    }

    public override void RotateTowardsTarget(Vector3 targetPosition)
    {
        // Find the targets Vector
        Vector3 TargetVector = targetPosition - transform.position;
        
        // Find the vector to look down
        Quaternion TargetRotation = Quaternion.LookRotation(TargetVector, Vector3.up);
        
        //Rotate close to vector, don't rotate more than turn speed allows 
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, turnSpeed * Time.deltaTime);
    }

    public override void MakeNoise()
    {
        if (noiseMaker != null)
        {
            noiseMaker.volumeDistance = noiseMakerVolume;
        }
    }

    public override void StopNoise()
    {
        if (noiseMaker != null)
        {
            noiseMaker.volumeDistance = 0;
        }
        
    }
}

