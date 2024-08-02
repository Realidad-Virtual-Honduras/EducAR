using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UiManagerCreativeMode : MonoBehaviour
{
    public static UiManagerCreativeMode instance;

    [Header("Creation Panels")]
    [SerializeField] private GameObject creationObjects;
    [SerializeField] private GameObject categotyObjects;
    [SerializeField] private GameObject typeObjects;
    [SerializeField] private GameObject arrowDirection;

    [Header("Show N Hide Properties")]
    [SerializeField] private float timer;
    [SerializeField] private float categoryNTypeHide;
    [SerializeField] private Vector2 creationHide;

    [Header("Swipe Menus")]
    [SerializeField] private SwipeMenu[] swipeMenus;

    [Header("Options Properties")]
    [SerializeField] private TextMeshProUGUI titleOptions;
    public GameObject[] btnsAcitve;
    public Image[] imageType;
    public TextMeshProUGUI[] titleType;
    

    bool showCreation = false , showCategory = false, showType = true;

    void Awake()
    {
        if(instance == null)
            instance = this;

        showCreation = true;
        showCategory = false;
        showType = true;
    }

    private void Start()
    {
        ShowNHydeCreation();
        ShowNHydeCategory();
    }

    public void ShowNHydeCreation()
    {
        showCreation = !showCreation;
        

        if (showCreation)
        {
            creationObjects.transform.DOLocalMoveY(creationHide.x, timer);
            arrowDirection.transform.DORotate(new Vector3(0, 0, -90), timer);
            RestartAllScrollPos();
        }
        else
        {
            creationObjects.transform.DOLocalMoveY(creationHide.y, timer);
            arrowDirection.transform.DORotate(new Vector3(0, 0, 90), timer);            
        }

        
    }

    public void ShowNHydeCategory()
    {
        showCategory = !showCategory;

        if(showCategory)
        {
            categotyObjects.transform.DOLocalMoveY(categoryNTypeHide, timer);
            UiManagerCreativeMode.instance.titleOptions.text = "Objetos";
        }
        else
        {
            categotyObjects.transform.DOLocalMoveY(-categoryNTypeHide, timer);
            UiManagerCreativeMode.instance.titleOptions.text = "Material";
        }

        ShowNHydeType();
    }

    public void ShowNHydeType()
    {
        showType = !showCategory;

        if(showType)
            typeObjects.transform.DOLocalMoveY(categoryNTypeHide, timer);
        else
            typeObjects.transform.DOLocalMoveY(-categoryNTypeHide, timer);

        RestartAllScrollPos();
    }


    private void RestartAllScrollPos()
    {
        for (int i = 0; i < swipeMenus.Length; i++)
            swipeMenus[i].scrollPos = 0;
    }
   
}
