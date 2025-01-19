using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class UiManager : MonoBehaviour
{
    //Instace Class
    public static UiManager instance;

    [Header("Scripts")]
    [SerializeField] private UiMediator uiMediator;

    #region Strings Ui
    [Header("Strings Ui")]
    [SerializeField] private string loging;
    [SerializeField] private string register;
    [SerializeField] private string mainMenu;
    [SerializeField] private string instructionsActivity;
    [SerializeField] private string showScore;
    [SerializeField] private string inGame;
    [SerializeField] private string classInfo;
    [SerializeField] private string activities;
    [SerializeField] private string options;
    [SerializeField] private string profile;

    private int logingValuePositionUi;
    private int registerValuePositionUi;
    private int mainMenuValuePositionUi;
    private int instructionsActivityValuePositionUi;
    private int showScoreValuePositionUi;
    private int inGameValuePositionUi;
    private int classInfoValuePositionUi;
    private int activityValuePositionUi;
    private int optionsValuePositionUi;
    private int profileValuePositionUi;

    #region Public Values Ui
    //Loging
    public int LogingValueUi()
    {
        return logingValuePositionUi;
    }

    //Register
    public int RegisterValueUi()
    {
        return registerValuePositionUi;
    }

    //Main Menu
    public int MainMenuValueUi()
    {
        return mainMenuValuePositionUi;
    }

    //Instructions
    public int InstructionsActivityValueUi()
    {
        return instructionsActivityValuePositionUi;
    }

    //Show Score
    public int ShowScoreValueUi()
    {
        return showScoreValuePositionUi;
    }

    //In Game
    public int InGameValueUi()
    {
        return inGameValuePositionUi;
    }

    //Classes
    public int ClassesValueUi()
    {
        return classInfoValuePositionUi;
    }

    //Activities
    public int ActivitiesValueUi()
    {
        return activityValuePositionUi;
    }

    //Options
    public int OptionsValueUi()
    {
        return optionsValuePositionUi;
    }

    //Profile
    public int ProfileValueUi()
    {
        return profileValuePositionUi;
    }
    #endregion
    #endregion

    [Header("Common")]
    public TextMeshProUGUI instructionsText;
    public bool useDarkMode = true;

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

        #region Detect Mediator Number;
        for (int i = 0; i < uiMediator.Medators.Length; i++)
        {
            //Loging
            if (uiMediator.Medators[i].name == loging)
            {
                logingValuePositionUi = i;
            }

            //Register
            if (uiMediator.Medators[i].name == register)
            {
                registerValuePositionUi = i;
            }

            //Main Menu
            if (uiMediator.Medators[i].name == mainMenu)
            {
                mainMenuValuePositionUi = i;
            }

            //Instructions
            if (uiMediator.Medators[i].name == instructionsActivity)
            {
                instructionsActivityValuePositionUi = i;
            }

            //Show Score
            if (uiMediator.Medators[i].name == showScore)
            {
                showScoreValuePositionUi = i;
            }

            //In Game
            if (uiMediator.Medators[i].name == inGame)
            {
                inGameValuePositionUi = i;
            }

            //Class info
            /*if (uiMediator.Medators[i].name == classInfo)
            {
                classInfoValuePositionUi = i;
            }*/

            //Activities
            if (uiMediator.Medators[i].name == activities)
            {
                activityValuePositionUi = i;
            }

            //Options
            /*if (uiMediator.Medators[i].name == options)
            {
                optionsValuePositionUi = i;
            }

            //Profile
            if (uiMediator.Medators[i].name == profile)
            {
                profileValuePositionUi = i;
            }*/
        }
        #endregion
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

    public void ChangeUi(string className, Sprite classIcon, Color lightColor, Color darkColor, Color darkText)
    {
        #region Icon
        for (int i = 0; i < imgIconClass.Length; i++)
            imgIconClass[i].sprite = classIcon;
        #endregion

        #region Text
        for (int i = 0; i < textToChangeClassName.Length; i++)
            textToChangeClassName[i].text = className;
        #endregion

        #region Color
        #region Light
        //BG
        for (int i = 0; i < imgLightClassBG.Length; i++)
            imgLightClassBG[i].color = lightColor;

        //Text
        for (int i = 0; i < textToChangeColorLight.Length; i++)
            textToChangeColorLight[i].color = lightColor;
        #endregion
        #region Dark
        //BG
        for (int i = 0; i < imgDarkClassBG.Length; i++)
            imgDarkClassBG[i].color = darkColor;

        //Text
        for (int i = 0; i < textToChangeColorDark.Length; i++)
            textToChangeColorDark[i].color = darkText;
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
