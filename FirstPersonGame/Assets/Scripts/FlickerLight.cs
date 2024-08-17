using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light lit;

    public float MinTime;
    public float MaxTime;
    public float Timer;

    //light sound effects i think?
    //public AudioSource AS;
    //public AudioClip LightAudio;

    // Start is called before the first frame update
    void Start()
    {
        Timer = Random.Range(MinTime, MaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        Flicker();
    }

    void Flicker()
    {
        if(Timer > 0)
            Timer -= Time.deltaTime;
        if(Timer <= 0)
        {
            lit.enabled = !lit.enabled;
            Timer = Random.Range(MinTime, MaxTime);
            //AS.PlayOneShot(LightAudio);
        }
    }

    //Reference: Electronic Brain. (2018). [Unity 3D] - How to Make Flickering Lights (Easy and Simple). [Online video]. Available at: https://youtu.be/iCCFPOdUaNI?si=Mktn92_fEMOmlH1j (Accessed: 17 August 2024).
}
