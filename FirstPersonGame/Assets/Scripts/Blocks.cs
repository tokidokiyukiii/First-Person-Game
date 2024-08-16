using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public char[] lettersOnFaces = new char[6]; // Letters on each face
    public Vector3[] correctFaceRotations = new Vector3[6]; // Correct rotations (Euler angles) for each face
    public int currentVisibleFaceIndex; // The index of the currently visible face

    // Method to check if the currently visible face is correctly oriented
    public bool IsFaceCorrectlyOriented()
    {
        // Get the current rotation in Euler angles
        Vector3 currentRotation = transform.eulerAngles;

        // Get the correct rotation for the current face
        Vector3 correctRotation = correctFaceRotations[currentVisibleFaceIndex];

        // Check if the current rotation is close to the correct rotation
        return Vector3.Distance(currentRotation, correctRotation) < 0.1f;
    }
}
