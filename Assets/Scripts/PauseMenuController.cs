using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    public RectTransform menuHightlight;
    public RectTransform[] menuItems;
    private int menuIndex = 0;
    private float movement;
    private float oldMovement;
    private float holdTime = 0;

    void OnEnable()
    {
        menuItems[2].GetComponent<TMP_Text>().text = AudioController.Instance.GetSFXStatus() ? "Sound Effect: On" : "Sound Effect: Off";
        menuItems[3].GetComponent<TMP_Text>().text = AudioController.Instance.GetBGMStatus() ? "Background Music: On" : "Background Music: Off";
    }
    // Update is called once per frame
    void Update()
    {
        // Using Raw for unfiltered input, no smoothing applied
        movement = Mathf.Round(Input.GetAxisRaw("Vertical"));

        if (movement != 0 && Math.Abs(movement - oldMovement) < 0.01f)
        {
            if (holdTime < 0.5f)
            {
                holdTime += Time.deltaTime;
            }
            else
            {
                movement = 0;
                holdTime = 0;
            }
        }
        else
        {
            holdTime = 0;
        }

        if (Math.Abs(movement - oldMovement) > 0.01f)
        {
            if (movement > 0)
            {
                // move up
                if (menuIndex > 0)
                {
                    AudioController.Instance.MenuNavigated();
                    menuIndex--;
                    menuHightlight.DOAnchorPos(menuItems[menuIndex].anchoredPosition, 0.15f);
                }
            }
            else if (movement < 0)
            {
                // move down
                if (menuIndex < menuItems.Length - 1)
                {
                    AudioController.Instance.MenuNavigated();
                    menuIndex++;
                    menuHightlight.DOAnchorPos(menuItems[menuIndex].anchoredPosition, 0.15f);
                }
            }

            oldMovement = movement;
        }

        if (Input.GetButtonDown("Submit"))
        {
            AudioController.Instance.MenuSelected();
            switch (menuIndex)
            {
                case 0:
                    // unpause
                    GameManager.Instance.Unpause();
                    break;
                case 1:
                    gameObject.SetActive(false);
                    GameManager.Instance.RestartLevel();
                    break;
                case 2:
                    menuItems[menuIndex].GetComponent<TMP_Text>().text = AudioController.Instance.ToggleSFX() ? "Sound Effect: On" : "Sound Effect: Off";
                    break;
                case 3:
                    menuItems[menuIndex].GetComponent<TMP_Text>().text = AudioController.Instance.ToggleBGM() ? "Background Music: On" : "Background Music: Off";
                    break;
                case 4:
                    gameObject.SetActive(false);
                    GameManager.Instance.QuitToMainMenu();
                    break;
            }
        }
    }
}
