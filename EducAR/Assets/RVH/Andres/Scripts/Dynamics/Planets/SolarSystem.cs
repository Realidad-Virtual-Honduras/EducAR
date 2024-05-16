using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public static SolarSystem instance;
    [SerializeField] private GameObject[] allPlanets;
    [SerializeField] private ObjectSelector[] allObjects;
    [SerializeField] private float distanceRandom;
    [SerializeField] private float distanceVRandom;

    void Awake()
    {        
        if(instance == null)
            instance = this;

        RaycastHitObjects.instance.objectSelectors = new ObjectSelector[allObjects.Length];
    }

    public void Shuffle()
    {
        for (int i = 0; i < allPlanets.Length; i++)
        {
            allPlanets[i].transform.position = new Vector3(Random.Range(-distanceRandom, distanceRandom), Random.Range(distanceVRandom, distanceRandom), Random.Range(-distanceRandom, distanceRandom));
        }

        for (int i = 0; i < allObjects.Length; i++)
        {
            RaycastHitObjects.instance.objectSelectors[i] = allObjects[i];
        }
    }
}
