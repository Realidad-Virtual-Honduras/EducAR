using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using TMPro;

public class UiLookAt : MonoBehaviour
{
    public static UiLookAt instance;
    [SerializeField] private Camera _camera;
    [SerializeField] private TextMeshProUGUI titleName;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject ui;
    private float time;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        _camera = Camera.main;

        _canvas.worldCamera = _camera;

        titleName.text = gameObject.GetComponentInParent<ObjectSelector>().name;
    }

    private void Start()
    {
        startUi(false);
    }

    /*IEnumerator<float> StartLook(float seconds)
    {
        while (time <= 999999999)
        {
            yield return Timing.WaitForSeconds(seconds);
            time += seconds;
        }
    }*/

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.back, _camera.transform.rotation * Vector3.up);        
    }

    public void startUi(bool active)
    {
        ui.SetActive(active);
    }
}
