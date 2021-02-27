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

    public AudioClip backgroundMusic;
    public AudioClip menuPositiveSFX;
    public AudioClip menuNegativeSFX;

    private void Start()
    {
        
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        
    }

    public void PlayBackgroundMusic()
    {

    }

}
