using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MEC;

public class BodyPartInfo : MonoBehaviour
{
    public static BodyPartInfo instance;
    public Vector3 startPos;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }
}
