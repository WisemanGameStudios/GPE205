using System;
using UnityEngine;

public class PlayerController : Controller
{
    protected AudioSource _audioSource;

    public KeyCode moveUp;
    public KeyCode moveLeft;
    public KeyCode moveDown;
    public KeyCode moveRight;
    public KeyCode shootKey;

    public AudioClip moveSound;
    public AudioClip shootSound;

    public Camera playerCamera;
    private bool _isMoving = false;

    public override void Start()
    {
        base.Start();
        SetupAudio();
        RegisterPlayer();
    }

    private void SetupAudio()
    {
        if (!TryGetComponent(out _audioSource))
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (moveSound != null)
        {
            _audioSource.clip = moveSound;
            _audioSource.loop = true;
            _audioSource.playOnAwake = false;
        }
    }

    private void RegisterPlayer()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.players.Add(this);
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.players.Remove(this);
        }
    }

    public override void Update()
    {
        if (pawn == null) return;
        ProcessInputs();
        base.Update();
    }

    public override void ProcessInputs()
    {
        bool keyPressed = false;

        if (Input.GetKey(moveUp))
        {
            Debug.Log("Moving Up"); // Debug
            pawn.MoveUp();
            keyPressed = true;
        }

        if (Input.GetKey(moveLeft))
        {
            Debug.Log("Rotating Left"); // Debug
            pawn.RotateLeft();
            keyPressed = true;
        }

        if (Input.GetKey(moveDown))
        {
            Debug.Log("Moving Down"); // Debug
            pawn.MoveDown();
            keyPressed = true;
        }

        if (Input.GetKey(moveRight))
        {
            Debug.Log("Rotating Right"); // Debug
            pawn.RotateRight();
            keyPressed = true;
        }

        if (Input.GetKeyDown(shootKey))
        {
            Debug.Log("Shooting"); // Debug
            pawn.Shoot();
            PlayShootSound();
        }

        // Ensure movement sound
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
            _audioSource.volume = AudioManager.Instance.sfxSource.volume;
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
        if (_audioSource != null && shootSound != null)
        {
            _audioSource.volume = AudioManager.Instance.sfxSource.volume;
            _audioSource.PlayOneShot(shootSound);
        }
    }
}