using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems; 

[DefaultExecutionOrder(-1)]
public class TouchMananger : Singleton<TouchMananger>
{
    #region Touch Events
    public delegate void StartTouchEvent(Vector2 pos, float time);
    public event StartTouchEvent OnStartTouch;

    public delegate void EndTouchEvent(Vector2 pos, float time);
    public event EndTouchEvent OnEndTouch;
    private TouchControls m_TouchControls;
    #endregion
    public static TouchMananger instance;

    [SerializeField] private float distance;
    [Space]
    [SerializeField] private ObjectSelector objectSelector;
    [SerializeField] private ObjectSelector lastObjectSelector;
    [SerializeField] private bool isSelected;
    [SerializeField] private Transform objectPos;

    private XRRayInteractor _interactor;
    private Camera _camera;
    Ray ray;
    RaycastHit hit;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        _camera = Camera.main;
        m_TouchControls = new TouchControls();
        _interactor = GetComponentInChildren<XRRayInteractor>();
    }

    private void Start()
    {
        LevelManager.instance.selectedObject.text = "";
        m_TouchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        m_TouchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void Update()
    {
        ray = _camera.ScreenPointToRay(Input.mousePosition);
    }

    private void RaycastSelector()
    {
        if (Physics.Raycast(ray, out hit, distance))
        {
            objectSelector = hit.transform.GetComponent<ObjectSelector>();
            float distancef = Vector3.Distance(_camera.transform.position, objectSelector.gameObject.transform.position);
            Debug.DrawRay(ray.direction, _interactor.rayOriginTransform.position * (distancef * 10f), Color.white);
            Debug.Log("La distancia del objeto es: " + distancef * 10f);
        }
        else
        {
            Debug.DrawRay(ray.direction, _interactor.rayOriginTransform.position * distance, Color.red);

            objectSelector = null;
        }
    }

    private void Hits()
    {
        if (hit.transform.name == objectSelector.name)
        {
            SelectObject();
        }
    }

    #region Selections
    private void SelectObject()
    {
        if (lastObjectSelector != null && lastObjectSelector == objectSelector)
        {
            Debug.Log("Volvi a topar con el mismo objeto");
            UnselectObject();
            return;
        }
        if(lastObjectSelector == null)
        {
            isSelected = true;
            lastObjectSelector = objectSelector;
            lastObjectSelector.gameObject.transform.SetParent(objectPos);
            lastObjectSelector.OnSelectObject(isSelected);
        }

        LevelManager.instance.selectedObject.text = lastObjectSelector.name;

        //lastObjectSelector.gameObject.transform.position = objectPos.position;

        Debug.Log("Se selecciono el objeto " + objectSelector.name);        

        //objectSelector.OnSelectObject(true);

        if (objectSelector != lastObjectSelector)
        {
            objectSelector.gameObject.transform.SetParent(null);
        }

    }
    private void UnselectObject()
    {
        isSelected = false;
        LevelManager.instance.selectedObject.text = "";
        lastObjectSelector.OnSelectObject(isSelected);
        lastObjectSelector.gameObject.transform.SetParent(null);
        lastObjectSelector = null;

        Debug.Log("Se deselecciono el objeto " + lastObjectSelector.name);

        //objectSelector.OnSelectObject(false);
        //lastObjectSelector.OnSelectObject(false);

        if (objectSelector != lastObjectSelector)
        {
            SelectObject();
            lastObjectSelector.gameObject.transform.SetParent(null);
        }

        //lastObjectSelector = null;
    }

    public void UnSelectAll()
    {
        objectSelector = null;
        UnselectObject();
    }
    #endregion

    #region Enable / Disable
    private void OnEnable()
    {
        m_TouchControls.Enable();
    }

    private void OnDisable()
    {
        m_TouchControls.Disable();
    }
    #endregion

    #region Touch Event Voids
    private void StartTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("Touch Started " + m_TouchControls.Touch.TouchPosition.ReadValue<Vector2>());
        
        if(LevelManager.instance.canInteract)
            RaycastSelector();
        //HitObjects();       

        if (OnStartTouch != null) 
            OnStartTouch(m_TouchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("Touch ended " + m_TouchControls.Touch.TouchPosition.ReadValue<Vector2>());

        if (LevelManager.instance.canInteract)
            Hits();

        if (OnEndTouch != null)
            OnEndTouch(m_TouchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
    }
    #endregion
}
