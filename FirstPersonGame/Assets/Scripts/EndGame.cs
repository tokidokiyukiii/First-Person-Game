using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject endCanvas;
    public GameObject HUDCanvas;
    
    public GameObject getOutSound;
    public GameObject endSound;
    public GameObject endMusic;
    public GameObject sfxSound;

    public FirstPersonControls firstPersonControls;
    private void OnTriggerEnter(Collider other)
    {
        // Disable player movement
        firstPersonControls.isInputEnabled = false;
    
        // Disable Other Canvases
        HUDCanvas.SetActive(false);
        
        getOutSound.SetActive(false);
        sfxSound.SetActive(false);
        endSound.SetActive(true);
        endMusic.SetActive(true);
    
        // Enable EndGame Canvas
        endCanvas.SetActive(true);
    }

}
