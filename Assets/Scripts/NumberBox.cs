using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberBox : MonoBehaviour
{
    public int initialNumberValue;
    public int maxOperations = 5;
    private int numberValue;
    private int operationsCount = 0;

    public TMP_Text numberText;
    void Start()
    {
        numberText.text = numberValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateNumber(int number)
    {
        this.numberValue = number;
        numberText.text = number.ToString();
    }

    public void ResetOperations()
    {
        operationsCount = 0;
        numberValue = initialNumberValue;
    }

    public void SetNumber(int newValue)
    {
        if (operationsCount < maxOperations)
        {
            numberValue = newValue;
            operationsCount++;
        }
    }

    public int GetNumberValue()
    {
        return numberValue;
    }
}
