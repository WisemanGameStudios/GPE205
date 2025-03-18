using UnityEngine;

public class Health : MonoBehaviour
{
    // Public Variables 
    public float currentHealth;
    public float maxHealth;
    public PlayerController controller;
    public bool isPlayer; // Identify if this entity is the player
    
    // Private Variables
    private GameStateManager gameStateManager;
    

    void Start()
    {
        currentHealth = maxHealth;
        
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }

        // Assign GameStateManager
        gameStateManager = GameStateManager.Instance;

        if (gameStateManager == null)
        {
            Debug.LogError("GameStateManager instance not found in the scene!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Press Q to take damage for testing
        {
            TakeDamage(10f); // Reduce health by 10
        }
    }
    
    public void TakeDamage(float amount)
    {
        TakeDamage(amount, null); // Calls the main method with null as source
    }
    
    public void TakeDamage(float amount, Pawn source)
    {
        currentHealth -= amount;
        Debug.Log($"{(source != null ? source.gameObject.name : "Unknown")} did {amount} damage to {gameObject.name}");
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die(source);
        }
    }

    public void Heal(float amount, Pawn source)
    {
        currentHealth += amount;
        Debug.Log($"{(source != null ? source.name : "Unknown")} healed {gameObject.name} for {amount}");
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void Die(Pawn source)
    {
        Debug.Log($"{gameObject.name} has died!");

        // If this is the player, trigger Game Over
        if (isPlayer && gameStateManager != null)
        {
            gameStateManager.ShowGameOverScreen();
            Debug.Log("Game Over triggered.");
        }

        Destroy(gameObject);
    }
}