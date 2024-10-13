using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThoughtCount : MonoBehaviour
{
    private int thoughtCount = 0;
    public int thoughtTotal = 5;

    public TextMeshProUGUI thoughtCountText;
    
    public TextMeshProUGUI keyText;

    public GameObject key;
    public GameObject glowingKey;

    public void AddThought()
    {
        thoughtCount++;

        if (thoughtCount >= thoughtTotal)
        {
            key.SetActive(true);
            glowingKey.SetActive(true);
            ShowMessage("Go find the key!");
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
            thoughtCountText.text = "Thoughts: " + thoughtCount + "/ 5";
        }
    }

    /*private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messagePanel.SetActive(false); // Hide the message panel
    }*/

}
