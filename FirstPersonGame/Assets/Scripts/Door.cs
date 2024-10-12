using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    /*public Animator anim; 
    bool toggle = false; 
    
    // Start is called before the first frame update
    public void openClose()
    {
        toggle = !toggle;
        if (toggle == false)
        {
            anim.ResetTrigger("Open");
            anim.SetTrigger("Close");
        }
        if (toggle == true) 
        {
            anim.ResetTrigger("Close");
            anim.SetTrigger("Open");
        }
    }*/
    
    public SoundManager soundManager;
    public bool playsAudio = true;

    public bool isOpen = false;  // Track whether the door is open
    public bool isOpening = false;

    public float rotationSpeed = 50f;  // Speed at which the door rotates
    public float slidingSpeed = 2f; // Speed at which the door slides
    public float targetRotation = 90f; // The desired rotation angle
    public float slidingDistance = 100f; // The desired sliding distance

    private Quaternion originalRotation; //Stores original rotation of door
    private Vector3 originalPosition; //Stores original position of sliding door
    public Vector3 slidingDirection = new Vector3(0, 0, 1);

    public bool isSliding = false; //Is the door a sliding door
    public bool isDrawer = false;
    public bool isCabinet = false;

    public bool needsKey = false; //Does the door need a key
    public bool hasKey = false;
    public bool hasUnlocked = false;
    public bool justUnlocked = false;

    private Coroutine doorCoroutine = null;  // Store reference to the coroutine

    void Start()
    {
        // Store the original rotation and position of the door
        originalRotation = transform.rotation;
        originalPosition = transform.position;
    }

    // Method to toggle door state
    public void ToggleDoor(Transform playerTransform)
    {
        // If the door is currently animating, stop the coroutine
        if (doorCoroutine != null)
        {
            StopCoroutine(doorCoroutine);
        }
        
        // Start the coroutine to open or close the door
        //doorCoroutine = StartCoroutine(RotateDoor(isOpen ? -targetRotation : targetRotation));
        //isOpen = !isOpen;  // Toggle the door state

        if (needsKey && !hasKey && playsAudio)
        {
            /*if (isOpen)
                doorCloseText.gameObject.SetActive(false);
            else if (!isOpen)
                doorOpenText.gameObject.SetActive(false);
            
            doorLockedText.gameObject.SetActive(true);*/
            if (playsAudio)
                soundManager.PlaySFX("Door Locked");
            
            //doorLockedText.gameObject.SetActive(false);
            return;
        }
            
        if (needsKey && hasKey && !hasUnlocked)
        {
            hasUnlocked = true;
            if (playsAudio)
                soundManager.PlaySFX("Open Lock");
            justUnlocked = true;
        }
        
        if ((isSliding || isDrawer) && (!needsKey || hasUnlocked))
        {
            doorCoroutine = StartCoroutine(SlideDoor(isOpen ? -slidingDistance : slidingDistance));
        }
        else if (!needsKey || hasUnlocked)
        {
            doorCoroutine = StartCoroutine(RotateDoor(isOpen ? -targetRotation : targetRotation));
        }
    }

    // Coroutine to rotate the door over time
    IEnumerator RotateDoor(float rotationAmount)
    {
        float rotatedAmount = 0f;  // Track how much the door has rotated
        float rotationDirection = Mathf.Sign(rotationAmount);  // Determine rotation direction (1 for open, -1 for close)
        
        if (isOpen && !isCabinet && playsAudio)
            soundManager.PlaySFX("Close Door");
        else if (!isOpen && playsAudio)
        {
            if (needsKey && !justUnlocked)
            {
                justUnlocked = false;
            }
            else if (!isCabinet)
                soundManager.PlaySFX("Open Door");
        }
        
        while (Mathf.Abs(rotatedAmount) < Mathf.Abs(rotationAmount))
        {
            /*if (isOpen)
                doorCloseText.gameObject.SetActive(false);
            else if (!isOpen)
                doorOpenText.gameObject.SetActive(false);*/
            
            // Calculate the rotation step for this frame
            float rotationStep = rotationSpeed * Time.deltaTime * rotationDirection;
            
            // Ensure that we don't overshoot the target rotation
            if (Mathf.Abs(rotatedAmount + rotationStep) > Mathf.Abs(rotationAmount))
            {
                rotationStep = rotationAmount - rotatedAmount;
            }

            // Apply the rotation
            transform.Rotate(Vector3.forward, rotationStep);
            rotatedAmount += rotationStep;

            // Wait until the next frame
            yield return null;
        }
        
        // After the door has finished rotating, update the isOpen state
        isOpen = !isOpen;
        
        /*if (isOpen)
            doorCloseText.gameObject.SetActive(true);
        else if (!isOpen)
            doorOpenText.gameObject.SetActive(true);*/
    }

    IEnumerator SlideDoor(float slidingAmount)
    {
        float movedAmount = 0f;  // Track how much the door has moved
        float slidingDirectionSign = Mathf.Sign(slidingAmount);  // Determine sliding direction (1 for open, -1 for close)

        if (isOpen && playsAudio)
            soundManager.PlaySFX("Sliding Door Close");
        else if (!isOpen && playsAudio)
        {
            if (needsKey && !justUnlocked)
            {
                justUnlocked = false;
            }
            else
                soundManager.PlaySFX("Sliding Door Open");
        }
        
        while (Mathf.Abs(movedAmount) < Mathf.Abs(slidingAmount))
        {
            /*if (isOpen)
                doorCloseText.gameObject.SetActive(false);
            else if (!isOpen)
                doorOpenText.gameObject.SetActive(false);*/
            
            // Calculate the sliding step for this frame
            float slidingStep = slidingSpeed * Time.deltaTime * slidingDirectionSign;

            // Ensure we don't overshoot the target sliding distance
            if (Mathf.Abs(movedAmount + slidingStep) > Mathf.Abs(slidingAmount))
            {
                slidingStep = slidingAmount - movedAmount;
            }

            Vector3 movement = slidingDirection.normalized * slidingStep;
            transform.Translate(movement);
            movedAmount += slidingStep;

            // Wait until the next frame
            yield return null;
        }

        // After the door has finished sliding, update the isOpen state
        isOpen = !isOpen;
        
        /*if (isOpen)
            doorCloseText.gameObject.SetActive(true);
        else if (!isOpen)
            doorOpenText.gameObject.SetActive(true);*/
    }

    IEnumerator OpenDrawer(float openAmount)
    {
        float movedAmount = 0f;  // Track how much the door has moved
        float slidingDirectionSign = Mathf.Sign(openAmount);  // Determine sliding direction (1 for open, -1 for close)

        while (Mathf.Abs(movedAmount) < Mathf.Abs(openAmount))
        {
            // Calculate the sliding step for this frame
            float slidingStep = slidingSpeed * Time.deltaTime * slidingDirectionSign;

            // Ensure we don't overshoot the target sliding distance
            if (Mathf.Abs(movedAmount + slidingStep) > Mathf.Abs(openAmount))
            {
                slidingStep = openAmount - movedAmount;
            }

            Vector3 movement = slidingDirection.normalized * slidingStep;
            transform.Translate(movement);
            movedAmount += slidingStep;

            // Wait until the next frame
            yield return null;
        }

        // After the door has finished sliding, update the isOpen state
        isOpen = !isOpen;
    }
}
