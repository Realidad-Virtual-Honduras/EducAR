using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateObjects : MonoBehaviour
{
    public enum ObjectForm { Cube, Sphere, Pyramid, Torus, Cylinder, Gun}

    public static CreateObjects instance;

    public bool canCreateObject;
    [HideInInspector] public int amountCreated;
    public int maxAmount;
    [SerializeField] private NewTypeImages[] newTypeImages;    

    [HideInInspector] public int selectedType, selectedCategory;

    void Awake()
    {
        if (instance == null)
            instance = this;

        canCreateObject = false;
        amountCreated = 0;
    }

    public void CreateObject(Vector3 pos, Quaternion rotation)
    {
        Instantiate(newTypeImages[selectedCategory].typeVariant[selectedType], pos, rotation, transform);
    }

    public void SetType(int type)
    {
        selectedType = type;
    }

    public void SetCategory(int category)
    {
        selectedCategory = category;
    }

    public void CanCreate(bool active)
    {
        canCreateObject = active;
    }

    public void AcitveType()
    {
        for (int i = 0; i < UiManagerCreativeMode.instance.btnsAcitve.Length; i++)
        {
            UiManagerCreativeMode.instance.btnsAcitve[i].SetActive(false);

            if (i <= newTypeImages[selectedCategory].typeVariant.Length - 1)
            {
                UiManagerCreativeMode.instance.btnsAcitve[i].SetActive(true);
                UiManagerCreativeMode.instance.imageType[i].sprite = newTypeImages[selectedCategory].spriteType[i];
                UiManagerCreativeMode.instance.titleType[i].text = newTypeImages[selectedCategory].typeNames[i];
            }
        }
    }
}

[System.Serializable]
public class NewTypeImages
{
    public string tName;
    public string[] typeNames;
    public Sprite[] spriteType;
    public GameObject[] typeVariant;
}