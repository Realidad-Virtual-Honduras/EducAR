using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleElements : MonoBehaviour
{
    public static ShuffleElements instance;

    [Header("Objects To Shuffle")]
    [SerializeField] private GameObject[] objects;

    [Header("Shuffle Spects")]
    [SerializeField] private Transform[] pos;
    [SerializeField, Range(0, 1)] private float shuffleTime = 0.3f;
    [SerializeField] private float distanceHRandom = 0.7f;
    [SerializeField] private float distanceVRandom = 0.5f;

    [Header("Usage")]
    [SerializeField] private bool useRandomPos;
    [SerializeField] private bool useParent;
    [SerializeField] private bool useRotation;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float timeToWaitRotation = 0.02f;
    private float time;

    private Vector3[] allPos;
    private List<Transform> isTaken;

    void Awake()
    {
        if (instance == null)
            instance = this;

        allPos = new Vector3[pos.Length];
        isTaken = new List<Transform>(pos);
        time = 0;
    }

    private void Start()
    {
        if(useRandomPos)
        {
            for (int i = 0; i < allPos.Length; i++)
            {
                allPos[i] = new Vector3(Random.Range(-distanceHRandom, distanceHRandom), distanceVRandom, Random.Range(-distanceHRandom, distanceHRandom));
                pos[i].position = allPos[i];
            }
        }            
    }

    public void Shuffle()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }

        if (useRandomPos)
        {
            for (int i = 0; i < pos.Length; i++)
            {
                //pos[i].position = allPos[i];
                Debug.Log(GetComponentInChildren<Transform>().position);
            }
        }

        if(useRotation)
            Timing.RunCoroutine(StartRotation(timeToWaitRotation));

        Timing.RunCoroutine(ShuffleAll());
    }

    private IEnumerator<float> ShuffleAll()
    {
        foreach (GameObject objs in objects)
        {
            int randomIdx = Random.Range(0, isTaken.Count);
            Transform randomPos = isTaken[randomIdx];

            if(useParent)
                objs.transform.SetParent(randomPos);

            objs.transform.position = randomPos.position;
            objs.transform.rotation = randomPos.rotation;

            isTaken.RemoveAt(randomIdx);
        }

        yield return Timing.WaitForSeconds(shuffleTime);

        for (int i = 0; i < pos.Length; i++)
        {
            if (useParent)
                objects[i].gameObject.transform.SetParent(null);

            objects[i].SetActive(true);
        }
    }

    private IEnumerator<float> StartRotation(float seconds)
    {
        while (time <= 999999)
        {
            yield return Timing.WaitForSeconds(seconds);
            time += seconds;
            gameObject.transform.eulerAngles = new Vector3(0, rotationSpeed * time, 0);
        }
    }
}
