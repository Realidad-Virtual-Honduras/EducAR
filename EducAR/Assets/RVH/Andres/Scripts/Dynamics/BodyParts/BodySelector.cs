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
    [SerializeField] private GameObject[] finalPos;
    [SerializeField] private List<string> bodyPartsName;
    [SerializeField] private float waitTime;
    [Space]
    public Material bodyMaterialSelected;

    [SerializeField] private Animation bodyAnimation;
    
    [Header("Question Maker")]
    [SerializeField] private string questionPart1;
    [SerializeField] private string questionPart2;
    [SerializeField] private TextMeshProUGUI bodyQuestionText;
    [SerializeField] private string colorN;

    [Header("Events")]
    public UnityEvent placedObjectEvent;

    [Header("Rotation element")]
    [SerializeField] private Transform elementToRotate;
    [SerializeField] private float curRotation;


    private GameObject bodyPartSelected;
    public string nameSelected;
    [SerializeField] private string nameSelectedSpaced;

    void Awake()
    {
        if (instance == null)
            instance = this;

        SetEmptyAll();
        colorN = ColorToHexadecimal(UiManager.instance.darkTextClassColor[UiManager.instance.selectedIdx]);
    }

    private string ColorToHexadecimal(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    private void Start()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyPartsName.Add(bodyParts[i].GetComponent<EyePart_Interactor>().bodyPartEye.ToString());
        }
    }

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

            for (int i = 0; i < finalPos.Length; i++)
            {
                if(nameSelected != finalPos[i].GetComponent<EyePart_Detector>().bodyPartEye.ToString())
                {
                    finalPos[i].GetComponent<BoxCollider>().enabled = false;
                }
            }

            SpaceCapitals(nameSelected, true, true, false);

            bodyQuestionText.text = questionPart1 + "<color=" + colorN + ">" + nameSelectedSpaced + "</color>" + questionPart2;
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

        for (int i = 0; i < finalPos.Length; i++)
        {
            finalPos[i].GetComponent<BoxCollider>().enabled = true;
        }

        bodyPartSelected = null;
    }

    public void SetEmptyAll()
    {
        nameSelected = "";
        bodyQuestionText.text = "";
        bodyMaterialSelected.color = Color.white;       

        bodyPartsName.Clear();
    }

    #region Rotation
    public void RotateElement(float addRotation)
    {
        curRotation = elementToRotate.transform.eulerAngles.y + addRotation;
        elementToRotate.transform.eulerAngles = new Vector3(0, curRotation, 0);
    }
    #endregion
}
