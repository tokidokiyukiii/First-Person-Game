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

    public bool isOpening;
    
    public AudioSource sfxSource;
    public AudioClip sfxSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasMoved)
            {
                if (unlockDoor)
                {
                    doorToUnlock.needsKey = false;
                }

                originalRotation = doorToOpen.targetRotation;
                doorToOpen.targetRotation = triggerRotationAmount;

                doorToOpen.customRotation = triggerRotationAmount;

                doorToOpen.playsSounds = false;
                sfxSource.PlayOneShot(sfxSound);
                
                doorToOpen.ToggleDoor();
                doorToOpen.targetRotation = originalRotation;

                if (isOpening)
                {
                    doorToOpen.shouldMinus = true;
                }

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
}
