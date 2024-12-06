using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CategoryManager;

public class CategoryManager : MonoBehaviour
{
    public static CategoryManager Instance;

    //Prefabs Ui
    [SerializeField] private Transform btn_CreationPrefab;

    //Creation containers
    [SerializeField] private Transform sv_Creation;
    [SerializeField] private CategoryData[] categories;
    [Space]

    //Ui
    [SerializeField] private TextMeshProUGUI txt_Title;
    [SerializeField] private Button btn_Back;

    //Selected ones
    [Space]
    private CategoryData selectedCategory;
    private ObjectData selectedObject;
    private MaterialData selectedMaterial;
    public MaterialData SelectedMaterial()
    {
        return selectedMaterial; 
    }    

    private void Awake()
    {
        Instance = this;        
    }

    private void Start()
    {
        PopulateCategories();
    }

    public void DetectObjectAmount()
    {
        if(selectedObject.materials.Length >= 2)
        {
            PopulateMaterials(selectedObject);
        }
        else
        {
            PopulateObjects(selectedCategory);
        }
    }

    #region Populate Categories
    void PopulateCategories()
    {
        //Clean All Scroll Views
        CleanContentTables();

        //Assing Title
        txt_Title.text = "Categorias";

        //Hide and clean Back Button
        btn_Back.gameObject.SetActive(false);
        btn_Back.onClick.RemoveAllListeners();

        //Clean Selected Items
        selectedCategory = null;
        selectedObject = null;
        selectedMaterial = null;

        //Parse all categories to Populate
        foreach (CategoryData category in categories)
        {
            //Instance btns for each category
            Transform btn_Category = Instantiate(btn_CreationPrefab, sv_Creation);

            //Populate name and Icon
            btn_Category.GetComponentInChildren<Image>().sprite = category.categoryIcon;
            btn_Category.GetComponentInChildren<TextMeshProUGUI>().text = category.categoryName;

            //Populate buttons actions
            btn_Category.GetComponent<Button>().onClick.AddListener(() => {
                PopulateObjects(category);
            });
        }      
    }
    #endregion

    #region Populate Objects
    private void PopulateObjects(CategoryData category)
    {
        //Set selectex category
        selectedCategory = category;

        //Set title for de selected category
        txt_Title.text = selectedCategory.categoryName;

        //Clean All Scroll Views
        CleanContentTables();  

        //Reset back button
        btn_Back.gameObject.SetActive(true);
        btn_Back.onClick.RemoveAllListeners();

        //Set actions for back button
        btn_Back.onClick.AddListener(() => {
            PopulateCategories();
        });

        //Parse each element in category
        foreach (ObjectData objectData in category.objects)
        {
            //Error if a material of any object is less than 1
            if (objectData != null && objectData.materials.Length < 1)
            {
                Debug.LogError($"{objectData.objectName} doesn't have materials");
                return;
            }

            //Instance a button for each element
            Transform btn_Objects = Instantiate(btn_CreationPrefab, sv_Creation);

            //Populate name and Icon
            btn_Objects.GetComponentInChildren<Image>().sprite = objectData.objectIcon;
            btn_Objects.GetComponentInChildren<TextMeshProUGUI>().text = objectData.objectName;

            //Populate buttons actions
            btn_Objects.GetComponent<Button>().onClick.AddListener(() => {
                PopulateMaterials(objectData);
            });
        }
    }
    #endregion

    #region Populate Materials
    private void PopulateMaterials(ObjectData objectData)
    {
        //Set selected Object
        selectedObject = objectData;

        //Set title
        txt_Title.text = selectedObject.objectName + " de:";

        //Clean All scroll views
        CleanContentTables();

        //Reset back button
        btn_Back.gameObject.SetActive(true);
        btn_Back.onClick.RemoveAllListeners();

        //Set actions for back button
        btn_Back.onClick.AddListener(() => {
            PopulateObjects(selectedCategory);
        });


        //Check if Selected object has 2 or more materials
        if (selectedObject.materials != null && selectedObject.materials.Count() >= 2)
        {
            //Parce each material
            foreach (MaterialData materialData in selectedObject.materials)
            {
                //Insance all buttons for materials
                Transform btn_Materials = Instantiate(btn_CreationPrefab, sv_Creation);

                //Populate name and Icon
                btn_Materials.GetComponentInChildren<Image>().sprite = materialData.materialIcon;
                btn_Materials.GetComponentInChildren<TextMeshProUGUI>().text = materialData.materialName;

                //Populate buttons actions
                btn_Materials.GetComponent<Button>().onClick.AddListener(() => {
                    MaterialSameActions(materialData);
                });
            }
        }
        else
        {
            //Pass to the action if doesn't have 2 or more materials
            MaterialSameActions(selectedObject.materials[0]);
        }
    }

    private void MaterialSameActions(MaterialData data)
    {
        selectedMaterial = data;
        SessionManager.instance.SwapFactoryToCreated();
        SessionManager.instance.CanCreate(true);
        TapToPlace.instance.CreateObjectToVisualise(data.objectData.objectVisualPrefab);
    }
    #endregion

    #region Clean Scroll views
    private void CleanContentTables()
    {
        foreach (Transform child in sv_Creation)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion

    #region Public classes
    [System.Serializable]
    public class CategoryData
    {
        public string categoryName;
        public Sprite categoryIcon;
        public ObjectData[] objects;
    }

    [System.Serializable]
    public class ObjectData
    {
        public string objectName;
        public Sprite objectIcon;
        public MaterialData[] materials;
    }

    [System.Serializable]
    public class MaterialData
    {
        public string materialName;
        public Sprite materialIcon;
        public SO_PhysicalObjectData objectData;
    }
    #endregion
}
