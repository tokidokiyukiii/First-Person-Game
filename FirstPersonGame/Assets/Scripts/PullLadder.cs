using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullLadder : MonoBehaviour
{
    public float slidingSpeed;
    public float slidingDistance;
    private Vector3 slidingDirection = Vector3.down;

    public bool hasMoved = false;
    public bool hasFinishedMoving = false;

    private Coroutine ladderCoroutine;

    public void ToggleLadder()
    {
        if (ladderCoroutine != null)
        {
            StopCoroutine(ladderCoroutine);
        }

        if (!hasMoved)
        {
            ladderCoroutine = StartCoroutine(SlideLadder(slidingDistance));
        }
        
        hasMoved = !hasMoved;
    }
    
    IEnumerator SlideLadder(float slidingAmount)
    {
        float movedAmount = 0f;  // Track how much the ladder has moved

        // PLAYSOUND 

        while (Mathf.Abs(movedAmount) < Mathf.Abs(slidingAmount))
        {
            // Calculate the sliding step for this frame
            float slidingStep = slidingSpeed * Time.deltaTime;

            // Ensure we don't overshoot the target sliding distance
            if (Mathf.Abs(movedAmount + slidingStep) > Mathf.Abs(slidingAmount))
            {
                slidingStep = Mathf.Abs(slidingAmount) - Mathf.Abs(movedAmount);
            }

            // Determine movement direction and apply the movement
            Vector3 movement = slidingDirection.normalized * (slidingStep * Mathf.Sign(slidingAmount));
            transform.Translate(movement, Space.World);
            movedAmount += slidingStep;

            // Wait until the next frame
            yield return null;
        }

        hasFinishedMoving = true;
    }
}
