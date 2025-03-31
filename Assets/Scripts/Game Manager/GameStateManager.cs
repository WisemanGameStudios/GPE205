using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public enum GameState
    {
        TitleScreen,
        Options,
        Gameplay,
        GameOver,
        Credits
    }

    public GameObject TitleScreenObject;
    public GameObject OptionsScreenObject;
    public GameObject GameOverScreenObject;
    public GameObject CreditsScreenObject;
    public GameObject UIContainer; // Stores all UI elements to disable them on game start

    public Toggle mapOfTheDayToggle;
    public Toggle randomSeedToggle;
    public TMP_InputField seedInputField;
    public Toggle splitScreenToggle; // UI Toggle for Split-Screen

    public GameState currentState;
    private GameManager gameManager;
    private MultiplayerManager multiplayerManager; // MultiplayerManager Reference
    
    // Score
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;

    private int player1Score = 0;
    private int player2Score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        multiplayerManager = FindObjectOfType<MultiplayerManager>(); // Find the multiplayer manager

        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        if (multiplayerManager == null)
        {
            Debug.LogError("MultiplayerManager instance not found!");
            return;
        }
        
        GameStateManager.Instance.ChangeState(GameState.Gameplay);
        ChangeState(GameState.TitleScreen);
        Time.timeScale = 1;
        UpdateScoreUI(1);
        UpdateScoreUI(2);
    }
    
    public void AddScore(int playerNumber, int points)
    {
        if (playerNumber == 1)
        {
            player1Score += points;
            UpdateScoreUI(1);
        }
        else if (playerNumber == 2)
        {
            player2Score += points;
            UpdateScoreUI(2);
        }
    }

    private void UpdateScoreUI(int playerNumber)
    {
        if (playerNumber == 1 && player1ScoreText != null)
            player1ScoreText.text = "P1 Score: " + player1Score.ToString();
    
        if (playerNumber == 2 && player2ScoreText != null)
            player2ScoreText.text = "P2 Score: " + player2Score.ToString();
    }

    private void DeactivateAllStates()
    {
        if (TitleScreenObject) TitleScreenObject.SetActive(false);
        if (OptionsScreenObject) OptionsScreenObject.SetActive(false);
        if (GameOverScreenObject) GameOverScreenObject.SetActive(false);
        if (CreditsScreenObject) CreditsScreenObject.SetActive(false);
    }

    public void ChangeState(GameState newState)
    {
        DeactivateAllStates();
        currentState = newState;

        switch (currentState)
        {
            case GameState.TitleScreen:
                if (TitleScreenObject) TitleScreenObject.SetActive(true);
                break;
            case GameState.Options:
                if (OptionsScreenObject) OptionsScreenObject.SetActive(true);
                break;
            case GameState.Credits:
                if (CreditsScreenObject) CreditsScreenObject.SetActive(true);
                break;
            case GameState.GameOver:
                if (GameOverScreenObject) GameOverScreenObject.SetActive(true);
                break;
        }
    }
    
    public void ShowTitleScreen() => ChangeState(GameState.TitleScreen);
    public void ShowOptionsScreen() => ChangeState(GameState.Options);
    public void ShowCreditsScreen() => ChangeState(GameState.Credits);
    public void ShowGameOverScreen() => ChangeState(GameState.GameOver);

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void SetMapOfTheDay(bool value)
    {
        gameManager.isMapOfTheDay = value;
        gameManager.isRandomSeed = !value;
        gameManager.GenerateSeed();
    }

    public void SetRandomSeed(bool value)
    {
        gameManager.isRandomSeed = value;
        gameManager.isMapOfTheDay = !value;
        gameManager.GenerateSeed();
    }
    
    public void SetCustomSeed()
    {
        gameManager.isMapOfTheDay = false;
        gameManager.isRandomSeed = false;
        gameManager.mapSeed = int.TryParse(seedInputField.text, out int userSeed) ? userSeed : 0;
        mapOfTheDayToggle.isOn = false;
        randomSeedToggle.isOn = false;
        gameManager.GenerateSeed();
    }

    public void StartGame()
    {
        Debug.Log("Starting Game...");

        // Disable UI when the game starts
        if (UIContainer != null)
        {
            UIContainer.SetActive(true); // Ensure the UI container stays active
        
            foreach (Transform child in UIContainer.transform)
            {
                child.gameObject.SetActive(false); // Disable only its children
            }
        }
        
        // Check if Split-Screen Toggle is Enabled
        bool isSplitScreenEnabled = splitScreenToggle != null && splitScreenToggle.isOn;
        Debug.Log($"🎮 Split-Screen Mode: {(isSplitScreenEnabled ? "Enabled" : "Disabled")}");

        // Apply the correct camera settings in MultiplayerManager
        multiplayerManager.isSplitScreen = isSplitScreenEnabled;
        
        // Reset Lives before game starts
        multiplayerManager.ResetPlayerLives();

        // Set to Gameplay
        ChangeState(GameState.Gameplay); 
        
        // Start the actual game
        gameManager.StartGame();
    }
}