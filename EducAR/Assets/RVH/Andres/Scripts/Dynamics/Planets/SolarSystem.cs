using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public static SolarSystem instance;
    [SerializeField] private GameObject[] allPlanets;
    [SerializeField] private Vector3[] allPos;
    [SerializeField] private float distanceRandom;
    [SerializeField] private float distanceVRandom;

    void Awake()
    {        
        if(instance == null)
            instance = this;

        allPos = new Vector3[allPlanets.Length];
        Shuffle();
    }

    public void Shuffle()
    {
        for (int i = 0; i < allPlanets.Length; i++)
        {
            allPlanets[i].transform.position = Vector3.zero;
            allPos[i] = new Vector3(Random.Range(-distanceRandom, distanceRandom), Random.Range(distanceVRandom, distanceRandom), Random.Range(-distanceRandom, distanceRandom));
            allPlanets[i].transform.position = allPos[i];
        }
    }
}
