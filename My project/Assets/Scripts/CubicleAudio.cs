using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubicleAudio : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the scene name matches the scene you want the audio to play in
        if (scene.name == "CubicleChaos")
        {
            audioSource.Play();
        }
    }

    // Be sure to unsubscribe from the event when this script is destroyed
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
