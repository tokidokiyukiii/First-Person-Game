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

    public GameObject Lights;
    public bool lightsOff = true;

    public bool isBedroom;
    public EnemyAI enemyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            roomNameText.text = roomName;
            
            //LightsOn();
            if (lightsOff)
                Lights.SetActive(true);

            if (hasThoughts)
            {
                ShowThoughts();
            }
            else
            {
                numThoughtsRoomText.gameObject.SetActive(false);
            }

            if (isBedroom)
                enemyAI.isInBedroom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (lightsOff)
            Lights.SetActive(false);
        
        if (isBedroom)
            enemyAI.isInBedroom = false;
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

    public void LightsOn()
    {
        //foreach (var light in Lights)
        {
            //light.SetActive(true);
        }
    }

    public void LightsOff()
    {
        //foreach (var light in Lights)
        {
            //light.SetActive(false);
        }
    }
}
