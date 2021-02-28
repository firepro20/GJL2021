using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public Image blackScreen;
    public GameObject[] dialogues;
    public bool isIntro;
    public RectTransform creditText;

    private int dialogueIndex = 0;
    private bool isTransitioning = false;
    void Start()
    {
        blackScreen.DOFade(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !isTransitioning)
        {
            AudioController.Instance.MenuNavigated();
            if (dialogueIndex < dialogues.Length - 1)
            {
                dialogueIndex++;
                for (int i = 0; i < dialogues.Length; i++)
                {
                    dialogues[i].SetActive(i == dialogueIndex);
                }
            }
            else
            {
                isTransitioning = true;
                if (isIntro)
                {
                    AudioController.Instance.FadeToTrack(1);
                    blackScreen.DOFade(1f, 1f).OnComplete(() =>
                    {
                        SceneManager.LoadScene(2);
                    });
                }
                else
                {
                    // run credit
                    Sequence newSequence = DOTween.Sequence();
                    newSequence.Append(blackScreen.DOFade(1f, 1f));
                    newSequence.Append(creditText.DOAnchorPosY(750f, 15f).SetEase(Ease.Linear));
                    newSequence.AppendCallback(GameManager.Instance.QuitToMainMenu);
                }
            }
        }
    }
}
