using UnityEngine;
using System.Collections.Generic;

public class Monitor : MonoBehaviour
{
    public Queue<PopUp> PopUps = new Queue<PopUp>();
    public Player[] players;
    public GameObject[] popUpPrefabs; // Define an array of prefab objects

    // Constructor
    void Start()
    {
        int numPopups = 10; // Replace with your desired number of popups
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
            }
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