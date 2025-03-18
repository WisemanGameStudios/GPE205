using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameStateManager gameStateManager;

    public Button startGameButton;
    public Button optionsButton;
    public Button creditsButton;
    public Button backButton;
    public Button quitButton;
    public Button restartButton;
    public Button mainMenuButton;

    private void Start()
    {
        ShowTitleScreen(); // Start at the title screen
    }

    public void ShowTitleScreen()
    {
        gameStateManager.TitleScreenObject.SetActive(true);
    }
    

    public void ShowOptionsMenu()
    {
        
        gameStateManager.OptionsScreenObject.SetActive(true);
    }

    public void ShowCreditsScreen()
    {
        
        gameStateManager.CreditsScreenObject.SetActive(true);
    }
    
    public void ShowGameOverScreen()
    {
        
        gameStateManager.GameOverScreenObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}
