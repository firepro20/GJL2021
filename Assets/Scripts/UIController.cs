using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject pauseCanvas;
    public TextMeshProUGUI currentPowerText;

    public GameObject equationSign;
    public TMP_Text[] numberTexts;
    public GameObject[] minusSigns;
    public TMP_Text expectedResultText;
    public TMP_Text coinText;
    public Image[] partyMembersUI;
    public GameObject partyMembersParent;

    public bool dialogueShowing = false;
    public GameObject dialogueBox;
    public TMP_Text dialogueText;

    public ColorDatabase colorDB;
    private void Start()
    {
        //PopulatePartyImages();
        for (int i = 0; i < partyMembersUI.Length; i++)
        {
            Material mat = Instantiate(partyMembersUI[i].material);
            mat.SetColor("_RedColorReplace", colorDB.colors[i]);
            partyMembersUI[i].material = mat;
        }
    }

    void Update()
    {
        if (dialogueShowing && Input.anyKeyDown)
        {
            HideDialogue();
        }
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
                currentPower = "ADD (+3)";
                break;
            case Power.MULTIPLY:
                currentPower = "MULTIPLY (*2)";
                break;
            case Power.POWER:
                currentPower = "POWER (^2)";
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

    public void UpdateEquation(List<string> numbers, List<Color> colors, string result)
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            numberTexts[i].gameObject.SetActive(true);
            numberTexts[i].text = numbers[i];
            numberTexts[i].color = colors[i];
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

    public void UpdatePartyMembersUI(int[] discoveredPowers)
    {
        for (int i = 0; i < discoveredPowers.Length; i++)
        {
            if(discoveredPowers[i] == 1)
            {
                partyMembersUI[i].gameObject.SetActive(true);
            }
            else
            {
                partyMembersUI[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowDialogue(string text)
    {
        dialogueBox.SetActive(true);
        dialogueText.text = text;
        dialogueShowing = true;
        GameManager.Instance.SetGameState(GameState.Paused);
    }

    public void HideDialogue()
    {
        dialogueShowing = false;
        dialogueBox.SetActive(false);
        GameManager.Instance.SetGameState(GameState.Playing);
    }
}
