using UnityEngine;

public class TheJerk : AIController
{
    public override void Start()
    {
        base.Start(); // Call the base class Start() method
        TargetPlayerOne(); // Automatically select the nearest target when spawned
    }

    public override void ProcessInputs()
    {
        ChangeState(AIState.Chase);
    }
}
