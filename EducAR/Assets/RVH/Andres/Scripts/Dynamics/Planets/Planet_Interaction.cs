using MEC;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Planet_Interaction : MonoBehaviour
{
    public PlanetType planetType;
    [Space]
    [SerializeField] private GameObject planetTemplate;
    [SerializeField] private Transform parentRotation;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float timeToWait;
    private float time;
    [Space]
    public bool hasTheRightPlanet;
    private Planet planet;
    [SerializeField] private string planetName;
    private void Awake()
    {
        time = 0;
        planet = FindObjectOfType<Planet>();
        planetName = planetType.ToString();

        RotatePlanet();
    }

    private void Start()
    {
        Timing.PauseCoroutines("Rotation:" + planetType);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Planet_Mover>() != null)
        {
            if (planetType == obj.GetComponent<Planet_Mover>().planetType)
            {                
                Timing.RunCoroutine(SamePlanet(obj));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Planet_Mover>() != null)
        {
            planetTemplate.SetActive(true);
        }
    }

    public void RotatePlanet()
    {
        Timing.RunCoroutine(StartRotation(timeToWait), "Rotation:" + planetType);
    }

    private IEnumerator<float> StartRotation(float seconds)
    {
        while(time <= 999999)
        {
            yield return Timing.WaitForSeconds(seconds);
            time += seconds;
            parentRotation.eulerAngles = new Vector3(0, rotationSpeed * time, 0);
        }
    }

    private IEnumerator<float> SamePlanet(GameObject obj)
    {
        //obj.transform.SetParent(null);
        obj.transform.SetParent(transform);
        planetTemplate.SetActive(false);

        obj.GetComponent<SphereCollider>().enabled = false;

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;

        Timing.ResumeCoroutines("Rotation:" + planetType);
        planet.RemoveElement(planetName);

        yield return Timing.WaitForSeconds(0.3f);

        obj.GetComponentInChildren<UiLookAt>().startUi(true);

        gameObject.GetComponent<SphereCollider>().enabled = false;


        planet.onCorrect.Invoke();
    }
}
