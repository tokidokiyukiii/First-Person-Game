using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/ObjectData", order = 1)]
public class ObjectData : ScriptableObject
{
    public string objectName;
    public string objectDescription;
    public Transform objectModel; // Reference to the 3D model
    //public List<ObjectInfo> objects = new List<ObjectInfo>();
}
