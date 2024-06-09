using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    #region events
    private delegate void StartTouch(Vector2 pos, float time);
    private event StartTouch onStartTouch;
    
    private delegate void EndTouch(Vector2 pos, float time);
    private event EndTouch onEndTouch;

    public delegate void Tapped();
    public event Tapped onTapped;
    
    public delegate void MultiTapped();
    public event MultiTapped onMultiTapped;

    public delegate void LeftSwipe();
    public event LeftSwipe onLeftSwipe;

    public delegate void RightSwipe();
    public event RightSwipe onRightSwipe;

    public delegate void UpSwipe();
    public event UpSwipe onUpSwipe;

    public delegate void DownSwipe();
    public event DownSwipe onDownSwipe;
    #endregion

    [SerializeField] private float minimumDistance = 15f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = 0.9f;

    private Vector2 startPosition, endPosition;
    private float startTime, endTime;

    private TouchControls inputActions;

    void Awake() => inputActions = new TouchControls();
    private void Start()
    {
        inputActions.Gesture.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        inputActions.Gesture.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

        inputActions.Gesture.Tap.performed += ctx => TappedPerformed(ctx);
        inputActions.Gesture.MultiTap.performed += ctx => MultiTappedPerformed(ctx);
    }

    #region Enable/Disable
    private void OnEnable()
    {
        inputActions.Enable();
        onStartTouch += StartSwipe;
        onEndTouch += EndSwipe;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        onStartTouch -= StartSwipe;
        onEndTouch -= EndSwipe;
    }
    #endregion

    #region Start Actions
    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (onStartTouch != null)
            onStartTouch(ScreenPosition(), (float)ctx.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (onEndTouch != null)
            onEndTouch(ScreenPosition(), (float)ctx.time);
    }

    private Vector2 ScreenPosition()
    {
        return inputActions.Gesture.PrimaryPosition.ReadValue<Vector2>();
    }

    private void TappedPerformed(InputAction.CallbackContext ctx)
    {
        if (onTapped != null)
            onTapped();
    }

    private void MultiTappedPerformed(InputAction.CallbackContext ctx)
    {
        if (onMultiTapped != null)
            onMultiTapped();
    }
    #endregion

    private void StartSwipe(Vector2 pos, float time)
    {
        startPosition = pos;
        startTime = time;
    }

    private void EndSwipe(Vector2 pos, float time)
    {
        endPosition = pos;
        endTime = time;

        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if(Vector3.Distance(startPosition, endPosition) >= minimumDistance && (endTime - startTime) < maximumTime)
        {
            Vector3 dir = endPosition - startPosition;
            Vector2 dir2D = new Vector2(dir.x, dir.y).normalized;

            SwipeDirection(dir2D);
        }
    }

    private void SwipeDirection(Vector2 dir)
    {
        if (Vector2.Dot(Vector2.up, dir) > directionThreshold)
        {
            if(onUpSwipe!= null) 
                onUpSwipe();
        }
        else if (Vector2.Dot(Vector2.down, dir) > directionThreshold)
        {
            if (onDownSwipe != null) 
                onDownSwipe();
        }
        else if (Vector2.Dot(Vector2.left, dir) > directionThreshold)
        { 
            if (onLeftSwipe != null)
                onLeftSwipe();
        }
        else if (Vector2.Dot(Vector2.right, dir) > directionThreshold)
        {
            if (onRightSwipe != null)
                onRightSwipe();
        }
    }
}
