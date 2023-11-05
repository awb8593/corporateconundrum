using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void Title()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Start()
    {
        SceneManager.LoadScene(2);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadJobBoard()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene(5);
    }

    public void LoadLeaderboard()
    {
        SceneManager.LoadScene(6);
    }

    public void LoadEndSeq()
    {
        SceneManager.LoadScene(7);
    }
}