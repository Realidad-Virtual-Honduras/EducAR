using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhandedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class TapToPlace : MonoBehaviour
{
    [Header("Object Complete Events")]
    [SerializeField] private UnityEvent eventsActions;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;

    private List<ARRaycastHit> hitList = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhandedTouch.TouchSimulation.Enable();
        EnhandedTouch.EnhancedTouchSupport.Enable();
        EnhandedTouch.Touch.onFingerDown += FingerDown;
        EnhandedTouch.Touch.onFingerUp += FingerUp;
    }

    private void OnDisable()
    {
        EnhandedTouch.TouchSimulation.Disable();
        EnhandedTouch.EnhancedTouchSupport.Disable();
        EnhandedTouch.Touch.onFingerDown -= FingerDown;
        EnhandedTouch.Touch.onFingerUp -= FingerUp;
    }

    private void FingerDown(EnhandedTouch.Finger finger)
    {
        if (finger.index != 0)
            return;

        if (CreateObjects.instance.canCreateObject)
        {
            if (raycastManager.Raycast(finger.currentTouch.screenPosition, hitList, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hitList)
                {
                    Pose pose = hit.pose;
                    //CreateObjects.instance.CreateObject(pose.position, pose.rotation);
                    PhysicalObjectFactory.Instance.CreateObject(CategoryManager.Instance.SelectedMaterial().objectData, pose.position, pose.rotation, transform);
                    CreateObjects.instance.amountCreated++;
                }
            }
        }
    }

    private void FingerUp(EnhandedTouch.Finger finger)
    {
        if (CreateObjects.instance.canCreateObject)
        {
            if (raycastManager.Raycast(finger.currentTouch.screenPosition, hitList, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hitList)
                {
                    if (CreateObjects.instance.amountCreated >= CreateObjects.instance.maxAmount)
                    {
                        UiManagerCreativeMode.instance.ShowNHydeCreation();
                        CreateObjects.instance.amountCreated = 0;

                        //Invoke
                        eventsActions.Invoke();
                    }
                }
            }
        }
    }
}
