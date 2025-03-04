using UnityEngine;

public class ThePacifist : AIController
{
    public override void ProcessInputs()
    {
        if (IsCanSee(target) || IsCanHear(target))
        {
            Debug.Log("I'm too young to die!!!!!!!! Thank you!");
            // Just Patrols
            PatrollingState();
        }
    }
}
