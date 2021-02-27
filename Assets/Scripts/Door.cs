using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    private bool unlocked = false;
    private SpriteRenderer sprRenderer;

    public Sprite[] doorSprites;
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }

    public void Unlock()
    {
        if (!unlocked)
        {
            AudioController.Instance.DoorOpened();
        }
        unlocked = true;
        sprRenderer.sprite = doorSprites[1];
    }

    public void Lock()
    {
        unlocked = false;
        sprRenderer.sprite = doorSprites[0];
    }

    public bool GetUnlocked()
    {
        return unlocked;
    }
}
