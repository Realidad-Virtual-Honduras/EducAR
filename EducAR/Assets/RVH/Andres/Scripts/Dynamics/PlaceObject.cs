using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhandedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class PlaceObject : MonoBehaviour
{
    [Header("Use Prefab")]
    [SerializeField] private bool usePrefab;
    [SerializeField] private GameObject placementObject;

    [Header("Object Placement Events")]
    [SerializeField] private UnityEvent eventsActions;

    private GameObject obj;
    private bool isPlaced;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;

    private List<ARRaycastHit> hitList = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();

        if (usePrefab)
        {
            obj = null;
            obj = Instantiate(placementObject);
        }
        else
            obj = placementObject;

    }

    private void Start()
    {
        obj.SetActive(false);        
    }

    private void OnEnable()
    {
        EnhandedTouch.TouchSimulation.Enable();
        EnhandedTouch.EnhancedTouchSupport.Enable();
        EnhandedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhandedTouch.TouchSimulation.Disable();
        EnhandedTouch.EnhancedTouchSupport.Disable();
        EnhandedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhandedTouch.Finger finger)
    {
        if (finger.index != 0)
            return;

        if (!isPlaced)
        {
            if (raycastManager.Raycast(finger.currentTouch.screenPosition, hitList, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hitList)
                {
                    isPlaced = true;
                    obj.SetActive(true);
                    Pose pose = hit.pose;
                    obj.transform.position = pose.position;
                    obj.transform.rotation = pose.rotation;

                    //Invoke
                    eventsActions.Invoke();
                }
            }
        }
    }
}
