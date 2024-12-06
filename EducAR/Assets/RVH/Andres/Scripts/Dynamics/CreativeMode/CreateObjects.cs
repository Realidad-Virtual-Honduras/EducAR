using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateObjects : MonoBehaviour
{
    public static CreateObjects instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
}