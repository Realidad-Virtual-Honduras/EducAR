using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlanetType{Sun, Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune, }
public class Planet : MonoBehaviour
{
    public static Planet instance;
    [SerializeField] Planet_Interaction[] interactions;

    [SerializeField] private bool[] isActive;
    [SerializeField] private bool isAllActive;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        isActive = new bool[interactions.Length];

        for(int i = 0; i < isActive.Length; i++)
            isActive[i] = false;
    }

    public void CheckActives()
    {
        for (int i = 0; i < interactions.Length; i++) 
        {
            isActive[i] = interactions[i].hasTheRightPlanet;
        }

        foreach (bool b in isActive)
        {
            if (b)
            {
                isAllActive = true;
            }
            else
            {
                Debug.Log("not true");
                isAllActive = false;
                break;
            }
        }

        if (isAllActive)
        {
            GameManager.instance.ActiveContinue(isAllActive);

            for (int i = 0; i < interactions.Length; i++)
            {
                interactions[i].StartRotation(true);
            }
        }
    }
}
