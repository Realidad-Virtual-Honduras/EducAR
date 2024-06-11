using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(SwipeDetection))]
public class SwipeInput : MonoBehaviour
{
    private SwipeDetection swipeDetection;

    [SerializeField] private UnityEvent swipeUpEvent;
    [SerializeField] private UnityEvent swipeDownEvent;
    [SerializeField] private UnityEvent swipeLeftEvent;
    [SerializeField] private UnityEvent swipeRightEvent;
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
        swipeUpEvent.Invoke();
    }

    private void OnDownSwipe()
    {
        swipeDownEvent.Invoke();
    }

    private void OnLeftSwipe()
    {
        swipeLeftEvent.Invoke();
    }

    private void OnRightSwipe()
    {
        swipeRightEvent.Invoke();
    }
}
