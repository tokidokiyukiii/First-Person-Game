using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject endCanvas;
    public GameObject HUDCanvas;
    public GameObject SecondViewCanvas;
    public GameObject NormalViewCanvas;

    public FirstPersonControls firstPersonControls;
    private void OnTriggerEnter(Collider other)
    {
        // Disable player movement
        firstPersonControls.isInputEnabled = false;
    
        // Disable Other Canvases
        HUDCanvas.SetActive(false);
        SecondViewCanvas.SetActive(false);
        NormalViewCanvas.SetActive(false);
    
        // Enable EndGame Canvas
        endCanvas.SetActive(true);
    }

}
