using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator anim; 
    bool toggle = false; 
    
    // Start is called before the first frame update
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
