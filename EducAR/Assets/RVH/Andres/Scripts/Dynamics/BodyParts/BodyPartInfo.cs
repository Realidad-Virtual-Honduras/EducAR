using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MEC;

public class BodyPartInfo : MonoBehaviour
{
    public static BodyPartInfo instance;
    [SerializeField] private string informationPart;
    public Vector3 startPos;
    [SerializeField] private GameObject info;
    [SerializeField] private TextMeshProUGUI infoText;
    private bool isShowed = true;

    void Awake()
    {
        if(instance == null)
            instance = this;

        ShowInfo();
    }

    public void ShowInfo()
    {
        isShowed = !isShowed;
        info.SetActive(isShowed);

        if(isShowed) 
        {
            infoText.text = informationPart;
        }
        else
        {
            infoText.text = "";
        }
    }
}
