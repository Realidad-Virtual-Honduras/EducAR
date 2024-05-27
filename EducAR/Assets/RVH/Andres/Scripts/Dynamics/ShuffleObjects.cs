using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;

public class ShuffleObjects : MonoBehaviour
{
    public static ShuffleObjects instance;

    [Header("Objects To Shuffle")]
    [SerializeField] private GameObject[] objects;

    [Header("Shuffle Spects")]
    [SerializeField] private Transform[] pos;
    [SerializeField, Range(0,2)] private float shuffleTime;
    [SerializeField] private float distanceHRandom;
    [SerializeField] private float distanceVRandom;
    [SerializeField] private bool useRandomPos;

    private Vector3[] allPos;
    private List<Transform> isTaken;

    void Awake()
    {
        if(instance == null)
            instance = this;

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }

        allPos = new Vector3[objects.Length];
        isTaken = new List<Transform>(pos);

        if (useRandomPos)
        {
            for (int i = 0; i < allPos.Length; i++)
            {
                //objects[i].SetActive(false);
                allPos[i] = new Vector3(Random.Range(-distanceHRandom, distanceHRandom), Random.Range(distanceVRandom, distanceHRandom), Random.Range(-distanceHRandom, distanceHRandom));
                pos[i].position = allPos[i];
            }
        }
        //Shuffle();
    }

    public void Shuffle()
    {
        Timing.RunCoroutine(ShuffleAll());
    }

    private IEnumerator<float> ShuffleAll()
    {
        foreach (GameObject objs in objects)
        {
            int randomIdx = Random.Range(0, isTaken.Count);
            Transform randomPos = isTaken[randomIdx];

            objs.transform.SetParent(randomPos);
            objs.transform.position = randomPos.position;
            objs.transform.rotation = randomPos.rotation;

            isTaken.RemoveAt(randomIdx);
        }

        yield return Timing.WaitForSeconds(shuffleTime);

        for(int i = 0; i < pos.Length;i++)
        {
            objects[i].gameObject.transform.SetParent(null);
            objects[i].SetActive(true);
        }
    }
}
