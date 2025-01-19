using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CategoryManager;

public class ClassroomFactory : MonoBehaviour
{
    public static ClassroomFactory instance;

    [SerializeField] private ClassesData[] allClasses;
    [Space(20)]

    //Prefabs Ui
    [Header("Prefabs Buttons")]
    [SerializeField] private Transform btn_openClass;
    [SerializeField] private Transform btn_openHomeworks;
    [SerializeField] private Transform btn_creativeMode;
    [SerializeField] private Transform btn_Activities;

    //Creation containers
    [Header("Containers To Create Objets")]
    [SerializeField] private Transform sv_Classes;
    [SerializeField] private Transform sv_ClassesActivity;
    [SerializeField] private Transform sv_Activities;

    //Ui
    [Header("Ui")]
    [SerializeField] private TextMeshProUGUI txt_TitleClasses;
    [SerializeField] private TextMeshProUGUI txt_TitleActivities;
    [SerializeField] private string titleClasses;
    [SerializeField] private string titleActivities;

    //Selected ones
    [Space]
    private ClassesData selectedClass;
    private ActivityData selectedActivity;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PopulateClasses();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Populate Classe
    public void PopulateClasses()
    {
        //Clean All Scroll Views
        CleanContentTables();

        //Assing Title
        txt_TitleClasses.text = titleClasses;
        txt_TitleActivities.text = titleActivities;

        //Clean Selected Items
        selectedClass = null;
        selectedActivity = null;

        //Parse all categories to Populate
        foreach (ClassesData classes in allClasses)
        {
            //Instance btns for each class
            Transform btn_Classes = Instantiate(btn_openClass, sv_Classes);
            Transform btn_ClasesActivities = Instantiate(btn_openHomeworks, sv_ClassesActivity);

            //Populate name and Icon
            btn_Classes.GetComponent<ClassroomButtonInformation>().classIcon.sprite = classes.classIcon;
            btn_Classes.GetComponent<ClassroomButtonInformation>().classTitle.text = classes.className;            

            //Homeworks
            btn_ClasesActivities.GetComponent<ClassroomButtonInformation>().classIcon.sprite = classes.classIcon;
            btn_ClasesActivities.GetComponent<ClassroomButtonInformation>().classTitle.text = classes.className;


            if (UiManager.instance.useDarkMode)
            {
                btn_Classes.GetComponent<ClassroomButtonInformation>().classBackground.color = classes.darkBgClassColor;
                btn_Classes.GetComponent<ClassroomButtonInformation>().classTitle.color = classes.darkTextClassColor;

                btn_ClasesActivities.GetComponent<ClassroomButtonInformation>().classBackground.color = classes.darkBgClassColor;
                btn_ClasesActivities.GetComponent<ClassroomButtonInformation>().classTitle.color = classes.darkTextClassColor;
            }
            else
            {
                btn_Classes.GetComponent<ClassroomButtonInformation>().classBackground.color = classes.lightBgClassColor;
                btn_Classes.GetComponent<ClassroomButtonInformation>().classTitle.color = classes.lightTextClassColor;

                btn_ClasesActivities.GetComponent<ClassroomButtonInformation>().classBackground.color = classes.lightBgClassColor;
                btn_ClasesActivities.GetComponent<ClassroomButtonInformation>().classTitle.color = classes.lightTextClassColor;
            }

            //Populate buttons clases
            btn_Classes.GetComponent<ClassroomButtonInformation>().btn_.onClick.AddListener(() => 
            {
                UiMediator.instance.GoToUi(UiManager.instance.ShowScoreValueUi());
                UiManager.instance.ChangeUi(classes.className, classes.classIcon, classes.lightBgClassColor, classes.darkBgClassColor, classes.darkTextClassColor);
                //PopulateObjects(classes);
            });

            //Populate buttons activities
            btn_ClasesActivities.GetComponent<ClassroomButtonInformation>().btn_.onClick.AddListener(() =>
            {
               
                UiMediator.instance.GoToUi(UiManager.instance.ActivitiesValueUi());
                UiManager.instance.ChangeUi(classes.className, classes.classIcon, classes.lightBgClassColor, classes.darkBgClassColor, classes.darkTextClassColor);                

                PopulateActivities(classes);
            });
        }

        Transform btn_creativeModes = Instantiate(btn_creativeMode, sv_ClassesActivity);
    }
    #endregion

    #region Populete Activity
    private void PopulateActivities(ClassesData classesData)
    {
        //Set Selected Class
        selectedClass = classesData;

        //Instance btns for each class
        //Transform btn_Activity = Instantiate(btn_Activities, sv_Activities);

        //Populate data
        //btn_Classes.GetComponent<ClassroomButtonInformation>().classIcon.sprite = classes.classIcon;

        foreach (ActivityData activities in selectedClass.Activities)
        {
            //Btn Listener
            { 
                //GameBehaviour.instance.GameInitialize();
                //GameManager.instance.LoadScene(GameManager.instance.classesDirections + activities.activityName);
            }
        }
    }
    #endregion

    #region Clean Scroll views
    private void CleanContentTables()
    {
        foreach (Transform child in sv_Classes)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in sv_ClassesActivity)
        {
            Destroy(child.gameObject);
        }

        /*foreach (Transform child in sv_Activities)
        {
            Destroy(child.gameObject);
        }*/
    }
    #endregion

    #region Public classes
    [System.Serializable]
    public class ClassesData
    {
        public string className;
        public Sprite classIcon;

        [Header("Light Colors")]
        public Color lightBgClassColor;
        public Color lightTextClassColor;

        [Header("Dark Colors")]
        public Color darkBgClassColor;
        public Color darkTextClassColor;

        [Header("Activities")]
        public ActivityData[] Activities;
    }

    [System.Serializable]
    public class ActivityData
    {
        public string activityName;
        public Sprite activityIcon;
        public string activityDescription;
        public GameObject activityComplete;
        //public MaterialData[] materials;
    }

    /*[System.Serializable]
    public class MaterialData
    {
        public string materialName;
        public Sprite materialIcon;
        public SO_PhysicalObjectData objectData;
    }*/
    #endregion
}
