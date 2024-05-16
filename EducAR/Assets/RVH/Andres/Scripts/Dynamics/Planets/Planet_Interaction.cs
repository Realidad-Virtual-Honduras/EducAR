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
    private float time;
    [Space]
    public bool hasTheRightPlanet;
    public bool isRotating;

    private void Awake()
    {
        StartRotation(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Planet_Mover>() != null)
        {
            if(!hasTheRightPlanet)
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                planetTemplate.SetActive(false);

                if (obj.GetComponent<Planet_Mover>().Planet_Sctiptable.planet == planet_Sctiptable.planet)
                {
                    Debug.Log("Son el mismo planeta: " + planet_Sctiptable.planet);
                    hasTheRightPlanet = true;
                    obj.GetComponent<SphereCollider>().enabled = false;
                    Planet.instance.CheckActives();
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
            planetTemplate.SetActive(true);

            if (obj.GetComponent<Planet_Mover>().Planet_Sctiptable.planet == planet_Sctiptable.planet)
            {
                hasTheRightPlanet = false;
                Planet.instance.CheckActives();
            }
        }
    }

    private void FixedUpdate()
    {
        if(isRotating) 
        {
            time = Time.fixedTime;
            parentRotation.eulerAngles = new Vector3(0, rotationSpeed * time, 0);
        }
    }

    public void StartRotation(bool active)
    {
        isRotating = active;
    }
}
