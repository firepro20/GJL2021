using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject pauseCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPauseMenu(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                pauseCanvas.SetActive(true);
                break;
            case GameState.Paused:
                pauseCanvas.SetActive(false);
                break;
            default:
                break;
        }
    }
}
