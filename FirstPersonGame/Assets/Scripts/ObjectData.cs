using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectInfo
{
    public string objectName;
    public string objectDescription;
    public GameObject objectModel; // Reference to the 3D model
}

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/ObjectData", order = 1)]
public class ObjectData : ScriptableObject
{
    public List<ObjectInfo> objects = new List<ObjectInfo>();
}
