using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Door doorToOpen;
    public Door doorToLock;
    public Door doorToUnlock;
    
    private float originalRotation;
    public float triggerRotationAmount;

    public bool lockDoor = false;
    public bool lockCurrentDoor = false;
    public bool unlockDoor = false;
    private bool hasMoved = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasMoved)
        {
            if (unlockDoor)
            {
                doorToUnlock.needsKey = false;
            }
            
            originalRotation = doorToOpen.targetRotation;
            doorToOpen.targetRotation = triggerRotationAmount;
        
            doorToOpen.ToggleDoor();
            doorToOpen.targetRotation = originalRotation;

            hasMoved = true;

            if (lockDoor)
            {
                if (doorToLock.isOpen)
                {
                    doorToLock.ToggleDoor();
                }
                doorToLock.needsKey = true;
            }

            if (lockCurrentDoor)
                doorToOpen.needsKey = true;
        }
        
    }
}
