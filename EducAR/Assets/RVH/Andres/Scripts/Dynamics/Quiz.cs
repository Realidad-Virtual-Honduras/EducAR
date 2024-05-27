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
    [SerializeField] private List<string> objectsNames;
    private GameObject objectSelected;
    private string nameSelected;
    [Space]
    [SerializeField] private Material materialSelected;
    [SerializeField] private Color[] checkColor;
    [SerializeField,Range(0,1)] private float waitToCheck;
    [Space]
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private UnityEvent eventOnCorrect;

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
                    materialSelected.color = checkColor[0];

                    objectsNames.Remove(nameSelected);

                    yield return Timing.WaitForSeconds(waitToCheck);

                    objectSelected.SetActive(false);
                    GenerateQuestion();
                }
                else
                {
                    materialSelected.color = checkColor[1];
                }
            }
        }
        yield return Timing.WaitForSeconds(waitToCheck);

        objectSelected.GetComponent<ObjectSelector>().OnSelectObject();

        objectSelected = null;

        eventOnCorrect.Invoke();
    }

    public void GenerateQuestion()
    {
        if (objectsNames.Count == 0)
        {
            LevelManager.instance.WinGame();
            GameManager.instance.ActiveContinue(true);
            eventOnCorrect.Invoke();
        }
        else
        {
            int random = Random.Range(0, objectsNames.Count);
            nameSelected = objectsNames[random];

            question.text = "¿Qué dinosaruio es el: " + nameSelected + "? Selecciona el correcto";
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
}
