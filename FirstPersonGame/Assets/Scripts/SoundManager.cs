using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //public Volume[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public AudioClip[] sfxSounds;

    public void PlaySFX(string name)
    {
        sfxSource.clip = Array.Find(sfxSounds, X => X.name == name);
        sfxSource.Play();
    }

}
