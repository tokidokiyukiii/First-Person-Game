using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riddle : MonoBehaviour
{
    public GameObject riddleUI;
    bool toggle; // toggle riddle true or false 

    public void openCloseRiddle()
    { 
    toggle = !toggle; //not equal to
        if (toggle == false)
        { 
        riddleUI.SetActive(false);
        }
        if(toggle == true) 
        { 
        riddleUI.SetActive(true);
        }
    }


}
