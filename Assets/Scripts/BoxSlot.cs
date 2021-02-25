using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSlot : MonoBehaviour
{
    private NumberBox boxOnTop;

    private List<SlotsObserver> observers = new List<SlotsObserver>();
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update()
    // {
    //
    // }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Box"))
        {

            boxOnTop = col.gameObject.GetComponent<NumberBox>();
            boxOnTop.slot = this;
            Debug.Log("[BoxSlot] Got box with value: " + boxOnTop.GetNumberValue(), this);
            foreach (var slotsObserver in observers)
            {
                slotsObserver.CalculateResult();
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Box"))
        {
            boxOnTop.slot = null;
            boxOnTop = null;
            Debug.Log("[BoxSlot] Box detached", this);
            foreach (var slotsObserver in observers)
            {
                slotsObserver.CalculateResult();
            }
        }
    }

    public void AddObserver(SlotsObserver observer)
    {
        observers.Add(observer);
    }

    public NumberBox GetBoxOnTop()
    {
        return boxOnTop;
    }

    public void TriggerCalculateResult()
    {
        foreach (var slotsObserver in observers)
        {
            slotsObserver.CalculateResult();
        }
    }
}
