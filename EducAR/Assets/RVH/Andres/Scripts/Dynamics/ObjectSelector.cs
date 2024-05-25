using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(LeanDragTranslate), typeof(LeanTwistRotateAxis))]
public class ObjectSelector : MonoBehaviour
{
    private static ObjectSelector instance;
    [SerializeField] private GameObject select;
    [Space]
    private LeanDragTranslate leanDragTranslate;
    private LeanTwistRotateAxis leanRotateAxis;

    void Awake()
    {
        if (instance == null)
            instance = this;

        leanDragTranslate = GetComponent<LeanDragTranslate>();
        leanRotateAxis = GetComponent<LeanTwistRotateAxis>();

        OnSelectObject(false);
    }

    public void OnSelectObject(bool active)
    {
        //leanDragTranslate.enabled = active;
        //leanRotateAxis.enabled = active;
        select.SetActive(active);
    }
}
