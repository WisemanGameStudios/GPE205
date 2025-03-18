using UnityEngine;

public class Player1Test : MonoBehaviour
{
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode shootKey = KeyCode.Space;
    
    private Pawn pawn;
    private Camera playerCamera;
    private Health playerHealth;
    private GameStateManager gameStateManager;

    void Start()
    {
        pawn = GetComponent<Pawn>();
        playerHealth = GetComponent<Health>(); // Get Health component
        gameStateManager = GameStateManager.Instance; // Assign GameStateManager

        if (pawn == null)
        {
            Debug.LogError("🚨 No Pawn component found on Player 1!");
        }

        if (playerHealth == null)
        {
            Debug.LogError("🚨 No Health component found on Player 1!");
        }

        // Subscribe to health check if available
        if (playerHealth != null)
        {
            playerHealth.isPlayer = true; // Mark as player
        }
    }

    void Update()
    {
        ProcessInputs();

        // Check if Player 1's health is zero
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    public void SetupCamera(Camera cam)
    {
        playerCamera = cam;
        playerCamera.rect = new Rect(0, 0, 0.5f, 1); // Left side of the screen
        playerCamera.transform.SetParent(transform);
        playerCamera.transform.localPosition = new Vector3(0, 10, -10);
        playerCamera.transform.LookAt(transform.position);
    }

    void ProcessInputs()
    {
        if (pawn == null) return;

        if (Input.GetKey(moveUp)) pawn.MoveUp();
        if (Input.GetKey(moveLeft)) pawn.RotateLeft();
        if (Input.GetKey(moveDown)) pawn.MoveDown();
        if (Input.GetKey(moveRight)) pawn.RotateRight();
        if (Input.GetKeyDown(shootKey)) pawn.Shoot();
    }

    private void TriggerGameOver()
    {
        Debug.Log("Player 1 has died! Triggering Game Over...");

        if (gameStateManager != null)
        {
            gameStateManager.ShowGameOverScreen();
        }
        else
        {
            Debug.LogError("🚨 GameStateManager not found!");
        }
    }
}
