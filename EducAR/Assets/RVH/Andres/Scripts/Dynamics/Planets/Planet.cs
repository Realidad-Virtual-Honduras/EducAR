using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public enum PlanetType{Sun, Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune, }
public class Planet : MonoBehaviour
{
    public static Planet instance;
    public Planet_Interaction[] interactions;
    public List<string> planetsNames = new List<string>();
    public UnityEvent onCorrect;
    public bool isAllActive;
    public Toggle toggle;
    public TextMeshProUGUI text;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        toggle.isOn = isAllActive;
        text.text = "";

        AddPlanets();
    }

    public void AddPlanets()
    {
        for (int i = 0; i < interactions.Length; i++)
        {
            planetsNames.Add(interactions[i].GetComponent<Planet_Interaction>().planetType.ToString());
        }

        foreach (var listmember in planetsNames)
        {
            text.text += "\n" + listmember.ToString();
        }
    }

    public void CheckActives()
    {
        if(planetsNames.Count == 0)
        {
            LevelManager.instance.WinGame();
        }
    }

    public void RemoveElement(string element)
    {
        planetsNames.Remove(element);

        text.text = "";

        foreach (var listmember in planetsNames)
        {
            text.text += "\n" + listmember.ToString();
        }
    }
}
