using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thoughts : MonoBehaviour
{
    public string writtenThought;
    public string myThought;
    public GameObject GlowingObject;
    public bool enemyMove = false;
    public bool openDoor;
    public Door door;
    //public string roomName;
    public GameObject[] roomTriggers;

    public void MinusThought()
    {
        foreach (var room in roomTriggers)
        {
            EnterRooms roomThoughts = room.gameObject.GetComponent<EnterRooms>();
            --roomThoughts.numThoughts;
            roomThoughts.ShowThoughts();
        }
    }
}
