using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    public GameObject glowingKey;
    public GameObject[] doorToOpen;

    public void UnlockDoor()
    {
        foreach (var door in doorToOpen)
        {
            Door openDoor = door.GetComponent<Door>();
            openDoor.hasKey = true;
        }
        
        Destroy(glowingKey);
        Destroy(gameObject);
    }
}
