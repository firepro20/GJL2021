using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject pauseCanvas;
    public TextMeshProUGUI currentPowerText;
    // Start is called before the first frame update
    void Start()
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
    /// <summary>
    /// Update current power text
    /// </summary>
    /// <param name="p"></param>
    public void UpdateCurrentPower(Power p)
    {
        string currentPower;
        switch (p)
        {
            case Power.MOVE:
                currentPower = "MOVE";
                break;
            case Power.ADD:
                currentPower = "ADD";
                break;
            case Power.MULTIPLY:
                currentPower = "MULTIPLY";
                break;
            case Power.POWER:
                currentPower = "POWER";
                break;
            case Power.DIVIDE:
                currentPower = "DIVIDE";
                break;
            case Power.RESET:
                currentPower = "RESET";
                break;
            default:
                currentPower = "MOVE";
                break;
        }
        currentPowerText.text = currentPower; 
    }
}
