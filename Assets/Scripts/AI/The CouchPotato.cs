using UnityEngine;

public class TheCouchPotato : AIController
{
    public override void ProcessInputs()
    {
        if ((IsCanSee(target) || IsCanHear(target) && IsPlayerDistanceLessThan(target, attackDistance)))
        {
            Debug.Log("The Couch Potato says not going after you");
            ChangeState(AIState.Attack);
        }
        else
        {
            ChangeState(AIState.Patrol);
        }
    }
}
