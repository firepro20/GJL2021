using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioBehaviour>
{
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

    // Update is called once per frame
    void LateUpdate()
    {

    }
}
