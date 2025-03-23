using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float currentHealth;
    public float maxHealth;

    [Header("References")]
    public PlayerController controller;
    public bool isPlayer;

    private GameStateManager gameStateManager;
    private bool isDead = false;

    // ✅ Declare the death event
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
            // If the tank is a player, notify the player script to fire OnDeath
            if (isPlayer)
            {
                if (TryGetComponent<Player1Test>(out var p1)) p1.Die();
                else if (TryGetComponent<Player2Test>(out var p2)) p2.Die();
            }
            else
            {
                // AI or others
                Die(source);
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
    
    public void Die(Pawn source)
    {
        // Optional: use 'source' for tracking who caused the death
        Debug.Log($"{gameObject.name} was killed by {(source != null ? source.name : "Unknown")}");

        Die(); // Call the no-arg version
    }
}
