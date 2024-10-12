using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThoughtCount : MonoBehaviour
{
    private int thoughtCount = 0;

    public GameObject countPanel;
    public TMP_Text thoughtCountText;

    public GameObject messagePanel; 
    public TMP_Text messageText;

    public GameObject key;

    public void AddThought()
    {
        thoughtCount++;

        if (thoughtCount >= 5)
        {
            key.SetActive(true);
            ShowMessage("Go find the key on the table!");
        }

        UpdateUI(); 
    }

    private void ShowMessage(string message)
    {
        messagePanel.SetActive(true);
        countPanel.SetActive(false);
        messageText.text = message; 

        // StartCoroutine(HideMessageAfterDelay(2f));
    }

    private void UpdateUI()
    {
        if (thoughtCountText != null)
        {
            thoughtCountText.text = "Thoughts: " + thoughtCount + "/ 5";
        }
    }

    /*private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messagePanel.SetActive(false); // Hide the message panel
    }*/
}
