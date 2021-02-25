using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void LoadMainLevel()
    {
       SceneManager.LoadScene("Main");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
