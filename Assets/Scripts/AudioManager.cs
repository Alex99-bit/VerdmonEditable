using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource controlAudio;
    [SerializeField] private AudioClip[] audios;

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
    }

    public void SelectAudio(int indice, float volumen)
    {
        controlAudio.PlayOneShot(audios[indice], volumen);
    }
}
