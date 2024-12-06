using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using EnhandedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;

    [SerializeField] private UiMediator uiMediator;
    private XRRayInteractor _interactor;
    private RaycastHit hit;

    private bool canCreateObject;
    private int amountCreated;
    [SerializeField] private int maxAmount = 1;

    [SerializeField] private GameObject selectedObject;

    [Header("Show And Hide Factory Panel")]
    [SerializeField] private TextMeshProUGUI txt_TemporalItemShowAndHideFactory;
    [SerializeField] private CanvasGroup CG_ShowNHidePanel;
    private bool isFactoryShow;
    private bool isCreatingObject;
    private bool isSelectedObject;

    private const string FACTORY = "Factory";
    private const string CREATION = "CreationConfig";
    private const string CATEGORIES = "Categories";
    private const string SELECTED = "SelectedConfig";

    //Position in the array
    private int factoryPosList;
    private int creaticionConfigPosList;
    private int catedoriesPosList;
    private int selectedPosList;

    void Awake()
    {
        if (instance == null)
            instance = this;

        canCreateObject = false;
        amountCreated = 0;

        _interactor = FindObjectOfType<XRRayInteractor>();
        _interactor.maxRaycastDistance = distanceRay;

        #region Array Mediator
        //Parse each element in the mediator yo detect where is each element
        for (int i = 0; i < uiMediator.Medators.Length; i++)
        {
            if (uiMediator.Medators[i].name == FACTORY)
            {
                factoryPosList = i;
            }

            if (uiMediator.Medators[i].name == CREATION)
            {
                creaticionConfigPosList = i;
            }

            if (uiMediator.Medators[i].name == CATEGORIES)
            {
                catedoriesPosList = i;
            }

            if (uiMediator.Medators[i].name == SELECTED)
            {
                selectedPosList = i;
            }
        }
        #endregion
    }

    private void Start()
    {
        ShowFactory();
    }

    [SerializeField] private float distanceRay;

    #region Array Positions
    public int FactoryPos()
    {
        return factoryPosList;
    }

    public int CreationConfigPos() 
    {
        return creaticionConfigPosList;
    }

    public int CatedoriesPos()
    {
        return catedoriesPosList;
    }

    public int SelectedPos()
    {
        return selectedPosList;
    }
    #endregion

    #region Creation Objects
    #region Can Create
    //Modify bool
    public void CanCreate(bool active)
    {
        canCreateObject = active;
    }

    //Return Bool
    public bool CanCreateObjects()
    {
        return canCreateObject;
    }
    #endregion

    #region Amount
    //Return Current Amoutn
    public int AmountCreated()
    {
        return amountCreated;
    }

    //Return Max Amount Value
    public int MaxAmount()
    {
        return maxAmount;
    }

    //Add 1 to the amount
    public void AddAmount()
    {
        amountCreated++;
    }

    //Reset Values
    public void ResetAmount()
    {
        amountCreated = 0;
    }
    #endregion
    #endregion

    #region Show N Hide
    #region Factory
    public void HideFactory()
    {
        uiMediator.HidePanel(factoryPosList);
        txt_TemporalItemShowAndHideFactory.text = "<<";
        isFactoryShow = false;
    }

    public void ShowFactory()
    {
        uiMediator.ShowPanel(factoryPosList);
        txt_TemporalItemShowAndHideFactory.text = ">>";
        isFactoryShow = true;
    }

    public void ShowNHideFactory()
    {
        if(!isSelectedObject && !isCreatingObject)
        {
            if (isFactoryShow) 
            {
                HideFactory();
            }
            else
            {
                ShowFactory();
            }
        }
    }
    #endregion
    #endregion

    #region Swaps
    #region Selected
    public void SwapFactoryToSelected()
    {
        HideFactory();
        uiMediator.ShowPanel(selectedPosList);
        isSelectedObject = true;
        CG_ShowNHidePanel.ignoreParentGroups = false;
    }

    public void SwapSelectedToFactory()
    {
        uiMediator.HidePanel(selectedPosList);
        ShowFactory();
        isSelectedObject = false;
        CG_ShowNHidePanel.ignoreParentGroups = true;
        selectedObject = null;
    }
    #endregion

    #region Created
    public void SwapFactoryToCreated()
    {
        HideFactory();
        uiMediator.ShowPanel(creaticionConfigPosList);
        isCreatingObject = true;
        CG_ShowNHidePanel.ignoreParentGroups = false;
    }

    public void SwapCreatedToFactory()
    {
        uiMediator.HidePanel(creaticionConfigPosList);
        ShowFactory();
        isCreatingObject = false;
        CG_ShowNHidePanel.ignoreParentGroups = true;
    }
    #endregion
    #endregion

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
        //RaycastSelector();
    }

    private void FingerUp(EnhandedTouch.Finger finger)
    {
        RaycastSelector();
    }

    public void DestroySelected()
    {
        Destroy(selectedObject);
        selectedObject = null;
        SwapSelectedToFactory();
    }

    private void RaycastSelector()
    {
        if(_interactor.TryGetCurrent3DRaycastHit(out hit)) 
        {
            if(hit.transform.gameObject.GetComponent<ObjectSelector>() != null)
            {
                selectedObject = hit.transform.gameObject;
                selectedObject.GetComponent<ObjectSelector>().OnSelectObject();
                SwapFactoryToSelected();
                Debug.Log(hit.transform.name);
            }
            else if (hit.transform.gameObject.GetComponent<ObjectSelector>() == null)
            {
                if(selectedObject != null)
                {
                    selectedObject.GetComponent<ObjectSelector>().OnSelectObject();
                    SwapSelectedToFactory();
                    //selectedObject = null;
                }
            }
        }
        /*if (Physics.Raycast(ray, out hit, distanceRay))
        {
            objectSelector = hit.transform.GetComponent<ObjectSelector>();
            float distancef = Vector3.Distance(_camera.transform.position, objectSelector.gameObject.transform.position);
            Debug.DrawRay(ray.direction, _interactor.rayOriginTransform.position * (distancef * 10f), Color.white);
            Debug.Log("La distancia del objeto es: " + distancef * 10f);
        }
        else
        {

            objectSelector = null;
        }*/
    }
}
