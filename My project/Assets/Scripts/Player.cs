using UnityEngine;

public class Player : MonoBehaviour
{
    public Monitor monitor;
    public int Score;

    // Called when player clicks on a popup of type "A"
    public void ClickedA()
    {
        monitor.ClickPopUp(this);
    }

    // Change the player's score
    public void ChangeScore(int points)
    {
        Score += points;
    }
}