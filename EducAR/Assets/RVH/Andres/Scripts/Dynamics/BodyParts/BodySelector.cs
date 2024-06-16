using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using MEC;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;

public class BodySelector : MonoBehaviour
{
    public static BodySelector instance;
    [SerializeField] private GameObject[] bodyParts;
    public GameObject[] finalPos;
    [SerializeField] private List<string> bodyPartsName;
    [SerializeField] private List<string> allBodyPartsName;
    [SerializeField] private float waitTime;
    [Space]
    public Material bodyMaterialSelected;

    [SerializeField] private Animation bodyAnimation;
    
    [Header("Question Maker")]
    [SerializeField] private string questionPart1;
    [SerializeField] private string questionPart2;
    [SerializeField] private TextMeshProUGUI bodyQuestionText;

    [Header("Events")]
    public UnityEvent placedObjectEvent;

    [Header("Rotation element")]
    [SerializeField] private Transform elementToRotate;
    [SerializeField] private float curRotation;

    [Header("Element List To show")]
    [SerializeField] private List<GameObject> objectsToSelect;
    [SerializeField] private List<GameObject> objectsNoSelected;
    [SerializeField, Range(1, 10)] private int objectsToSelectIndex;
    [SerializeField] private bool isInList;
    [SerializeField] private GameObject ob;

    private GameObject bodyPartSelected;
    public string nameSelected;
    [SerializeField] private string nameSelectedSpaced;

    void Awake()
    {
        if (instance == null)
            instance = this;

        SetEmptyAll();

        objectsNoSelected = bodyParts.ToList();

        objectsToSelect = SelectRandomObjects(bodyParts.ToList(), objectsToSelectIndex);

        for (int i = 0; i < finalPos.Length; i++)
        {
            allBodyPartsName.Add(finalPos[i].GetComponent<EyePart_Detector>().bodyPartEye.ToString());
        }

    }

    private void Start()
    {

        RePoseElementsObjects();

        

        /*for (int i = 0; i < finalPos.Length; i++)
        {
            isInList = IsObjectInListString(finalPos.ToList(), allBodyPartsName[i]);
            Debug.Log($"¿Esta el nombre {objectsNoSelected[i].GetComponent<EyePart_Detector>().bodyPartEye} en la lista? R:{isInList}");
        }*/




        /*for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyPartsName.Add(bodyParts[i].GetComponent<EyePart_Interactor>().bodyPartEye.ToString());
        }*/
    }

    private void RePoseElementsObjects()
    {
        foreach (GameObject obj in finalPos)
        {
            isInList = IsObjectInListString(objectsNoSelected, obj.GetComponent<EyePart_Detector>().bodyPartEye.ToString());

            if(isInList)
            {
                //Debug.Log($"¿Esta el nombre {obj.GetComponent<EyePart_Detector>().bodyPartEye.ToString()} en la lista? R:{isInList}");               
            }
            //Debug.Log($"¿Esta el nombre {obj.GetComponent<EyePart_Detector>().bodyPartEye.ToString()} en la lista? R:{isInList}");
        }

        foreach (GameObject tem in objectsNoSelected)
        {
            ob = IsObjectInList(finalPos.ToList(), tem.GetComponent<EyePart_Interactor>().bodyPartEye.ToString());

            if(ob != null)
            {
                tem.transform.SetParent(ob.transform);

                tem.transform.position = ob.transform.position;
                tem.transform.rotation = ob.transform.rotation;
                tem.transform.localScale = ob.transform.localScale;
                tem.GetComponent<Collider>().enabled = false;
                tem.GetComponentInChildren<UiLookAt>().startUi(false);

                Debug.Log($"El objeto {tem.name} se posisiono como hijo de: {ob.name}");
            }    
        }

        ShuffleElements.instance.objects = new GameObject[objectsToSelect.Count];

        for (int i = 0; i < ShuffleElements.instance.objects.Length; i++)
        {
            ShuffleElements.instance.objects[i] = objectsToSelect[i];
        }
    }

    #region Selector de objetos random
    List<GameObject> SelectRandomObjects(List<GameObject> objects, int n)
    {
        List<GameObject> result = new List<GameObject>();

        if (n > objects.Count)
        {
            Debug.LogError("The amount of objects to choose is greater than the list");
            return result;
        }

        List<GameObject> tempList = new List<GameObject>(objects);
        for (int i = 0; i < n; i++)
        {
            int randomIdx = Random.Range(0, tempList.Count);
            result.Add(tempList[randomIdx]);
            objectsNoSelected.RemoveAt(randomIdx);

            bodyPartsName.Add(tempList[randomIdx].GetComponent<EyePart_Interactor>().bodyPartEye.ToString());
            tempList.RemoveAt(randomIdx);   
        }


        return result;
    }

    GameObject IsObjectInList(List<GameObject> objects, string obj)
    {
        foreach (GameObject objs in objects)
        {
            if (objs.GetComponent<EyePart_Detector>().bodyPartEye.ToString() == obj)
                return objs;
        }
        return null;
    }

    bool IsObjectInListString(List<GameObject> objects, string part)
    {
        foreach (GameObject obj in objects)
        {
            if(obj.GetComponent<EyePart_Interactor>().bodyPartEye.ToString() == part)
                return true;
        }
        return false;
    }
    #endregion

    public void SetStartPos()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].GetComponent<BodyPartInfo>().startPos = bodyParts[i].transform.position;
        }
    }

    public void GenerateQuestion()
    {
        if (bodyPartsName.Count == 0)
        {
            bodyAnimation.Play("idle");
            LevelManager.instance.WinGame();
            GameManager.instance.ActiveContinue(true);
        }
        else
        {
            int random = Random.Range(0, bodyPartsName.Count);
            nameSelected = bodyPartsName[random]; 

            SpaceCapitals(nameSelected, true, true, false);

            bodyQuestionText.text = questionPart1 + "<color=" + LevelManager.instance.colorImporantWord + ">" + nameSelectedSpaced + "</color>" + questionPart2;
        }
    }

    public string SpaceCapitals(string value, bool spaceLowerChar = true, bool spaceDigitalChar = true, bool spaceSymbolChar = false)
    {
        nameSelectedSpaced = "";

        for (int i = 0; i < value.Length; i++)
        {
            char currentChar = value[i];
            char nextChar = value[i < value.Length - 1 ? i + 1 : value.Length - 1];

            if (spaceLowerChar && char.IsLower(currentChar) && !char.IsLower(nextChar))
                nameSelectedSpaced += value[i] + " ";
            else if (spaceDigitalChar && char.IsDigit(currentChar) && !char.IsDigit(nextChar))
                nameSelectedSpaced += value[i] + " ";
            else if (spaceSymbolChar && char.IsSymbol(currentChar) && !char.IsSymbol(nextChar))
                nameSelectedSpaced += value[i];
            else
                nameSelectedSpaced += value[i];
        }

        return nameSelectedSpaced;
    }

    public void SelectBodyPart(int idx)
    {
        bodyPartSelected = bodyParts[idx];        
    }

    public void OnCorrect()
    {
        bodyPartsName.Remove(nameSelected);
        bodyPartSelected = null;
    }

    public void SetEmptyAll()
    {
        nameSelected = "";
        bodyQuestionText.text = "";
        bodyMaterialSelected.color = Color.white;       

        bodyPartsName.Clear();
    }    

    public void AllElementsPos()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            //bodyParts[i].GetComponent<BodyPartInfo>().startPos = 
        }
    }

    #region Rotation
    public void RotateElementRight(float addRotation)
    {
        curRotation = elementToRotate.transform.localEulerAngles.y + addRotation;

        elementToRotate.transform.eulerAngles = new Vector3(0, curRotation, 0);
    }

    public void RotateElementLeft(float addRotation)
    {
        curRotation = elementToRotate.transform.localEulerAngles.y - addRotation;

        elementToRotate.transform.eulerAngles = new Vector3(0, curRotation, 0);
    }
    #endregion
}
