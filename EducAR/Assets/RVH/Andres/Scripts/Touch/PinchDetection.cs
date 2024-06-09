using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PinchDetection : MonoBehaviour
{
    public static PinchDetection instance;

    public Transform objectTranform;
    public TextMeshProUGUI textMeshProUGUI;

    private TouchControls controls;
    private Coroutine zoomCoroutine;

    private float objectSpeed = 4f;
    private float objectScale = 1f;
    private float prevDistance, distance;
    
    void Awake()
    {
        if(instance == null)
            instance = this;

        controls = new TouchControls();
        objectTranform = Camera.main.transform;
    }

    private void Start()
    {
        controls.Touch.SecundaryTouchContact.started += ctx => ZoomStart();
        controls.Touch.SecundaryTouchContact.canceled += ctx => ZoomEnd();
    }

    private void Update()
    {
        textMeshProUGUI.text = controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>().ToString();
    }

    private void ZoomStart()
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd()
    {
        StopCoroutine(zoomCoroutine);
    }

    IEnumerator ZoomDetection()
    {
        while (true) 
        {
            distance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(), controls.Touch.SecundaryFingerPosition.ReadValue<Vector2>());

            #region Detection
            //Zoom Out / Small Object
            if(distance > prevDistance)
            {
                Vector3 scaleObject = objectTranform.localScale;

                scaleObject.x += objectScale;
                scaleObject.y += objectScale;
                scaleObject.z += objectScale;

                float scale = Time.fixedDeltaTime * objectSpeed;

                objectTranform.localScale = Vector3.Slerp(objectTranform.localScale, scaleObject, scale);
            }
            //Zoom In / Big Object
            else if(distance < prevDistance)
            {
                Vector3 scaleObject = objectTranform.localScale;

                scaleObject.x -= objectScale;
                scaleObject.y -= objectScale;
                scaleObject.z -= objectScale;

                float scale = Time.fixedDeltaTime * objectSpeed;

                objectTranform.localScale = Vector3.Slerp(objectTranform.localScale, scaleObject, scale);
            }
            #endregion

            prevDistance = distance;
            yield return null;
        }
    }

    #region Enable/Disable
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    #endregion
}
