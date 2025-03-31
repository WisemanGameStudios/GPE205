using UnityEngine;

public class ThePacifist : AIController
{
    public override void Update()
    {
        base.Update();

        currentState = AIState.Patrol;
    }
}