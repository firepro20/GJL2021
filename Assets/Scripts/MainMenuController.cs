using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public RectTransform menuHightlight;
    public RectTransform[] menuItems;

    private int menuIndex = 0;
    private float movement;
    private float oldMovement;
    private float holdTime = 0;
    private bool isSelected = false;
    private Animator animator;

    void Start()
    {
        if (GameManager.Instance)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        animator = GetComponent<Animator>();
        Cursor.visible = false;
    }

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

        if (Input.GetButtonDown("Submit") && !isSelected)
        {
            switch (menuIndex)
            {
                case 0:
                    LoadMainLevel();
                    AudioController.Instance.MenuSelected();
                    isSelected = true;
                    break;
                case 1:
                    
                    break;
                case 2:
                    isSelected = true;
                    AudioController.Instance.MenuSelected();
                    Exit();
                    break;
            }

        }
    }

    public void LoadMainLevel()
    {
        animator.SetTrigger("FadeToBlack");
        StartCoroutine(loadMainLevel());
    }

    IEnumerator loadMainLevel()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Intro");
    }

    public void Exit()
    {
        animator.SetTrigger("FadeToBlack");
        Application.Quit();
    }
}
