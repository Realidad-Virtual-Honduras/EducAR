using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using DG.Tweening;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private float timer = 1f;

    private MeshRenderer spawnMaterial;
    void Awake()
    {
        spawnMaterial = GetComponent<MeshRenderer>();

        spawnMaterial.material = new Material(spawnMaterial.material);
        spawnMaterial.material.SetFloat("_Alpha_Cliping", 1f);
    }

    private void Start()
    {        
        Timing.RunCoroutine(ActiveCreation(timer));
    }

    private IEnumerator<float> ActiveCreation(float timer)
    {
        spawnMaterial.material.DOFloat(0, "_Alpha_Cliping", timer);
        yield break;
    }
}
