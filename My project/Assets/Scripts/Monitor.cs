using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class Monitor : MonoBehaviour
{
    public Queue<PopUp> PopUps = new Queue<PopUp>();
     public List<Player> players = new List<Player>();
    public GameObject[] popUpPrefabs; // Define an array of prefab objects
    public TextMeshProUGUI scoreText;

    // Constructor
    void Start()
    {
        Player p1 = createPlayer(0);
        players.Add(p1);

        int numPopups = 30; // Replace with your desired number of popups
        for (int i = 0; i < numPopups; i++)
        {
            PopUp popup = InstantiateRandomPopUpPrefab();
            PopUps.Enqueue(popup);
        }
    }

    private void Update() {
         if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call DestroyPopUp for the top pop-up if there are pop-ups in the queue
            if (PopUps.Count > 0)
            {
                PopUp currentPopup = PopUps.Dequeue();
                currentPopup.DestroyPopUp();
                players[0].ChangeScore(1);
                UpdateScoreText();
            }
        }
        if (PopUps.Count < 1) {
            SceneManager.LoadScene(6);
        }
    }

    Player createPlayer(int initialScore)
    {
        Player newPlayer = new Player();
        newPlayer.Score = initialScore;
        newPlayer.monitor = this; // Set the monitor reference

        return newPlayer;
    }

    void UpdateScoreText()
    {
        // Update the TMP score text with the current score of p1
        if (scoreText != null && players.Count > 0)
        {
            scoreText.text = "P1: " + players[0].Score.ToString();
        }
    }

    // Instantiate a random popup prefab
    PopUp InstantiateRandomPopUpPrefab()
    {
        int randNum = Random.Range(0, popUpPrefabs.Length - 1);
        GameObject popupPrefab = popUpPrefabs[randNum];

        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            float xViewport = Random.Range(0f, 1f); // Random x position in viewport space (0 to 1)
            float yViewport = Random.Range(0f, 1f); // Random y position in viewport space (0 to 1)

            Vector3 spawnPosition = new Vector3(xViewport, yViewport, mainCamera.nearClipPlane);
            Vector3 worldPosition = mainCamera.ViewportToWorldPoint(spawnPosition);

            PopUp result = Instantiate(popupPrefab, worldPosition, Quaternion.identity).GetComponent<PopUp>();
            return result;
        }
        else
        {
            Debug.LogError("Main camera not found.");
            return null; // Handle the case where there's no main camera
        }
    }

    // Handle a player clicking a popup
    public void ClickPopUp(Player player)
    {
        if (PopUps.Count > 0)
        {
            PopUp currentPopup = PopUps.Dequeue();
            int scoreChange = currentPopup.PopUpClicked();
            player.ChangeScore(scoreChange);
            currentPopup.DestroyPopUp();
        }
    }
}