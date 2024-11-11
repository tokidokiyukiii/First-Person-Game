using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thoughts : MonoBehaviour
{
    public string writtenThought;
    public string myThought;
    public GameObject GlowingObject;
    //public bool enemyMove = false;
    public bool openDoor;
    public Door door;
    //public string roomName;
    public GameObject[] roomTriggers;
    public bool isThought1;
    public bool isThought2;

    public GameObject candleLight;
    public Light lightIntensity;


    private void Start()
    {
        foreach (var room in roomTriggers)
        {
            EnterRooms roomThoughts = room.gameObject.GetComponent<EnterRooms>();
            roomThoughts.hasThoughts = true;
            ++roomThoughts.numThoughts;
        }
    }

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
