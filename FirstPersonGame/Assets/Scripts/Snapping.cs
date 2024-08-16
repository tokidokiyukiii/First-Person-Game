using UnityEngine;

public class Snapping : MonoBehaviour
{
    public Transform snapTarget; // The target position and rotation to snap to
    public GameObject target;// The percentage of overlap needed to trigger the snap
    public bool isSnapped = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the box and if it's not already snapped
        /*if (other.CompareTag("Movable") && !isSnapped)
        {
            SnapToSurface(other.transform);
        }*/

        if (other.gameObject == target)
        {
            SnapToSurface(other.transform);
        }
    }

    private void SnapToSurface(Transform boxTransform)
    {
        Vector3 snapPosition = snapTarget.position;
        float boxHeight = boxTransform.GetComponent<Collider>().bounds.size.y;
        
        snapPosition.y += snapTarget.GetComponent<Collider>().bounds.extents.y + boxHeight / 2;
        
        boxTransform.localPosition = snapPosition;
        boxTransform.localRotation = snapTarget.rotation;
        
        Rigidbody rb = boxTransform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Disable physics for the box
        }

        isSnapped = true; // Mark as snapped to prevent further snapping
    }
}
