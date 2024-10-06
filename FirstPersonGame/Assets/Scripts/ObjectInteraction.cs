using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public ObjectData objectData;    // Reference to the ScriptableObject database

    public GameObject objectViewPanel;     // The UI panel to display item details
    public TextMeshProUGUI objectNameText;            // UI Text for item name
    public TextMeshProUGUI objectDescriptionText;     // UI Text for item description
    public Transform objectModelHolder;    // Empty GameObject where the item model will be displayed

    private GameObject currentObjectModel;
    
    // Start is called before the first frame update
    public void ShowObjectDetails(string itemName)
    {
        // Search for the item in the database based on the item's name
        ObjectInfo? newObject = objectData.objects.Find(i => i.objectName == itemName);

        if (newObject.HasValue)
        {
            // Activate the UI panel and set the item details
            objectViewPanel.SetActive(true);
            objectNameText.text = newObject.Value.objectName;
            objectDescriptionText.text = newObject.Value.objectDescription;

            // Remove any existing model from the model holder
            if (currentObjectModel != null)
                Destroy(currentObjectModel);

            // Instantiate the item model in the model holder for display
            if (newObject.Value.objectModel != null)
            {
                if (currentObjectModel != null)
                {
                    currentObjectModel = Instantiate(newObject.Value.objectModel, objectModelHolder);
                    Debug.Log($"Instantiated {currentObjectModel.name} at {currentObjectModel.transform.position}");
                }
                else
                {
                    Debug.LogError("itemModel is null!");
                }
                //currentObjectModel = Instantiate(newObject.Value.objectModel, objectModelHolder);
                //currentItemModel.transform.localPosition = Vector3.zero;  // Ensure the model is centered
                //currentItemModel.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void HideObjectDetails()
    {
        // Deactivate the UI panel and destroy the model when not looking at an "Info" object
        objectViewPanel.SetActive(false);

        if (currentObjectModel != null)
        {
            Destroy(currentObjectModel);
            currentObjectModel = null;
        }
    }
}
