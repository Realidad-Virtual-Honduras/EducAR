using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MEC;

public class TapSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coordinatesText;
    [SerializeField] private Vector2 screenTouch;
    [SerializeField] private Vector3 screenTouchV3;

    [SerializeField] private Camera mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        screenTouchV3 = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawRay(screenTouchV3, mainCamera.transform.forward*1000, Color.magenta);

        coordinatesText.text = screenTouchV3.ToString();
    }
}
