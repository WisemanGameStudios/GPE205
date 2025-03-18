using UnityEngine;

public class Player1Controller : PlayerController
{
    public override void Start()
    {
        playerNumber = 1;
        moveUp = KeyCode.W;
        moveLeft = KeyCode.A;
        moveDown = KeyCode.S;
        moveRight = KeyCode.D;
        shootKey = KeyCode.Space;

        base.Start();
        SetupCamera();
    }

    private void SetupCamera()
    {
        if (playerCamera != null)
        {
            playerCamera.rect = new Rect(0, 0, 0.5f, 1); // Left half of the screen
            AttachCameraToPawn();
        }
    }

    private void AttachCameraToPawn()
    {
        if (pawn != null && playerCamera != null)
        {
            playerCamera.transform.SetParent(pawn.transform);
            playerCamera.transform.localPosition = new Vector3(0, 10, -10);
            playerCamera.transform.LookAt(pawn.transform.position);
        }
    }
}
