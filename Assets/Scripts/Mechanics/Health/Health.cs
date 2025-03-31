using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Health Settings
    public float currentHealth;
    public float maxHealth;

    // References
    public PlayerController controller;
    public bool isPlayer;

    private GameStateManager gameStateManager;
    private bool isDead = false;

    // Declare the death event
    public event Action<GameObject> OnDeath;

    void Start()
    {
        currentHealth = maxHealth;

        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }

        gameStateManager = GameStateManager.Instance;
        if (gameStateManager == null)
        {
            Debug.LogError("🚨 GameStateManager instance not found in the scene!");
        }
    }

    
    public void TakeDamage(float amount, Pawn source = null)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (source != null)
        {
            Debug.Log($"{source.gameObject.name} dealt {amount} damage to {gameObject.name}.");
        }
        else
        {
            Debug.Log($"{gameObject.name} took {amount} damage.");
        }

        // Update the health bar visually
        TankHealthBar healthBar = GetComponent<TankHealthBar>();
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            int killerPlayerNum = 0;

            // Try to determine which player caused the damage
            if (source != null)
            {
                if (source.controller is Player1Test) killerPlayerNum = 1;
                else if (source.controller is Player2Test) killerPlayerNum = 2;
            }

            // If the tank is a player, notify their script
            {
                

                if (TryGetComponent<Player1Test>(out var p1)) p1.Die();
                else if (TryGetComponent<Player2Test>(out var p2)) p2.Die();

                // Give score to killer for killing a player
                if (killerPlayerNum > 0)
                    GameStateManager.Instance?.AddScore(killerPlayerNum, 300); // or 500 etc
                
                // AI or others
                Die(source, killerPlayerNum); // Updated to include player who did the kill
            }
        }
    }

    public void Heal(float amount, Pawn source = null)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{(source != null ? source.name : "Unknown")} healed {gameObject.name} for {amount}.");

        // Update health bar
        TankHealthBar healthBar = GetComponent<TankHealthBar>();
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"💀 {gameObject.name} has died!");

        if (isPlayer)
        {
            OnDeath?.Invoke(gameObject);
        }
        
        
        Destroy(gameObject);
    }
    
    public void Die(Pawn source, int killedByPlayerNum = 0)
    {
        Debug.Log($"{gameObject.name} was killed by {(source != null ? source.name : "Unknown")}");

        int scoreToGive = 0;

        // Determine score based on AI type
        if (TryGetComponent<TheJerk>(out _)) scoreToGive = 150;
        else if (TryGetComponent<ScrewThis>(out _)) scoreToGive = 100;
        else if (TryGetComponent<CouchPotato>(out _)) scoreToGive = 50;
        else if (TryGetComponent<ThePacifist>(out _)) scoreToGive = 25;
        else scoreToGive = 100; // fallback if not AI type

        // Determine which player killed it
        if (source != null)
        {
            Player1Test p1 = source.GetComponent<Player1Test>();
            Player2Test p2 = source.GetComponent<Player2Test>();

            if (p1 != null)
            {
                gameStateManager.AddScore(1, scoreToGive);
            }
            else if (p2 != null)
            {
                gameStateManager.AddScore(2, scoreToGive);
            }
        }

        Die(); // call the original
    }
}
