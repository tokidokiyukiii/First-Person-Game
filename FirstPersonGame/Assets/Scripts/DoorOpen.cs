using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Door doorToOpen;
    private float originalRotation;
    public float triggerRotationAmount;
    private bool hasOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasOpened)
        {
            originalRotation = doorToOpen.targetRotation;
            doorToOpen.targetRotation = triggerRotationAmount;
        
            doorToOpen.ToggleDoor();
            doorToOpen.targetRotation = originalRotation;

            hasOpened = true;
        }
        
    }
}
