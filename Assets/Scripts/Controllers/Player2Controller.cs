using UnityEngine;

public class Player2Controller : PlayerController
{
    public override void Start()
    {
        moveUp = KeyCode.UpArrow;
        moveLeft = KeyCode.LeftArrow;
        moveDown = KeyCode.DownArrow;
        moveRight = KeyCode.RightArrow;
        shootKey = KeyCode.Return;

        base.Start();
        SetupCamera();
    }

    private void SetupCamera()
    {
        if (playerCamera != null)
        {
            playerCamera.rect = new Rect(0.5f, 0, 0.5f, 1); // Right half of the screen
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
