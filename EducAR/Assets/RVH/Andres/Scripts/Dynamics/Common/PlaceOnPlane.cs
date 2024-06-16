using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class PlaceOnPlane : MonoBehaviour
{
    public static PlaceOnPlane instance;
    [Header("Objects to be place")]
    [SerializeField, Tooltip("")] private GameObject prefabObjectPointer;
    [SerializeField, Tooltip("")] private GameObject placedObject;
    [SerializeField, Range(0, 1)] private float minimumDistToPlace;
    [SerializeField] private bool usePrefab;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent OnPlaceObject;

    public GameObject placedPrefab
    {
        get { return placedObject; }
        set { placedObject = value; }
    }

    public GameObject spawnedObject
    {
        get; private set;
    }

    public bool isObjectPlaced = false;
    public bool isPlaneDetected = false;

    [SerializeField] private TextMeshProUGUI textInstruction;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARRaycastManager m_RaycastManager;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        m_RaycastManager = GetComponent<ARRaycastManager>();
        spawnedObject = placedObject;
    }

    void Start()
    {
        prefabObjectPointer.SetActive(false);

        if(usePrefab)
        {
            spawnedObject = Instantiate(placedPrefab);
        }

        spawnedObject.SetActive(false);
    }

    public void StartGame(Vector2 pos)
    {
        if (isPlaneDetected && !isObjectPlaced)
        {
            if (m_RaycastManager.Raycast(pos, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in s_Hits)
                {
                    Pose pose = hit.pose;
                    Debug.Log("Tap on: " + pose.position);

                    spawnedObject.SetActive(true);

                    //Set transform
                    spawnedObject.transform.position = prefabObjectPointer.transform.position;
                    spawnedObject.transform.rotation = pose.rotation;
                    spawnedObject.transform.localScale = prefabObjectPointer.transform.localScale;

                    spawnedObject.transform.parent = null;

                    isObjectPlaced = true;

                    prefabObjectPointer.SetActive(false);

                    //Invoke
                    OnPlaceObject.Invoke();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isObjectPlaced)
        {
            if (isPlaneDetected)
                textInstruction.text = "Doble Tap para empezar";
            else
                textInstruction.text = "Escanea la superficie";

            Vector3 rayEmitPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);

            if(m_RaycastManager.Raycast(rayEmitPos, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in s_Hits)
                {
                    Pose pose = hit.pose;

                    if(Vector3.Distance(pose.position, Camera.main.transform.position) > minimumDistToPlace)
                    {
                        isPlaneDetected = true;
                        prefabObjectPointer.SetActive(true);
                        prefabObjectPointer.transform.position = pose.position;
                        prefabObjectPointer.transform.rotation = pose.rotation;

                        //Try
                    }
                    else
                    {
                        isPlaneDetected = false;
                        prefabObjectPointer.SetActive(false);
                    }
                }
            }
        }
    }
}
