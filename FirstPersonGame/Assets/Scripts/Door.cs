using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator anim; 
    bool toggle = false; 
    
    // Omogonix. (2023). How to Make Doors in Unity.[Online video]. Available at: https://youtu.be/wzNykqSPa0M?si=Ypm6S-bhQYVisCDZ (Accessed: 16 August 2024).
    public void openClose()
    {
        toggle = !toggle;
        if (toggle == false)
        {
            anim.ResetTrigger("open");
            anim.SetTrigger("close");
        }
        if (toggle == true) 
        {
            anim.ResetTrigger("close");
            anim.SetTrigger("open");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
