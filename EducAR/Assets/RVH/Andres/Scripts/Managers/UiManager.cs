using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public TextMeshProUGUI instructionsText;

    [Header("Button back from game")]
    [SerializeField, Tooltip("Boton del juego que regresa al menu")] private GameObject[] btnBackFromPoints;

    [Header("Elements To Change")]
    [SerializeField, Tooltip("Nombre de las clases")] private string[] classNames;
    [Tooltip("Color claro de las clases")] public Color[] lightClassColor;
    [Tooltip("Color obscuro de las clases")] public Color[] darkClassColor;
    [Tooltip("Color obscuro de los titulos de clases")] public Color[] darkTextClassColor;
    [SerializeField, Tooltip("Icono de las clases")] private Sprite[] spriteIconClass;
    [HideInInspector] public int selectedIdx;

    [Header("Class BG")]
    [SerializeField, Tooltip("Imagen de fondo de las clases en claro")] private Image[] imgLightClassBG;
    [SerializeField, Tooltip("Imagen de fondo de las clases en obscuro")] Image[] imgDarkClassBG;

    [Header("Class Icon")]
    [SerializeField, Tooltip("Imagen de fondo de las clases")] Image[] imgIconClass;

    [Header("Texts to change")]
    [SerializeField, Tooltip("Texto a ser cambiado por el color claro")] TextMeshProUGUI[] textToChangeColorLight;
    [SerializeField, Tooltip("Texto a ser cambiado por el color obscuro")] TextMeshProUGUI[] textToChangeColorDark;
    [SerializeField, Tooltip("Texto a ser cambiado por el nombre de la clase")] TextMeshProUGUI[] textToChangeClassName;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void ChangeElements(int idx)
    {
        selectedIdx = idx;
        #region Icon
        for (int i = 0; i < imgIconClass.Length; i++)
            imgIconClass[i].sprite = spriteIconClass[idx];
        #endregion

        #region Text
        for (int i = 0; i < textToChangeClassName.Length; i++)
            textToChangeClassName[i].text = classNames[idx];
        #endregion

        #region Color
        #region Light
        //BG
        for (int i = 0; i < imgLightClassBG.Length; i++)
            imgLightClassBG[i].color = lightClassColor[idx];

        //Text
        for (int i = 0; i < textToChangeColorLight.Length; i++)
            textToChangeColorLight[i].color = lightClassColor[idx];
        #endregion
        #region Dark
        //BG
        for (int i = 0; i < imgDarkClassBG.Length; i++)
            imgDarkClassBG[i].color = darkClassColor[idx];

        //Text
        for (int i = 0; i < textToChangeColorDark.Length; i++)
            textToChangeColorDark[i].color = darkTextClassColor[idx];
        #endregion
        #endregion
    }

    public void ActiveBtnBackFromPoints(int idx)
    {
        for (int i = 0; i < btnBackFromPoints.Length; i++)
            btnBackFromPoints[i].SetActive(false);

        btnBackFromPoints[idx].SetActive(true);
    }
}
