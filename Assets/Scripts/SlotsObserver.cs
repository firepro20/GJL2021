using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotsObserver : MonoBehaviour
{
    public UnityEvent solvedEvent;
    public UnityEvent unsolvedEvent;

    public BoxSlot[] boxSlots;
    public int expectedResult;
    void Start()
    {
        foreach (var boxSlot in boxSlots)
        {
            boxSlot.AddObserver(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateResult()
    {
        // also check if all slots are occupied
        if (boxSlots.Length > 0)
        {
            if (boxSlots[0].GetBoxOnTop())
            {
                int result = boxSlots[0].GetBoxOnTop().GetNumberValue();
                for (int i = 1; i < boxSlots.Length; i++)
                {
                    if (boxSlots[i].GetBoxOnTop())
                    {
                        result -= boxSlots[i].GetBoxOnTop().GetNumberValue();
                    }
                    else
                    {
                        unsolvedEvent.Invoke();
                        return;
                    }
                }

                if (result == expectedResult)
                {
                    solvedEvent.Invoke();
                }
                else
                {
                    unsolvedEvent.Invoke();
                }
            }
            else
            {
                unsolvedEvent.Invoke();
            }
        }
        else
        {
            Debug.LogWarning("[SlotsObserver] No BoxSlot is assigned in this SlotsObserver!", this);
        }
    }
}
