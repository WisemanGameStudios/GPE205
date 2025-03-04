using UnityEngine;

public class TheJerk : AIController
{
    public override void ProcessInputs()
    {
        if (target != null && IsCanSee(target) || IsCanHear(target))
        {
            ChangeState(AIState.Chase);
            if (IsPlayerDistanceLessThan(target, attackDistance))
            {
                ChangeState(AIState.Attack);
                Seeking(target);
            }
        }
    }
}
