using UnityEngine;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour
{
    // UI References
    public GameObject inGameMenuPanel;

    private bool isPaused = false;

    void Start()
    {
        if (inGameMenuPanel != null)
        {
            inGameMenuPanel.SetActive(false); // Hide menu on start
        }
    }

    void Update()
    {
        // Make sure GameStateManager exists
        if (GameStateManager.Instance == null) return;

        // Toggle in-game menu on ESC only during gameplay
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P Pressed");

            if (GameStateManager.Instance != null &&
                GameStateManager.Instance.currentState == GameStateManager.GameState.Gameplay)
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (inGameMenuPanel != null)
            inGameMenuPanel.SetActive(true);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (inGameMenuPanel != null)
            inGameMenuPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void GotoOptions()
    {
        GameStateManager.Instance.ShowOptionsScreen();
    }

    public void QuitToMainMenu()
    {
        // Optional method to switch to title screen if you want
        ResumeGame(); // Resume time before switching
        GameStateManager.Instance.ShowTitleScreen();
    }
}
