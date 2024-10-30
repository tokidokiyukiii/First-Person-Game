using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnterRooms : MonoBehaviour
{
    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI numThoughtsRoomText;
    
    public string roomName;
    public int numThoughts;

    public bool hasThoughts = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            roomNameText.text = roomName;

            if (hasThoughts)
            {
                ShowThoughts();
            }
            else
            {
                numThoughtsRoomText.gameObject.SetActive(false);
            }
        }
    }

    public void ShowThoughts()
    {
        numThoughtsRoomText.gameObject.SetActive(true);
        if (numThoughts > 1)
            numThoughtsRoomText.text = numThoughts + " Thoughts";
        else if (numThoughts == 1)
            numThoughtsRoomText.text = numThoughts + " Thought";
        else if (numThoughts == 0)
            numThoughtsRoomText.gameObject.SetActive(false);
    }
}
