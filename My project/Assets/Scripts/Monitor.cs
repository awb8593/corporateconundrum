using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Monitor : MonoBehaviour
{
    public Queue<PopUp> PopUps = new Queue<PopUp>();
     public List<Player> players = new List<Player>();
    public GameObject[] popUpPrefabs; // Define an array of prefab objects
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI adCaption;
    public GameObject adCaptionBox;
    public AudioSource audioSource;
    public AudioSource countdownAudio;
    private bool isRemovingAd = false;
    private Coroutine adRemovalCoroutine;

    // Constructor
    void Start()
    {
        Player p1 = createPlayer(0);
        players.Add(p1);

        adCaption.gameObject.SetActive(false);
        adCaptionBox.gameObject.SetActive(false);

        int numPopups = 30; // Replace with your desired number of popups
        for (int i = 0; i < numPopups; i++)
        {
            PopUp popup = InstantiateRandomPopUpPrefab();
            PopUps.Enqueue(popup);
        }
    }

    private void Update()
    {
        UpdateScoreText();
        if (PopUps.Count > 0)
        {
            PopUp[] popUpArray = PopUps.ToArray(); // Convert the Queue to an array
            int lastIndex = popUpArray.Length - 1;
            PopUp currentPopup = popUpArray[lastIndex]; // Access the last element in the array

            if (currentPopup.type == "banzai" && !currentPopup.enlarged)
            {
                EnlargePopUp(currentPopup);
                currentPopup.enlarged = true;
            } else {
                if (!currentPopup.enlarged) {
                    EnlargePopUp(currentPopup);
                }
                if (!isRemovingAd && currentPopup.type == "ad")
                {
                    removeAd();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isRemovingAd = false;
                adCaption.gameObject.SetActive(false);
                adCaptionBox.gameObject.SetActive(false);
                if (countdownAudio != null && countdownAudio.isPlaying)
                {
                    countdownAudio.Stop();
                }
                // Call DestroyPopUp for the last pop-up if space is pressed
                if (PopUps.Count > 0)
                {
                    PopUp lastPopup = DequeueLast(PopUps);
                    lastPopup.DestroyPopUp();
                    if(lastPopup.type == "ad") {
                        players[0].ChangeScore(-3);
                    } else {
                        players[0].ChangeScore(1);
                    }
                    
                }
            }
        }

        if (PopUps.Count < 1)
        {
            SavePlayerData();
            SceneManager.LoadScene(6);
        }
    }

    private void removeAd()
    {
        adCaption.gameObject.SetActive(true);
        adCaptionBox.gameObject.SetActive(true);
        // play audio queue, wait 6 seconds, call DequeueLast(PopUps), and call DestroyPopUp 
        if (!isRemovingAd)
        {
            isRemovingAd = true;
            if (countdownAudio != null)
            {
                countdownAudio.Play();
            }
            adRemovalCoroutine = StartCoroutine(WaitAndDestroyAd(8.5f)); // Wait for 6 seconds and then destroy the last ad
        }
    }

    private IEnumerator WaitAndDestroyAd(float waitTime)
    {
        float startTime = Time.time;

        while (Time.time - startTime < waitTime)
        {
            if (!isRemovingAd)
            {
                yield break; // Stop the coroutine if ad removal is canceled
            }
            yield return null; // Wait for the next frame
        }

        adCaption.gameObject.SetActive(false);
        adCaptionBox.gameObject.SetActive(false);

        if (PopUps.Count > 0)
        {
            PopUp lastAd = DequeueLast(PopUps);
            lastAd.DestroyPopUp();
        }

        isRemovingAd = false;
        adRemovalCoroutine = null; // Reset the coroutine reference
    }


    // Dequeue the last element from the Queue
    private PopUp DequeueLast(Queue<PopUp> queue)
    {
        PopUp lastItem = default(PopUp);

        if (queue.Count > 0)
        {
            PopUp[] popUpArray = queue.ToArray();
            lastItem = popUpArray[popUpArray.Length - 1];
            queue.Clear(); // Clear the queue
            foreach (var item in popUpArray)
            {
                if (!item.Equals(lastItem))
                {
                    queue.Enqueue(item); // Enqueue all elements except the last one
                }
            }
        }

        return lastItem;
    }


    void EnlargePopUp(PopUp popup)
    {
        // Adjust the scale of the PopUp to make it larger
        float scaleFactor = 0.8f;
        popup.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        popup.enlarged = true;
        if (audioSource != null && popup.type != "ad") {
            audioSource.Play();
        }
    }

    Player createPlayer(int initialScore)
    {
        Player newPlayer = new Player();
        newPlayer.Score = initialScore;
        newPlayer.monitor = this; // Set the monitor reference

        return newPlayer;
    }

    void SavePlayerData()
    {
        for (int i = 0; i < players.Count; i++)
        {
            // Save each player's score using PlayerPrefs
            PlayerPrefs.SetInt("PlayerScore" + i, players[i].Score);
        }

        // Save the count of players
        PlayerPrefs.SetInt("PlayerCount", players.Count);

        // Call PlayerPrefs.Save() to save the data
        PlayerPrefs.Save();
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
        int randNum = Random.Range(0, popUpPrefabs.Length);
        GameObject popupPrefab = popUpPrefabs[randNum];

        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            float xViewport = Random.Range(0.4f, 0.6f); // Random x position in viewport space
            float yViewport = Random.Range(0.4f, 0.6f); // Random y position in viewport space

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