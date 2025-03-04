using UnityEngine;

public class ScrewThis : AIController
{
    public override void ProcessInputs()
    {
        if (target != null && IsCanSee(target) || IsCanHear(target))
        {
            ChangeState(AIState.Flee);
        }
    }
}
