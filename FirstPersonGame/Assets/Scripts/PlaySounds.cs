using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
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
        if (other.CompareTag("Player"))
        {
            if (!sfxSource.isPlaying && !hasPlayed)
            {
                sfxSource.PlayOneShot(sfxSound);
                hasPlayed = true;
            }
        }
    }
}
