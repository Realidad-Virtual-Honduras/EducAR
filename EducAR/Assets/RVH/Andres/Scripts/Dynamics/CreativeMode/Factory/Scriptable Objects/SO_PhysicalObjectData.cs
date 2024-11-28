using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Creative Physical Object", menuName = "Create/Creative Mode/Physical object data")]
public class SO_PhysicalObjectData : ScriptableObject
{
    public string objectName;
    public GameObject objectPrefab;
    public Vector3 objectSize = Vector3.one;
    public float objectMass = 1f;
    public PhysicMaterial physics;
}
