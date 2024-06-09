using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using MEC;

public class Quiz : MonoBehaviour
{
    public static Quiz instance;
    [SerializeField] private GameObject[] objectsOfQuiz;
    [SerializeField] private Transform[] pos;
    [SerializeField] private List<string> objectsNames;
    private GameObject objectSelected;
    private string nameSelected;
    [Space]
    [SerializeField] private Material materialSelected;
    [SerializeField] private Color[] checkColor;
    [SerializeField,Range(0,1)] private float waitToCheck;

    [Header("Question Maker")]
    [SerializeField] private string questionPart1;
    [SerializeField] private string questionPart2;
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private UnityEvent eventOnCorrect;
    [SerializeField] private bool hideObject;
    private List<Transform> isTaken;

    void Awake()
    {
        if (instance == null)
            instance = this;

        SetEmptyAll();

        for(int i = 0; i < objectsOfQuiz.Length; i++)
        {
            objectsNames.Add(objectsOfQuiz[i].name);
        }
    }

    public void CheckCorrect(int idx)
    {      
        Timing.RunCoroutine(CheckTimeCorrect(idx));
    }

    private IEnumerator<float> CheckTimeCorrect(int idx)
    {
        objectSelected = objectsOfQuiz[idx];
        materialSelected.color = Color.white;

        if (objectSelected != null)
        {
            for (int i = 0; i < objectsNames.Count; i++)
            {
                if (objectSelected.name == nameSelected)
                {
                    LevelManager.instance.ChangeColor(checkColor[0]);
                    //materialSelected.color = checkColor[0];

                    objectsNames.Remove(nameSelected);

                    yield return Timing.WaitForSeconds(waitToCheck);

                    if(hideObject)
                        objectSelected.SetActive(false);

                    eventOnCorrect.Invoke();
                    //GenerateQuestion();
                }
                else
                {
                    LevelManager.instance.ChangeColor(checkColor[1]);
                    //materialSelected.color = checkColor[1];
                }
            }
        }
        yield return Timing.WaitForSeconds(waitToCheck);

        //objectSelected.GetComponent<ObjectSelector>().OnSelectObject();
        TouchMananger.instance.UnSelectAll();

        objectSelected = null;

        //eventOnCorrect.Invoke();
    }

    public void GenerateQuestion()
    {
        if (objectsNames.Count == 0)
        {
            LevelManager.instance.WinGame();
            GameManager.instance.ActiveContinue(true);            
        }
        else
        {
            int random = Random.Range(0, objectsNames.Count);
            nameSelected = objectsNames[random];

            question.text = questionPart1 + "<color=#0153ff>" + nameSelected + "</color>" + questionPart2;
        }
    }

    public void SetEmptyAll()
    {
        nameSelected = "";
        question.text = "";
        materialSelected.color = Color.white;

        objectsNames.Clear();

        for (int i = 0;i < objectsOfQuiz.Length;i++)
            objectsOfQuiz[i].SetActive(false);
    }

    #region Shuffle
    public void Shuffle()
    {
        isTaken = new List<Transform>(pos);

        foreach (GameObject objs in objectsOfQuiz)
        {
            int randomIdx = Random.Range(0, isTaken.Count);
            Transform randomPos = isTaken[randomIdx];
            objs.transform.SetParent(randomPos);

            objs.transform.position = randomPos.position;
            objs.transform.rotation = randomPos.rotation;

            isTaken.RemoveAt(randomIdx);
        }
    }
    #endregion
}
