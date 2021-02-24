using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberBox : MonoBehaviour
{
    public int initialNumberValue;
    public int maxOperations = 5;
    private int numberValue;
    private int operationsCount = 0;

    public TMP_Text numberText;
    public Sprite[] operationIndicatorSprites;
    public Image[] operationIndicators;
    void Start()
    {
        numberText.text = numberValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        numberText.text = numberValue.ToString();
        for (int i = 0; i < maxOperations; i++)
        {
            if (i < operationsCount)
            {
                operationIndicators[i].sprite = operationIndicatorSprites[1];
            }
            else
            {
                operationIndicators[i].sprite = operationIndicatorSprites[0];
            }
        }
    }

    public void ResetOperations()
    {
        operationsCount = 0;
        numberValue = initialNumberValue;
        UpdateUI();
    }

    public void SetNumber(int newValue)
    {
        if (operationsCount < maxOperations)
        {
            numberValue = newValue;
            operationsCount++;
        }

        UpdateUI();
    }

    public int GetNumberValue()
    {
        return numberValue;
    }
}
