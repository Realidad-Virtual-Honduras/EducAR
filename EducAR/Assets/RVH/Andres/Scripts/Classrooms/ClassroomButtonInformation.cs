using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassroomButtonInformation : MonoBehaviour
{
    public static ClassroomButtonInformation instance;

    public TextMeshProUGUI classTitle;
    public Image classIcon;
    public Image classBackground;
    public Button btn_;

    void Awake()
    {
        instance = this;
    }
}
