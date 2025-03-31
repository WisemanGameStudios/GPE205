using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviour
{
    // Prefabs
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameObject player1CamPrefab;
    public GameObject player2CamPrefab;
    public GameObject mainCameraPrefab;

    private Camera player1Camera;
    private Camera player2Camera;
    private Camera mainCamera;

    public List<PawnSpawnPoint> pawnSpawnPoints = new List<PawnSpawnPoint>();
    public bool isSplitScreen = false;

    // Lives
    public LivesUI livesUI; // Drag and drop in Inspector
    public int maxLives = 3;
    private int player1Lives;
    private int player2Lives;

    private GameStateManager gameStateManager;

    void Start()
    {
        player1Lives = maxLives;
        player2Lives = maxLives;

        if (livesUI != null)
        {
            livesUI.SetLives(1, player1Lives);
            livesUI.SetLives(2, player2Lives);
        }
        
        gameStateManager = GameStateManager.Instance;
        SpawnPlayers();
        
        
    }
    
    public void ResetPlayerLives()
    {
        player1Lives = maxLives;
        player2Lives = maxLives;

        if (livesUI != null)
        {
            livesUI.SetLives(1, player1Lives);
            livesUI.SetLives(2, player2Lives);
        }
    }

    public void SpawnPlayers()
    {
        pawnSpawnPoints = new List<PawnSpawnPoint>(FindObjectsOfType<PawnSpawnPoint>());

        if (pawnSpawnPoints.Count < 2)
        {
            return;
        }

        var spawn1 = GetRandomSpawn();
        var spawn2 = GetRandomSpawn();

        GameObject p1 = Instantiate(player1Prefab, spawn1.position, Quaternion.identity);
        Camera cam1 = isSplitScreen ? SetupSplitScreenCamera(1, p1) : SetupMainCamera(p1);
        SetupPlayer(p1, 1, cam1);

        GameObject p2 = Instantiate(player2Prefab, spawn2.position, Quaternion.identity);
        Camera cam2 = isSplitScreen ? SetupSplitScreenCamera(2, p2) : null;
        SetupPlayer(p2, 2, cam2);
    }

    void SetupPlayer(GameObject player, int playerNumber, Camera cam)
    {
        if (playerNumber == 1 && player.TryGetComponent(out Player1Test p1))
        {
            p1.SetupCamera(cam, !isSplitScreen);
            p1.OnDeath += (id, obj) => RegisterDeath(1, obj);
        }
        else if (playerNumber == 2 && player.TryGetComponent(out Player2Test p2))
        {
            p2.SetupCamera(cam, !isSplitScreen);
            p2.OnDeath += (id, obj) => RegisterDeath(2, obj);
        }

        if (player.TryGetComponent(out TankHealthBar bar))
        {
            bar.SetCamera(cam);
            if (player.TryGetComponent(out Health health))
            {
                bar.SetMaxHealth(health.maxHealth);
                bar.SetHealth(health.currentHealth);
            }
        }
    }

    Camera SetupSplitScreenCamera(int playerNumber, GameObject target)
    {
        GameObject camObj = Instantiate(playerNumber == 1 ? player1CamPrefab : player2CamPrefab);
        Camera cam = camObj.GetComponent<Camera>();

        cam.transform.SetParent(target.transform);
        cam.transform.localPosition = new Vector3(0, 6, -8);
        cam.transform.localRotation = Quaternion.Euler(20, 0, 0);
        cam.rect = playerNumber == 1 ? new Rect(0f, 0f, 0.5f, 1f) : new Rect(0.5f, 0f, 0.5f, 1f);
        cam.gameObject.SetActive(true);

        if (playerNumber == 1) player1Camera = cam;
        else player2Camera = cam;

        return cam;
    }

    Camera SetupMainCamera(GameObject target)
    {
        GameObject camObj = Instantiate(mainCameraPrefab);
        Camera cam = camObj.GetComponent<Camera>();
        cam.transform.SetParent(target.transform);
        cam.transform.localPosition = new Vector3(0, 8, -14);
        cam.transform.LookAt(target.transform.position + Vector3.up * 2);
        cam.rect = new Rect(0, 0, 1, 1);
        cam.gameObject.SetActive(true);

        mainCamera = cam;
        return cam;
    }

    Transform GetRandomSpawn()
    {
        int index = Random.Range(0, pawnSpawnPoints.Count);
        Transform spawn = pawnSpawnPoints[index].transform;
        pawnSpawnPoints.RemoveAt(index);
        return spawn;
    }

    void RegisterDeath(int playerNumber, GameObject playerObject)
    {
        if (playerNumber == 1) player1Lives--;
        else player2Lives--;

        if (livesUI != null)
        {
            livesUI.SetLives(1, player1Lives);
            livesUI.SetLives(2, player2Lives);
        }

        if (player1Lives <= 0 || player2Lives <= 0)
        {
            Debug.Log("GAME OVER");
            gameStateManager?.ShowGameOverScreen();
        }
        else
        {
            StartCoroutine(RespawnWithDelay(playerNumber));
        }
    }

    IEnumerator RespawnWithDelay(int playerNumber)
    {
        yield return new WaitForSeconds(0.5f);

        var spawn = GetRandomSpawn();
        GameObject prefab = playerNumber == 1 ? player1Prefab : player2Prefab;
        GameObject playerObj = Instantiate(prefab, spawn.position, Quaternion.identity);
        Camera cam = playerNumber == 1
            ? (isSplitScreen ? SetupSplitScreenCamera(1, playerObj) : SetupMainCamera(playerObj))
            : (isSplitScreen ? SetupSplitScreenCamera(2, playerObj) : null);

        SetupPlayer(playerObj, playerNumber, cam);
    }
}