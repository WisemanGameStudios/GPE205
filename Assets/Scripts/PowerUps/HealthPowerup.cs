[System.Serializable]
public class HealthPowerUp : Powerup
{
    // Variables
    public float healthToAdd; 
    public override void Apply(PowerupManager target)
    {
        // Apply Health changes
        Health targetHealth = target.GetComponent<Health>();

        if (targetHealth != null)
        {

            targetHealth.Heal(healthToAdd, target.GetComponent<Pawn>());
        }
    }

    public override void Remove(PowerupManager target)
    {
        // Remove Health changes
        Health targetHealth = target.GetComponent<Health>();

        if (targetHealth != null)
        {
            // target reference
            targetHealth.TakeDamage(healthToAdd, target.GetComponent<Pawn>());
        }
    }
}