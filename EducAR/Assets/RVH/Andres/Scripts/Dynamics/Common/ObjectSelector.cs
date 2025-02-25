using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class ObjectSelector : MonoBehaviour
{
    public static ObjectSelector instance;
    [SerializeField] private GameObject select;
    [Space]
    [Header("Information")]
    public string objectInfo;
    [Space]
    public UnityEvent eventOnSelect;

    private bool isSelected = true;

    void Awake()
    {
        if (instance == null)
            instance = this;

        OnSelectObject();
    }

    public void OnSelectObject()
    {
        isSelected = !isSelected;
        //Debug.Log(gameObject.name + " is: " + isSelected);

        select.SetActive(isSelected);
    }
}
