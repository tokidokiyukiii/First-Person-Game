using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectInteraction : MonoBehaviour
{
    //public ObjectData objectData;    // Reference to the ScriptableObject database

    public FirstPersonControls firstPersonControls;
    public GameObject objectInfoCanvas;     // The UI panel to display item details
    public GameObject HUDCanvas;
    public TextMeshProUGUI objectNameText;            // UI Text for item name
    public TextMeshProUGUI objectDescriptionText;     // UI Text for item description
    //public Transform objectModelHolder;    // Empty GameObject where the item model will be displayed

    private Transform currentObjectModel = null;
    public ObjectViewer objectViewer;
    
    // Start is called before the first frame update
    public void ShowObjectDetails(ObjectData currentObjectData)
    {
        // Search for the item in the database based on the item's name
        //ObjectInfo? newObject = objectData.objects.Find(i => i.objectName == itemName);

        // Deactivate HUD
        HUDCanvas.SetActive(false);
        
        // Activate the UI panel and set the item details
        objectInfoCanvas.SetActive(true);
        objectNameText.text = currentObjectData.objectName;
        objectDescriptionText.text = currentObjectData.objectDescription;
        
        objectViewer.ShowModel(currentObjectData.objectModel);
    }

    public void HideObjectDetails()
    {
        // Activate HUD
        HUDCanvas.SetActive(true);
        
        // Deactivate the UI panel and destroy the model when not viewing the info of an object
        objectInfoCanvas.SetActive(false);
        objectViewer.HideObject();
    }
    
}
