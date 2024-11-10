using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ThoughtCount : MonoBehaviour
{
    public int thoughtCount = 0;
    public int secondPhaseThoughtCount;
    public int finalPhaseThoughtCount;
    public int thoughtTotal = 5;

    public TextMeshProUGUI phaseText;
    private int phaseCount = 1;
    public TextMeshProUGUI thoughtCountText;
    
    public TextMeshProUGUI keyText;

    public GameObject key;
    public GameObject glowingKey;

    public GameObject Thoughts2;
    public GameObject Thoughts3;
    public GameObject Thoughts4;
    public GameObject Thoughts5;

    public GameObject GlowingThoughts2;
    public GameObject GlowingThoughts3;
    public GameObject GlowingThoughts4;
    public GameObject GlowingThoughts5;

    public EnemyAI enemyAI;
    public Door towerDoor;

    public GameObject normalVolume;
    public GameObject finalVolume;

    public void AddThought()
    {
        thoughtCount++;

        if (thoughtCount >= thoughtTotal)
        {
            key.SetActive(true);
            glowingKey.SetActive(true);
            ShowMessage("The key is in the attic. Be quick...");
        }

        switch (thoughtCount)
        {
            case 3:
                Thoughts2.SetActive(true);
                GlowingThoughts2.SetActive(true);
                normalVolume.SetActive(false);
                finalVolume.SetActive(true);
                break;
            case 5:
                phaseText.text = "PHASE 2";
                phaseCount = 2;

                towerDoor.needsKey = false;
                towerDoor.ToggleDoor();
                
                enemyAI.gameObject.SetActive(true);
                enemyAI.canEnemyMove = true;
                break;
            case 6:
                Thoughts3.SetActive(true);
                GlowingThoughts3.SetActive(true);
                break;
            case 9:
                Thoughts4.SetActive(true);
                GlowingThoughts4.SetActive(true);
                break;
            case 10:
                phaseText.text = "FINAL PHASE";
                phaseCount = 3;
                normalVolume.SetActive(false);
                finalVolume.SetActive(true);
                break;
            case 12:
                Thoughts5.SetActive(true);
                GlowingThoughts5.SetActive(true);
                break;
        }

        UpdateUI(); 
    }

    public void ShowMessage(string message)
    {
        thoughtCountText.gameObject.SetActive(false);
        keyText.gameObject.SetActive(true);
        //countPanel.SetActive(false);
        keyText.text = message; 

        // StartCoroutine(HideMessageAfterDelay(2f));
    }

    private void UpdateUI()
    {
        if (thoughtCountText != null)
        {
            if (phaseCount == 1)
            {
                thoughtCountText.text = "Thoughts: " + thoughtCount + " / 5";
            }
            else if (phaseCount == 2)
            {
                thoughtCountText.text = "Thoughts: " + secondPhaseThoughtCount + " / 5";
            }
            else if (phaseCount == 2)
            {
                thoughtCountText.text = "Thoughts: " + finalPhaseThoughtCount + " / 5";
            }
        }
    }

    /*private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messagePanel.SetActive(false); // Hide the message panel
    }*/

}
