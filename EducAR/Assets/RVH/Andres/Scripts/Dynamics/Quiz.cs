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
    [SerializeField] private GameObject objectSelected;
    private string nameSelected;
    [Space]
    [SerializeField] private Material materialSelected;
    [SerializeField] private Color[] checkColor;
    [SerializeField,Range(0,10)] private float waitToCheck;

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
                //ChangeStatus(false);
                if (objectSelected.name == nameSelected)
                {
                    LevelManager.instance.ChangeColor(checkColor[0]);
                    LevelManager.instance.ShowQuestion(false);
                    //materialSelected.color = checkColor[0];
                    ChangeStatus(false);

                    objectsNames.Remove(nameSelected);
                    ChangeAlpha(1.1f);
                    Timing.PauseCoroutines("Timer");

                    yield return Timing.WaitForSeconds(waitToCheck);

                    if(hideObject)
                        objectSelected.SetActive(false);

                    Timing.ResumeCoroutines("Timer");
                    ChangeStatus(true);
                    ChangeAlpha(0f);
                    objectSelected = null;

                    eventOnCorrect.Invoke();
                    //GenerateQuestion();
                }
                else
                {
                    LevelManager.instance.ChangeColor(checkColor[1]);

                    ChangeStatus(false);

                    yield return Timing.WaitForSeconds(1f);

                    objectSelected.transform.position = objectSelected.GetComponent<DinosaurInfo>().newPos;
                    objectSelected.transform.rotation = Quaternion.Euler(objectSelected.GetComponent<DinosaurInfo>().newRotation.x, objectSelected.GetComponent<DinosaurInfo>().newRotation.y, objectSelected.GetComponent<DinosaurInfo>().newRotation.z);
                    ChangeStatus(true);
                    objectSelected = null;
                    TouchMananger.instance.UnSelectAll();
                    //ChangeStatus(true);
                    //materialSelected.color = checkColor[1];
                }
            }
        }
        yield return Timing.WaitForSeconds(waitToCheck);

        //objectSelected.GetComponent<ObjectSelector>().OnSelectObject();
        //objectSelected = null;

        //TouchMananger.instance.UnSelectAll();
        //ChangeStatus(true);

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

            question.text = questionPart1 + "<color=" + LevelManager.instance.colorImporantWord + ">" + nameSelected + "</color>" + questionPart2;
        }
    }

    public void ChangeStatus(bool active)
    {
        objectSelected.GetComponent<Collider>().enabled = active;
    }

    private void ChangeAlpha(float alpha)
    {
        LevelManager.instance.AlphaInMat(alpha);
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
            objs.GetComponent<DinosaurInfo>().newPos = randomPos.position; 
            objs.transform.rotation = randomPos.rotation;
            objs.GetComponent<DinosaurInfo>().newRotation = new Vector3(randomPos.rotation.x, randomPos.rotation.y, randomPos.rotation.z); 

            isTaken.RemoveAt(randomIdx);
        }
    }
    #endregion
}
