using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject pauseCanvas;
    public TextMeshProUGUI currentPowerText;
    // Start is called before the first frame update
    public GameObject equationSign;
    public TMP_Text[] numberTexts;
    public GameObject[] minusSigns;
    public TMP_Text expectedResultText;
    public TMP_Text coinText;
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

    public void ShowEquation()
    {
        equationSign.SetActive(true);
    }

    public void UpdateEquation(List<string> numbers, string result)
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            numberTexts[i].gameObject.SetActive(true);
            numberTexts[i].text = numbers[i];
        }

        for (int i = 0; i < numbers.Count - 1; i++)
        {
            minusSigns[i].gameObject.SetActive(true);
        }

        expectedResultText.text = result;
    }

    public void HideEquation()
    {
        foreach (var numberText in numberTexts)
        {
            numberText.gameObject.SetActive(false);
        }

        foreach (var minusSign in minusSigns)
        {
            minusSign.SetActive(false);
        }
        equationSign.SetActive(false);
    }

    public void UpdateCoinUI(int amount)
    {
        coinText.text = amount.ToString();
    }
}
