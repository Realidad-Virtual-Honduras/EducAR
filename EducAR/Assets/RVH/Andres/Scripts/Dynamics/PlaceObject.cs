using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhandedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class PlaceObject : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject obj;
    [SerializeField] private bool isPlaneDetected;
    [SerializeField] private bool isPlaced;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private List<ARRaycastHit> hitList = new List<ARRaycastHit>();
    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        obj = Instantiate(prefab);
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
                    obj.SetActive(true);
                    Pose pose = hit.pose;
                    obj.transform.position = pose.position;
                    obj.transform.rotation = pose.rotation;

                    //Invoke
                    SolarSystem.instance.Shuffle();
                }
                isPlaced = true;
            }
        }
    }
}
