using UnityEngine;
using System;

public class Player2Test : MonoBehaviour
{
    public KeyCode moveUp = KeyCode.UpArrow;
    public KeyCode moveLeft = KeyCode.LeftArrow;
    public KeyCode moveDown = KeyCode.DownArrow;
    public KeyCode moveRight = KeyCode.RightArrow;
    public KeyCode shootKey = KeyCode.Return;

    private Pawn pawn;
    private Camera playerCamera;
    private AudioSource _audioSource;
    private AudioSource _shootAudioSource;

    public AudioClip moveSound;
    public AudioClip shootSound;

    private bool _isMoving = false;

    public event Action<int, GameObject> OnDeath;

    void Start()
    {
        pawn = GetComponent<Pawn>();
        if (pawn == null)
        {
            Debug.LogError("No Pawn component found on Player 2!");
        }

        // Try to find and activate child camera
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>(true);
            if (playerCamera != null)
                playerCamera.gameObject.SetActive(true);
        }

        SetupAudio();

        // Subscribe to health death event
        var health = GetComponent<Health>();
        if (health != null)
            health.OnDeath += (obj) => Die();
    }

    public void SetupCamera(Camera cam, bool isSinglePlayer = false)
    {
        playerCamera = cam;

        if (playerCamera == null)
        {
            return;
        }

        playerCamera.transform.SetParent(transform);
        playerCamera.transform.localPosition = new Vector3(0, 6, -8);
        playerCamera.transform.rotation = Quaternion.Euler(20, 0, 0);
        playerCamera.gameObject.SetActive(true);
        playerCamera.enabled = true;

        if (isSinglePlayer)
        {
            playerCamera.rect = new Rect(0, 0, 1, 1); // Fullscreen
        }
        else
        {
            playerCamera.rect = new Rect(0.5f, 0, 0.5f, 1); // Right half
        }

        Debug.Log($"Player 2 camera initialized. SinglePlayer = {isSinglePlayer}");
    }

    private void SetupAudio()
    {
        // Movement AudioSource
        if (!TryGetComponent(out _audioSource))
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0f;
        if (moveSound != null)
        {
            _audioSource.clip = moveSound;
        }

        // Shooting AudioSource
        if (_shootAudioSource == null)
        {
            _shootAudioSource = gameObject.AddComponent<AudioSource>();
        }

        _shootAudioSource.loop = false;
        _shootAudioSource.playOnAwake = false;
        _shootAudioSource.spatialBlend = 0f;
    }

    void Update()
    {
        ProcessInputs();
    }

    public void ProcessInputs()
    {
        if (pawn == null) return;

        bool keyPressed = false;

        if (Input.GetKey(moveUp)) { pawn.MoveUp(); keyPressed = true; }
        if (Input.GetKey(moveLeft)) { pawn.RotateLeft(); keyPressed = true; }
        if (Input.GetKey(moveDown)) { pawn.MoveDown(); keyPressed = true; }
        if (Input.GetKey(moveRight)) { pawn.RotateRight(); keyPressed = true; }

        if (Input.GetKeyDown(shootKey))
        {
            pawn.Shoot();
            PlayShootSound();
        }

        HandleMovementSound(keyPressed);
    }

    private void HandleMovementSound(bool keyPressed)
    {
        if (keyPressed && !_isMoving)
        {
            PlayMoveSound();
            _isMoving = true;
        }
        else if (!keyPressed && _isMoving)
        {
            StopMoveSound();
            _isMoving = false;
        }
    }

    private void PlayMoveSound()
    {
        if (_audioSource != null && moveSound != null && !_audioSource.isPlaying)
        {
            _audioSource.volume = AudioManager.Instance?.sfxSource.volume ?? 1f;
            _audioSource.Play();
        }
    }

    private void StopMoveSound()
    {
        if (_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    private void PlayShootSound()
    {
        if (_shootAudioSource != null && shootSound != null)
        {
            _shootAudioSource.volume = AudioManager.Instance?.sfxSource.volume ?? 1f;
            _shootAudioSource.PlayOneShot(shootSound);
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        OnDeath?.Invoke(2, gameObject); // Let MultiplayerManager know Player 2 died
        Destroy(gameObject);
    }
}