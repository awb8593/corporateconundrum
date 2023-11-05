using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    List<Player> players = new List<Player>();
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        int playerCount = PlayerPrefs.GetInt("PlayerCount", 0);

        for (int i = 0; i < playerCount; i++)
        {
            Player player = new Player();
            player.Score = PlayerPrefs.GetInt("PlayerScore" + i, 0);
            // Add any other player-specific data you need to load here

            players.Add(player);
        }
    }

    void UpdateScoreText()
    {
        // Update the TMP score text with the current score of p1
        if (scoreText != null && players.Count > 0)
        {
            scoreText.text = "P1: " + players[0].Score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreText();
    }
}
