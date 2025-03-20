using UnityEngine;

public class Player2Test : MonoBehaviour
{
    public KeyCode moveUp = KeyCode.UpArrow;
    public KeyCode moveLeft = KeyCode.LeftArrow;
    public KeyCode moveDown = KeyCode.DownArrow;
    public KeyCode moveRight = KeyCode.RightArrow;
    public KeyCode shootKey = KeyCode.Return;

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
            Debug.LogError("No Pawn component found on Player 2!");
        }

        if (playerHealth == null)
        {
            Debug.LogError("No Health component found on Player 2!");
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
    }

    public void SetupCamera(Camera cam)
    {
        playerCamera = cam;
        playerCamera.rect = new Rect(0.5f, 0, 0.5f, 1); // Right side of the screen
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
}