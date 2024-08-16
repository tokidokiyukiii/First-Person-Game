using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class GameManager : MonoBehaviour
{
    // Array or list of all snapping surfaces in your level
    public Snapping[] snappingSurfaces;
    public Door canOpen;
    private bool hasOpened = false;

    // Method to check if all blocks are snapped
    public bool AreAllBlocksSnapped()
    {
        foreach (Snapping surface in snappingSurfaces)
        {
            if (!surface.isSnapped)
            {
                return false; // If any surface is not snapped, return false
            }
        }
        return true; // All surfaces are snapped
    }

    // Example usage to trigger an event, like opening a door
    void Update()
    {
        if (!hasOpened && AreAllBlocksSnapped())
        {
            canOpen.openClose(); // Call a method to open the door or trigger another event
            hasOpened = true;
        }
    }
}
