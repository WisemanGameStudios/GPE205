using UnityEngine;

public class TestGameStateTransitions : MonoBehaviour
{
    private GameStateManager gameManager;

    void Start()
    {
        // Find the GameManager
        gameManager = FindFirstObjectByType<GameStateManager>();

        // Log if GameManager is found or not
        if (gameManager != null)
        {
            Debug.Log("TestGameStateTransitions: GameManager found successfully.");
        }
        else
        {
            Debug.LogError("TestGameStateTransitions: GameManager not found! Make sure GameManager is in the scene.");
        }
    }

    void Update()
    {
        
        if (Input.anyKeyDown)
        {
            Debug.Log("A key was pressed.");
        }
        
        if (gameManager == null) return; // Ensure GameManager exists

        if (Input.GetKey(KeyCode.Alpha1))
        {
            Debug.LogWarning("Transitioning to Title Screen...");
            gameManager.ChangeState(GameStateManager.GameState.TitleScreen);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            Debug.LogWarning("Transitioning to Options...");
            gameManager.ChangeState(GameStateManager.GameState.Options);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            Debug.LogWarning("Transitioning to Gameplay...");
            gameManager.ChangeState(GameStateManager.GameState.Gameplay);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            Debug.LogWarning("Transitioning to Game Over...");
            gameManager.ChangeState(GameStateManager.GameState.GameOver);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            Debug.LogWarning("Transitioning to Credits...");
            gameManager.ChangeState(GameStateManager.GameState.Credits);
        }
    }
}
