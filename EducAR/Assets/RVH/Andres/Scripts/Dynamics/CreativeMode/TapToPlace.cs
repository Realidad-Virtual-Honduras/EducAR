using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhandedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class TapToPlace : MonoBehaviour
{
    public static TapToPlace instance;

    [Header("Object Complete Events")]
    [SerializeField] private UnityEvent eventsActions;
    public GameObject visualPrefab;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;

    private bool isPlaneDetected = false;
    [SerializeField, Range(0, 1)] private float minimumDistToPlace;
    [SerializeField] private TextMeshProUGUI textInstruction;

    private List<ARRaycastHit> hitList = new List<ARRaycastHit>();
    private List<RaycastHit> raycastHitList = new List<RaycastHit>();
    [SerializeField] private LayerMask layerMask;

    void Awake()
    {
        instance = this;
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

        if (SessionManager.instance.CanCreateObjects())
        {
            if (raycastManager.Raycast(finger.currentTouch.screenPosition, hitList, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hitList)
                {
                    Pose pose = hit.pose;
                    //Do something when touch the screen without Up the finger

                    SessionManager.instance.AddAmount();
                }
            }
        }
    }

    private void FingerUp(EnhandedTouch.Finger finger)
    {
        if (SessionManager.instance.CanCreateObjects() && visualPrefab != null) 
        {
            if (raycastManager.Raycast(finger.currentTouch.screenPosition, hitList, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hitList)
                {
                    CreationObjectAction();                    
                }
            }

            RaycastHit hits;
            Ray ray = Camera.main.ScreenPointToRay(finger.currentTouch.screenPosition);
            if (Physics.Raycast(ray, out hits))
            {
                CreationObjectAction();
            }
        }
    }

    private void CreationObjectAction()
    {
        if (SessionManager.instance.AmountCreated() >= SessionManager.instance.MaxAmount())
        {            
            DeleteVisual();
            //Invoke
            eventsActions.Invoke();            
        }
    }

    private void Update()
    {
        if(SessionManager.instance.CanCreateObjects() && visualPrefab != null)
        {
            Vector3 rayEmitPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);

            if (raycastManager.Raycast(rayEmitPos, hitList, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hitList)
                {
                    Pose pose = hit.pose;

                    if (Vector3.Distance(pose.position, Camera.main.transform.position) > minimumDistToPlace)
                    {
                        visualPrefab.transform.position = pose.position;
                        visualPrefab.transform.rotation = pose.rotation;
                    }
                }
            }

            RaycastHit hits;
            Ray ray = Camera.main.ScreenPointToRay(rayEmitPos);

            if (Physics.Raycast(ray, out hits))
            {
                Debug.Log(hits.collider.gameObject.name);

                visualPrefab.transform.position = hits.point;
                visualPrefab.transform.rotation = hits.transform.rotation;


                Debug.DrawRay(rayEmitPos, visualPrefab.transform.position, Color.magenta);
            }
        }
    }

    public void CreateObjectToVisualise(GameObject visual)
    {
        visualPrefab = Instantiate(visual, transform);
    }

    private void DeleteVisual()
    {
        PhysicalObjectFactory.Instance.CreateObject(CategoryManager.Instance.SelectedMaterial().objectData, visualPrefab.transform.position, visualPrefab.transform.rotation, visualPrefab.transform.localScale, transform);
        CancelCreation();
        Debug.Log("Se destruyo");
    }

    public void CancelCreation()
    {
        Destroy(visualPrefab);
        visualPrefab = null;

        SessionManager.instance.SwapCreatedToFactory();
        SessionManager.instance.ResetAmount();
        CategoryManager.Instance.DetectObjectAmount();
    }
}
