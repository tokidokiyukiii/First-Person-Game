using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectViewer : MonoBehaviour, IDragHandler
{
    private Transform currentObjectModel;
    public Transform objectModelHolder;
    
    public void ShowModel(Transform objectModel)
    {
        // Remove any existing model from the model holder
        if (currentObjectModel != null)
        {
            Destroy(currentObjectModel);
            Debug.Log("Destroying existing model: " + currentObjectModel.name);
        }

        // Instantiate the item model in the model holder for display
        if (objectModel != null)
        {
            currentObjectModel = Instantiate(objectModel, new Vector3(1000, 1000, 1000), Quaternion.identity);
            //currentObjectModel = Instantiate(objectModel, objectModelHolder);
            currentObjectModel.gameObject.layer = LayerMask.NameToLayer("ObjectView");
            Debug.Log("Model instantiated: " + currentObjectModel.name); // Debug log
        }
        else
        {
            Debug.Log("No Model");
        }
    }
    
    public void HideObject()
    {
        if (currentObjectModel != null)
        {
            Debug.Log("Hiding model: " + currentObjectModel.name);
            Destroy(currentObjectModel.gameObject);
            currentObjectModel = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        float rotationSpeed = 0.2f; 
        //currentObjectModel.eulerAngles += new Vector3(-eventData.delta.y, -eventData.delta.x) * rotationSpeed;
        
        Quaternion rotationY = Quaternion.AngleAxis(-eventData.delta.x * rotationSpeed, Vector3.up);
        Quaternion rotationX = Quaternion.AngleAxis(eventData.delta.y * rotationSpeed, Vector3.right);
        
        // Apply the rotations to the current model
        currentObjectModel.rotation = rotationY * currentObjectModel.rotation * rotationX;
        
        //float rotationSpeed = 0.5f; 
        //currentObjectModel.Rotate(new Vector3(-eventData.delta.y, -eventData.delta.x, 0) * rotationSpeed);
    }
}
