using System.Collections;
using System.Collections.Generic;
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

    private AudioSource audSrc;

    private void Start()
    {
        audSrc = GetComponent<AudioSource>();
    }

    public void MenuNavigated()
    {
        audSrc.PlayOneShot(menuNavigate);
    }

    public void MenuSelected()
    {
        audSrc.PlayOneShot(menuSelected);
    }

    public void DoorOpened()
    {
        audSrc.PlayOneShot(doorOpen);
    }

    public void CoinReceived()
    {
        audSrc.PlayOneShot(coinReceived);
    }

    public void PartyJoined()
    {
        audSrc.PlayOneShot(partyJoined);
    }

    public void PartySwitched()
    {
        audSrc.PlayOneShot(partySwitched);
    }

    public void NextLevel()
    {
        audSrc.PlayOneShot(nextLevel);
    }

    public void BoxOperated()
    {
        audSrc.PlayOneShot(boxOperated);
    }

    public void BoxPushed()
    {
        audSrc.PlayOneShot(boxPushed);
    }
}
