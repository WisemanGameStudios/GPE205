using System;
using UnityEngine;
using UnityEngine.UI;

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
    private InputField seedInputField;

    private GameState currentState;
    private GameManager gameManager;

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
        gameManager = GameManager.instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        ChangeState(GameState.TitleScreen);
        Time.timeScale = 1;
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
        gameManager.isMapOfTheDay = !value; // Ensure only one mode is selected
        gameManager.GenerateSeed(); // Generate a new random seed
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
        UIContainer.SetActive(false); // Hide UI when game starts
        gameManager.StartGame();
    }
}