using UnityEngine;

public class ScrewThis : AIController
{
    public AudioClip scream;
    private AudioSource audioSource;

    public override void Update()
    {
        base.Update();

        if (target == null)
        {
            FindPlayerTarget();
        }

        if (target != null && Vector3.Distance(transform.position, target.transform.position) <= detectionRadius)
        {
            if (!audioSource.isPlaying && scream != null)
            {
                audioSource.clip = scream;
                audioSource.Play();
            }

            currentState = AIState.Flee; 
        }
        else
        {
            currentState = AIState.Patrol; 
        }
    }

    private void FindPlayerTarget()
    {
        if (GameManager.Instance == null || GameManager.Instance.players == null) return;

        float closest = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (var player in GameManager.Instance.players)
        {
            if (player != null && player.pawn != null)
            {
                float dist = Vector3.Distance(transform.position, player.pawn.transform.position);
                if (dist < closest)
                {
                    closest = dist;
                    closestTarget = player.pawn.gameObject;
                }
            }
        }

        target = closestTarget;
    }
}