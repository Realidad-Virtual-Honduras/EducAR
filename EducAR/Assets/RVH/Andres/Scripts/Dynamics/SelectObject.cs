using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhandedTouch = UnityEngine.InputSystem.EnhancedTouch;


public class SelectObject : MonoBehaviour
{
    [SerializeField] private GameObject selectedObject;
    [Space]
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private List<ARRaycastHit> hitList = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        planeManager = FindObjectOfType<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhandedTouch.TouchSimulation.Enable();
        EnhandedTouch.EnhancedTouchSupport.Enable();
        EnhandedTouch.Touch.onFingerDown += ShotRay;
    }

    private void OnDisable()
    {
        EnhandedTouch.TouchSimulation.Disable();
        EnhandedTouch.EnhancedTouchSupport.Disable();
        EnhandedTouch.Touch.onFingerDown -= ShotRay;
    }

    public void ShotRay(EnhandedTouch.Finger finger)
    {
        if (finger.index != 0)
            return;

        if (raycastManager.Raycast(finger.currentTouch.screenPosition, hitList))
        {
            foreach (ARRaycastHit hit in hitList)
            {
                Pose pose = hit.pose;
                selectedObject = raycastManager.gameObject;
            }
        }
    }
}
