using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicalObject : MonoBehaviour
{
    public abstract void Initialize(Vector3 position, Quaternion rotation, SO_PhysicalObjectData data);
}
