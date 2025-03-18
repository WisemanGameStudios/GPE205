using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    public int mapSeed;
    public bool isMapOfTheDay = false;
    public bool isRandomSeed = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateSeed();
        Debug.Log("Loaded Seed in Old-Main: " + mapSeed);
    }

    public void GenerateSeed()
    {
        if (isMapOfTheDay)
        {
            mapSeed = DateToInt(DateTime.Now.Date);
        }
        else if (isRandomSeed)
        {
            mapSeed = DateToInt(DateTime.Now);
        }

        Debug.Log($"Generated Seed: {mapSeed}");
    }
    public int DateToInt(DateTime dateToUse)
    {
        return dateToUse.Year + dateToUse.Month + dateToUse.Day +
               dateToUse.Hour + dateToUse.Minute + dateToUse.Second +
               dateToUse.Millisecond;
    }
}
