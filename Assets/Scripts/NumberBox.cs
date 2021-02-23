using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberBox : MonoBehaviour
{
    public int number;

    public TMP_Text numberText;
    void Start()
    {
        numberText.text = number.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateNumber(int number)
    {
        this.number = number;
        numberText.text = number.ToString();
    }
}
