using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CategoryManager;

public class CategoryManager : MonoBehaviour
{
    private const string CATEGORIES = "Categories";
    private const string OBJECTS = "Objects";
    private const string MATERIALS = "Materials";

    public static CategoryManager Instance;

    [SerializeField] private PhysicalObjectFactory objectFactory;
    [SerializeField] private UiMediator uiMediator;

    //Prefabs Ui
    [SerializeField] private Transform btn_categoryPrefab;
    [SerializeField] private Transform btn_objectPrefab;
    [SerializeField] private Transform btn_materialPrefab;

    //Scroll view containers
    [SerializeField] private Transform sv_Category;
    [SerializeField] private Transform sv_Objects;
    [SerializeField] private Transform sv_Material;
    [Space]
    [SerializeField] private CategoryData[] categories;
    [SerializeField] private ObjectData selectedObject;
    [SerializeField] private CategoryData selectedCategory;
    [Space]
    [SerializeField] private TextMeshProUGUI txt_Title;
    [SerializeField] private Button btn_Back;

    private int categoriesPosList;
    private int objectsPosList;
    private int materialsPosList;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < uiMediator.Medators.Length; i++)
        {
            if (uiMediator.Medators[i].name == CATEGORIES)
            {
                categoriesPosList = i;
            }

            if (uiMediator.Medators[i].name == OBJECTS)
            {
                objectsPosList = i;
            }

            if (uiMediator.Medators[i].name == MATERIALS)
            {
                materialsPosList = i;
            }
        }
    }

    private void Start()
    {
        PopulateCategories();
    }

    void PopulateCategories()
    {
        CleanContentTables();

        txt_Title.text = "Categorias";

        btn_Back.gameObject.SetActive(false);
        btn_Back.onClick.RemoveAllListeners();

        selectedCategory = null;
        selectedObject = null;

        foreach (CategoryData category in categories)
        {
            Transform btn_Category = Instantiate(btn_categoryPrefab, sv_Category);

            btn_Category.GetComponentInChildren<Image>().sprite = category.categoryIcon;
            btn_Category.GetComponentInChildren<TextMeshProUGUI>().text = category.categoryName;

            //Assing listener to pass to de populate object
            btn_Category.GetComponent<Button>().onClick.AddListener(() => {
                PopulateObjects(category);
                uiMediator.ShowPanel(objectsPosList);
                uiMediator.HidePanel(categoriesPosList);
            });
        }      
    }

    private void PopulateObjects(CategoryData category)
    {
        selectedCategory = category;

        CleanContentTables();

        txt_Title.text = "Objetos";

        btn_Back.gameObject.SetActive(true);
        btn_Back.onClick.RemoveAllListeners();
        btn_Back.onClick.AddListener(() => {
            uiMediator.ShowPanel(categoriesPosList);
            uiMediator.HidePanel(objectsPosList);
            PopulateCategories();
        });

        foreach (ObjectData objectData in category.objects)
        {
            Transform btn_Objects = Instantiate(btn_objectPrefab, sv_Objects);

            btn_Objects.GetComponentInChildren<Image>().sprite = objectData.objectIcon;
            btn_Objects.GetComponentInChildren<TextMeshProUGUI>().text = objectData.objectName;

            btn_Objects.GetComponent<Button>().onClick.AddListener(() => {
                PopulateMaterials(objectData);
                uiMediator.ShowPanel(materialsPosList);
                uiMediator.HidePanel(objectsPosList);
            });
        }
    }

    private void PopulateMaterials(ObjectData objectData)
    {
        selectedObject = objectData;

        CleanContentTables();

        txt_Title.text = "Materiales";

        btn_Back.gameObject.SetActive(true);
        btn_Back.onClick.RemoveAllListeners();
        btn_Back.onClick.AddListener(() => {
            uiMediator.ShowPanel(objectsPosList);
            uiMediator.HidePanel(materialsPosList);
            PopulateObjects(selectedCategory);
        });

        if (selectedObject.materials != null && selectedObject.materials.Count() >= 2)
        {
            foreach (MaterialData materialData in selectedObject.materials)
            {
                Transform btn_Materials = Instantiate(btn_materialPrefab, sv_Material);

                btn_Materials.GetComponentInChildren<Image>().sprite = materialData.materialIcon;
                btn_Materials.GetComponentInChildren<TextMeshProUGUI>().text = materialData.materialName;

                btn_Materials.GetComponent<Button>().onClick.AddListener(() => {
                    MaterialSameActions();
                    //Set material
                });

                //Add the old system that create materials
            }
        }
        else
        {
            MaterialSameActions();
            //Hide panel
            //Tap to spawn 
        }
    }

    private void MaterialSameActions()
    {
        uiMediator.HidePanel(0);
        CreateObjects.instance.CanCreate(true);
    }

    private void CleanContentTables()
    {
        foreach (Transform child in sv_Category)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in sv_Objects)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in sv_Material)
        {
            Destroy(child.gameObject);
        }
    }

    public ObjectData objectDataPublic()
    {
        return selectedObject;
    }

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
        public SO_PhysicalObjectData objectData;
        public Sprite objectIcon;
        public MaterialData[] materials;
    }

    [System.Serializable]
    public class MaterialData
    {
        public string materialName;
        public Sprite materialIcon;
        public Material material;
    }
}
