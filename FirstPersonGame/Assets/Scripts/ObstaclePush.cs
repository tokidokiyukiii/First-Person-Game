using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePush : MonoBehaviour
{
    [SerializeField]
    private float forceMagnitude;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null)
        {
            if (hit.collider.CompareTag("Movable"))
            {
                // Enable interpolation to smooth out the movement
                rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

                // Use the hit move direction for a more accurate force application
                Vector3 forceDirection = hit.moveDirection;
                forceDirection.y = 0; // Keep force direction on the horizontal plane
                forceDirection.Normalize();

                // Adjust the force magnitude based on the mass of the object
                float adjustedForceMagnitude = forceMagnitude / rigidbody.mass;

                // Apply the force using VelocityChange for immediate response
                rigidbody.AddForce(forceDirection * adjustedForceMagnitude, ForceMode.VelocityChange);
            }
        }
    }

}
