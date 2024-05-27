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
    public bool isAllActive;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        isActive = new bool[interactions.Length];

        for(int i = 0; i < isActive.Length; i++)
            isActive[i] = false;
    }

    private void Update()
    {
        //CheckActives();
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
                //break;
            }
        }

        if (isAllActive)
        {
            LevelManager.instance.WinGame();
            //GameManager.instance.ActiveContinue(isAllActive);
        }
    }
}
