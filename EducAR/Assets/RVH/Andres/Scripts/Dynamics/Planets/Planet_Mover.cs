using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Planet_Mover : MonoBehaviour
{
    public static Planet_Mover Instance;

    [SerializeField] private Planet_Sctiptable planet_Sctiptable;
    public Vector3 planetSize;

    public Planet_Sctiptable Planet_Sctiptable
    {
        get { return planet_Sctiptable; }
    }

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        planetSize = transform.localScale;
    }
}
