using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackOut : MonoBehaviour
{
    private static int previousSceneIndex;

    public void GoToSettingsScene()
    {
        // Store the current scene index as the previous scene
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the settings scene 
        SceneManager.LoadScene(5);
    }

    public void ReturnToPreviousScene()
    {
        // Load the previous scene
        SceneManager.LoadScene(previousSceneIndex);
    }
}
