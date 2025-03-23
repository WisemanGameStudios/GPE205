using UnityEngine;
using System;

public class Player1Test : MonoBehaviour
{
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode shootKey = KeyCode.Space;

    private Pawn pawn;
    private Camera playerCamera;
    protected AudioSource _audioSource;
    public AudioClip moveSound;
    public AudioClip shootSound;

    private bool _isMoving = false;
    private AudioSource _shootAudioSource;

    public event Action<int, GameObject> OnDeath;

    void Start()
    {
        pawn = GetComponent<Pawn>();

        if (pawn == null)
            Debug.LogError("No Pawn component found on Player 1!");

        SetupAudio();

        // Try to find and enable camera if it exists as a child
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>(true);
            if (playerCamera != null)
                playerCamera.gameObject.SetActive(true);
        }

        // Hook up death event
        var health = GetComponent<Health>();
        if (health != null)
            health.OnDeath += (obj) => Die();
    }

    
    // Set up the camera, viewport, and parenting for Player 1.
    public void SetupCamera(Camera cam, bool isSinglePlayer = false)
    {
        if (cam == null) return;

        cam.transform.SetParent(transform);
        cam.transform.localPosition = new Vector3(0, 6, -8);
        cam.transform.localRotation = Quaternion.Euler(20, 0, 0);
        cam.enabled = true;

        cam.rect = isSinglePlayer ? new Rect(0, 0, 1, 1) : new Rect(0, 0, 0.5f, 1);
    }

    private void SetupAudio()
    {
        if (!TryGetComponent(out _audioSource))
            _audioSource = gameObject.AddComponent<AudioSource>();

        if (_shootAudioSource == null)
            _shootAudioSource = gameObject.AddComponent<AudioSource>();

        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0;

        if (moveSound != null)
            _audioSource.clip = moveSound;

        _shootAudioSource.playOnAwake = false;
        _shootAudioSource.loop = false;
        _shootAudioSource.spatialBlend = 0;
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
        if (Input.GetKeyDown(shootKey)) { pawn.Shoot(); PlayShootSound(); }

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
            _audioSource.Stop();
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
        OnDeath?.Invoke(1, gameObject);
        Destroy(gameObject);
    }
}