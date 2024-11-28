using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhandedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class PhysicalObjectFactory : MonoBehaviour
{
    private GameObject objCreated;
    public static PhysicalObjectFactory Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateObject(SO_PhysicalObjectData data, Vector3 position, Quaternion rotation, Transform parent)
    {
        if(data.objectPrefab == null)
        {
            Debug.LogError($"{data.objectName} mising prefab");
            return;
        }

        GameObject obj = Instantiate(data.objectPrefab, position, rotation, parent);

        if (obj.GetComponent<PhysicalObject>())
        {
            obj.GetComponent<PhysicalObject>().Initialize(position, rotation, data);
        }
        else
        {
            Debug.LogError($"Prefab {data.objectName} doesn't has the PhysicalObject.");
        }

        if (!obj.GetComponent<Rigidbody>())
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.mass = data.objectMass;
        }        

        objCreated = obj;

        Debug.Log($"Object created: {data.objectName}");
    }

    public void CreateObjectWithMaterial(SO_PhysicalObjectData data, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (data.objectPrefab == null)
        {
            Debug.LogError($"{data.objectName} mising prefab");
            return;
        }

        CreateObject(data, position, rotation, parent);

        MeshRenderer meshRenderer = objCreated.GetComponent<MeshRenderer>();
    }
}

