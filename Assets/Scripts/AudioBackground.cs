using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBackground : MonoBehaviour
{
    private AudioSource controlAudio;

    private void Awake()
    {
        controlAudio = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(GameManager.instance.currentGameState == GameState.inGame || GameManager.instance.currentGameState == GameState.inicio)
        {
            controlAudio.Play();
        }
        else
        {
            controlAudio.Pause();
        }
    }
}
