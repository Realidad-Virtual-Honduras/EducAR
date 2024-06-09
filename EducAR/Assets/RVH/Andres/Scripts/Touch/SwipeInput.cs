using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwipeInput : MonoBehaviour
{
    private SwipeDetection swipeDetection;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Awake()
    {
        swipeDetection = GetComponent<SwipeDetection>();
    }

    private void OnEnable()
    {
        swipeDetection.onUpSwipe += OnUpSwipe;
        swipeDetection.onDownSwipe += OnDownSwipe;
        swipeDetection.onLeftSwipe += OnLeftSwipe;
        swipeDetection.onRightSwipe += OnRightSwipe;
    }

    private void OnDisable()
    {
        swipeDetection.onUpSwipe -= OnUpSwipe;
        swipeDetection.onDownSwipe -= OnDownSwipe;
        swipeDetection.onLeftSwipe -= OnLeftSwipe;
        swipeDetection.onRightSwipe -= OnRightSwipe;
    }

    private void OnUpSwipe()
    {
        textMeshProUGUI.text = "Swipe Up";
    }

    private void OnDownSwipe()
    {
        textMeshProUGUI.text = "Swipe Down";
    }

    private void OnLeftSwipe()
    {
        textMeshProUGUI.text = "Swipe Left";
    }

    private void OnRightSwipe()
    {
        textMeshProUGUI.text = "Swipe Right";
    }
}
