using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip sfxSound;
    private bool hasPlayed;

    void Start()
    {
        sfxSource.clip = sfxSound;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!sfxSource.isPlaying && !hasPlayed)
        {
            sfxSource.Play();
            hasPlayed = true;
        }
    }

}
