using MEC;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Planet_Interaction : MonoBehaviour
{
    [SerializeField] Planet_Sctiptable planet_Sctiptable;
    public Planet_Sctiptable Planet_Sctiptable
    {
        get { return planet_Sctiptable; }
    }
    [Space]
    [SerializeField] private GameObject planetTemplate;
    [SerializeField] private Transform parentRotation;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float timeToWait;
    private float time;
    [Space]
    public bool hasTheRightPlanet;

    private void Awake()
    {
        time = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Planet_Mover>() != null)
        {
            if(!hasTheRightPlanet)
            {
                if (obj.GetComponent<Planet_Mover>().Planet_Sctiptable.planet == planet_Sctiptable.planet)
                {                    
                    planetTemplate.GetComponent<MeshRenderer>().enabled = (false);
                    hasTheRightPlanet = true;

                    obj.transform.position = transform.position;
                    obj.transform.rotation = transform.rotation;

                    TouchMananger.instance.UnSelectAll(); 
                    Planet.instance.CheckActives();

                    obj.transform.SetParent(planetTemplate.gameObject.transform);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Planet_Mover>() != null)
        {
            if (hasTheRightPlanet)
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Planet_Mover>() != null)
        {
            //planetTemplate.SetActive(true);

            if (obj.GetComponent<Planet_Mover>().Planet_Sctiptable.planet == planet_Sctiptable.planet)
            {
                hasTheRightPlanet = false;
                Planet.instance.CheckActives();
            }
        }
    }

    public void RotatePlanet()
    {
        Timing.RunCoroutine(StartRotation(timeToWait));
    }

    private IEnumerator<float> StartRotation(float seconds)
    {
        while(time <= 99999)
        {
            yield return Timing.WaitForSeconds(seconds);
            time += seconds;
            parentRotation.eulerAngles = new Vector3(0, rotationSpeed * time, 0);
        }
    }
}
