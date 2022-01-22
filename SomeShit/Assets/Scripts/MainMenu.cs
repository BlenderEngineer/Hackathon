using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play_EventHandler()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Exit_EventHandler()
    {
        Application.Quit();
    }
}
