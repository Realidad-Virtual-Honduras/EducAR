using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public TextMeshProUGUI instructionsText;
    
    void Awake()
    {
        if(instance == null)
            instance = this;
    }
}
