using System;
using UnityEngine;

[System.Serializable]
public class PlayerController : Controller
{
    // Keyboard KeyCodes
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode shootKey = KeyCode.Space;

    // Audio Variables 
    private AudioSource _audioSource;
    public AudioClip moveSound;
    public AudioClip shootSound;

    private bool _isMoving = false;

    // Start before first frame update 
    public override void Start()
    {
        // Get or Add the AudioSource component
        if (!TryGetComponent(out _audioSource))
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("AudioSource was missing, added automatically.");
        }

        // Assign the movement sound to the AudioSource
        if (moveSound != null)
        {
            _audioSource.clip = moveSound;
            _audioSource.loop = true; // Enable looping
            _audioSource.playOnAwake = false; // Don't play on start
        }
        else
        {
            Debug.LogError("Move sound is not assigned in the Inspector for " + gameObject.name);
        }

        // If GameManager exists
        if (GameManager.instance != null && GameManager.instance.players != null)
        {
            GameManager.instance.players.Add(this);
        }

        base.Start();
    }

    public void OnDestroy()
    {
        if (GameManager.instance != null && GameManager.instance.players != null)
        {
            GameManager.instance.players.Remove(this);
        }
    }

    // Update Function 
    public override void Update()
    {
        ProcessInputs();
        base.Update();
    }

    public override void ProcessInputs()
    {
        bool keyPressed = false;

        if (Input.GetKey(moveUp))
        {
            pawn.MoveUp();
            pawn.MakeNoise();
            keyPressed = true;
        }

        if (Input.GetKey(moveLeft))
        {
            pawn.RotateLeft();
            pawn.MakeNoise();
            keyPressed = true;
        }

        if (Input.GetKey(moveDown))
        {
            pawn.MoveDown();
            pawn.MakeNoise();
            keyPressed = true;
        }

        if (Input.GetKey(moveRight))
        {
            pawn.RotateRight();
            pawn.MakeNoise();
            keyPressed = true;
        }

        if (Input.GetKeyDown(shootKey))
        {
           pawn.Shoot();
           pawn.MakeNoise();
           PlayShootSound();
        }

        // Handle loop
        if (keyPressed)
        {
            if (!_isMoving)
            {
                PlayMoveSound();
                _isMoving = true;
            }
        }
        else
        {
            if (_isMoving)
            {
                StopMoveSound();
                _isMoving = false;
            }
        }

        if (!Input.GetKey(moveUp) && !Input.GetKey(moveDown) && !Input.GetKey(moveLeft) && !Input.GetKey(moveRight) && Input.GetKeyDown(shootKey))
        {
            pawn.StopNoise();
        }
    }

    // Play looping movement sound
    private void PlayMoveSound()
    {
        if (_audioSource != null && moveSound != null)
        {
            if (!_audioSource.isPlaying) // Prevent multiple
            {
                _audioSource.Play();
            }
        }
    }

    // Stop movement sound when not moving
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
            _audioSource.PlayOneShot(shootSound);
        }
    }
}

 
