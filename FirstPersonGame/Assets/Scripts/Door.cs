using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    public float rotationSpeed = 50f;  // Speed at which the door rotates
    public bool isOpen = false;  // Track whether the door is open
    public float targetRotation = 90f;  // The desired rotation angle in degrees

    private Coroutine doorCoroutine = null;  // Store reference to the coroutine

    // Method to toggle door state
    public void ToggleDoor()
    {
        // If the door is currently animating, stop the coroutine
        if (doorCoroutine != null)
        {
            StopCoroutine(doorCoroutine);
        }
        
        // Start the coroutine to open or close the door
        doorCoroutine = StartCoroutine(RotateDoor(isOpen ? -targetRotation : targetRotation));
        isOpen = !isOpen;  // Toggle the door state
    }

    // Coroutine to rotate the door over time
    IEnumerator RotateDoor(float rotationAmount)
    {
        float rotatedAmount = 0f;  // Track how much the door has rotated
        float rotationDirection = Mathf.Sign(rotationAmount);  // Determine rotation direction (1 for open, -1 for close)
        
        while (Mathf.Abs(rotatedAmount) < Mathf.Abs(rotationAmount))
        {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
