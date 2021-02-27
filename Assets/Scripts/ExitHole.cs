using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHole : MonoBehaviour
{
    void OnTriggerEnter2D()
    {
        AudioController.Instance.NextLevel();
        // load next level
        GameManager.Instance.LoadNextLevel(false);
    }
}
