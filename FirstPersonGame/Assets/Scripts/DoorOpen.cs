using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Door doorToOpen;

    private void OnTriggerEnter(Collider other)
    {
        //doorToOpen.CloseDoor();
    }
}
