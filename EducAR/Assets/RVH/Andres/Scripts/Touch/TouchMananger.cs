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
    [SerializeField] private Transform objectPos;
    [SerializeField] private Transform originalParent;
    [SerializeField] private bool useCenter;

    private bool isSelected;
    private ObjectSelector objectSelector;
    private ObjectSelector lastObjectSelector;

    private Camera _camera;
    private XRRayInteractor _interactor;

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
        LevelManager.instance.SelectedObjectInfoShow(false, "", "");

        //LevelManager.instance.selectedObject.text = "";
        m_TouchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        m_TouchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
        m_TouchControls.Touch.MultiTap.performed += ctx => MultiTappedPerformed(ctx);
    }

    #region Raycast
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
    #endregion

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
            if(useCenter)
            {
                lastObjectSelector.gameObject.transform.position = objectPos.position;
                lastObjectSelector.gameObject.transform.rotation = objectPos.rotation;
            }    
            lastObjectSelector.eventOnSelect.Invoke();
        }

        LevelManager.instance.SelectedObjectInfoShow(true, lastObjectSelector.name, lastObjectSelector.objectInfo);
        //LevelManager.instance.selectedObject.text = lastObjectSelector.name;

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
        LevelManager.instance.SelectedObjectInfoShow(false, "", "");
        //LevelManager.instance.selectedObject.text = "";
        lastObjectSelector.eventOnSelect.Invoke();

        lastObjectSelector.gameObject.transform.SetParent(null);

        lastObjectSelector = null;

        Debug.Log("Se deselecciono el objeto " + lastObjectSelector.name);

        //objectSelector.OnSelectObject(false);
        //lastObjectSelector.OnSelectObject(false);

        /*if (objectSelector != lastObjectSelector)
        {
            SelectObject();
            lastObjectSelector.gameObject.transform.SetParent(null);
        }*/

        //lastObjectSelector = null;
    }

    public void UnSelectAll()
    {
        objectSelector = null;
        UnselectObject();
    }
    public void UnSelectAllNoParent()
    {
        objectSelector = null;
        isSelected = false;
        LevelManager.instance.SelectedObjectInfoShow(false, "", "");
        //LevelManager.instance.selectedObject.text = "";
        lastObjectSelector.eventOnSelect.Invoke();
        lastObjectSelector = null;

        Debug.Log("Se deselecciono el objeto " + lastObjectSelector.name);
    }
    #endregion

    private void PinchScale()
    {

    }

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
        if (OnStartTouch != null) 
            OnStartTouch(m_TouchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);

        if(LevelManager.instance.canInteract)
            RaycastSelector();    
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
            OnEndTouch(m_TouchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);

        if (LevelManager.instance.canInteract)
            Hits();
    }

    private void MultiTappedPerformed(InputAction.CallbackContext context)
    {
        PlaceOnPlane.instance.StartGame(m_TouchControls.Touch.TouchPosition.ReadValue<Vector2>());
    }
    #endregion
}
