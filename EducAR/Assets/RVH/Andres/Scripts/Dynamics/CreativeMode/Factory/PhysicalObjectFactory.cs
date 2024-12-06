using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalObjectFactory : MonoBehaviour
{
    public static PhysicalObjectFactory Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateObject(SO_PhysicalObjectData data, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
    {
        if(data.objectPrefab == null)
        {
            Debug.LogError($"{data.objectName} mising prefab");
            return;
        }

        GameObject obj = Instantiate(data.objectPrefab, position, rotation, parent);
        obj.transform.localScale = scale;

        /*if (obj.GetComponent<PhysicalObject>())
        {
            obj.GetComponent<PhysicalObject>().Initialize(position, rotation, data);
        }
        else
        {
            Debug.LogError($"Prefab {data.objectName} doesn't has the PhysicalObject.");
        }*/

        if (!obj.GetComponent<Rigidbody>())
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();
        }        

        Debug.Log($"Object created: {data.objectName}");
    }
}

