using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;

    public static AudioController Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public AudioClip menuSelected;
    public AudioClip menuNavigate;
    public AudioClip doorOpen;
    public AudioClip coinReceived;
    public AudioClip partyJoined;
    public AudioClip partySwitched;
    public AudioClip nextLevel;
    public AudioClip boxOperated;
    public AudioClip boxPushed;

    public AudioClip[] bgm;

    public AudioSource[] audSrc;

    private bool soundEffectStatus = true;
    private bool bgmStatus = true;

    public void MenuNavigated()
    {
        audSrc[0].PlayOneShot(menuNavigate);
    }

    public void MenuSelected()
    {
        audSrc[0].PlayOneShot(menuSelected);
    }

    public void DoorOpened()
    {
        audSrc[0].PlayOneShot(doorOpen);
    }

    public void CoinReceived()
    {
        audSrc[0].PlayOneShot(coinReceived);
    }

    public void PartyJoined()
    {
        audSrc[0].PlayOneShot(partyJoined);
    }

    public void PartySwitched()
    {
        audSrc[0].PlayOneShot(partySwitched);
    }

    public void NextLevel()
    {
        audSrc[0].PlayOneShot(nextLevel);
    }

    public void BoxOperated()
    {
        audSrc[0].PlayOneShot(boxOperated);
    }

    public void BoxPushed()
    {
        audSrc[0].PlayOneShot(boxPushed);
    }

    public void FadeToTrack(int trackIndex)
    {
        if (bgmStatus)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(audSrc[1].DOFade(0f, 1f));
            seq.AppendCallback(() =>
            {
                audSrc[1].Stop();
                audSrc[1].clip = bgm[trackIndex];
                audSrc[1].Play();
            });
            seq.Append(audSrc[1].DOFade(0.8f, 1f));
        }
    }

    public bool ToggleBGM()
    {
        bgmStatus = !bgmStatus;
        audSrc[1].volume = bgmStatus ? 1 : 0;
        return bgmStatus;
    }

    public bool ToggleSFX()
    {
        soundEffectStatus = !soundEffectStatus;
        audSrc[0].volume = soundEffectStatus ? 1 : 0; 
        return soundEffectStatus;
    }

    public bool GetBGMStatus()
    {
        return bgmStatus;
    }

    public bool GetSFXStatus()
    {
        return soundEffectStatus;
    }
}
