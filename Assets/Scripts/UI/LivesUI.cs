using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LivesUI : MonoBehaviour
{
    public TMP_Text player1LivesText;
    public TMP_Text player2LivesText;

    public void SetLives(int playerNumber, int livesLeft)
    {
        if (playerNumber == 1 && player1LivesText != null)
        {
            player1LivesText.text = "Player 1 Lives: " + livesLeft;
        }
        else if (playerNumber == 2 && player2LivesText != null)
        {
            player2LivesText.text = "Player 2 Lives: " + livesLeft;
        }
    }
}