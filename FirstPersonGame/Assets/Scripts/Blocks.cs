/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public char[] lettersOnFaces = new char[6]; // Letters on each face
    public int currentVisibleFaceIndex; // The index of the currently visible face
    public Transform correctOrientation; // Reference to the correct orientation object

    // Method to check if the currently visible face is correctly oriented
    public bool IsFaceCorrectlyOriented()
    {
        // Compare the rotation of the box with the correct orientation reference
        return Quaternion.Angle(transform.rotation, correctOrientation.rotation) < 5f;
    }
}*/

using UnityEngine;

public class Blocks : MonoBehaviour
{
    public char frontBackLetter;
    public char leftRightLetter;
    public char topBottomLetter;

    public char GetVisibleLetter()
    {
        Vector3 localForward = transform.InverseTransformDirection(Camera.main.transform.forward);
        float angle = Vector3.SignedAngle(Vector3.forward, localForward, Vector3.up);

        // Normalize the angle to be between 0 and 360
        angle = (angle + 360) % 360;

        if (angle >= 315 || angle < 45)  // Facing front
            return frontBackLetter;
        else if (angle >= 45 && angle < 135)  // Facing right
            return leftRightLetter;
        else if (angle >= 135 && angle < 225)  // Facing back
            return frontBackLetter;
        else  // Facing left
            return leftRightLetter;
    }

    public bool IsLetterUpsideDown()
    {
        Vector3 localUp = transform.InverseTransformDirection(Vector3.up);
        return Vector3.Dot(localUp, Vector3.up) < 0;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
