using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using MEC;
using DG;

[RequireComponent(typeof(ContentSizeFitter))]
[RequireComponent(typeof(HorizontalLayoutGroup))]

public class SwipeMenu : MonoBehaviour
{
    public static SwipeMenu instance;    

    public bool canBeUse;

    [Header("Necesary Elements")]
    [SerializeField][Tooltip("Para saber en que dicreccion se mueve (Horizontal)")] private Scrollbar scrollbar = null;

    [Header("Min N Max Size")]
    [SerializeField] private float minSize = 0f;
    [SerializeField] private float maxSize = 0f;

    [Header("Buttons")]
    [SerializeField] private GameObject[] changingButtons = null;

    [Header(("Scroll"))] 
    public float scrollPos = 0f;
    [SerializeField] private float[] currentPos;

    [Header(("Events"))]
    [SerializeField] private UnityEvent touchHold;

    private int swapButton = 0;

    private ContentSizeFitter contentSizeFitter;
    private HorizontalLayoutGroup horizontalLayoutGroup;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        contentSizeFitter = GetComponent<ContentSizeFitter>();
        horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        swipeDetection = FindAnyObjectByType<SwipeDetection>();
    }

    private void Start()
    {
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

        horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
    }

    // Update is called once per frame
    void Update()
    {
        if (canBeUse)
        {
            currentPos = new float[transform.childCount];

            for (int i = 0; i < currentPos.Length; i++)
            {
                if (!transform.GetChild(i).gameObject.activeSelf)
                    currentPos = new float[i];
            }

            float distance = 1f / (currentPos.Length - 1f);
            for (int i = 0; i < currentPos.Length; i++)
                currentPos[i] = distance * i;

            //if (Input.touchCount > 0)
            if (Input.GetMouseButton(0))
            {
                //Touch touch = Input.GetTouch(0);

                //if(touch.phase == TouchPhase.Moved)
                    scrollPos = scrollbar.value;
            }
            else
            {
                for (int i = 0; i < currentPos.Length; i++)
                {
                    if (scrollPos < currentPos[i] + (distance / 2) && scrollPos > currentPos[i] - (distance / 2))
                        scrollbar.value = Mathf.Lerp(scrollbar.value, currentPos[i], 0.1f);
                }
            }

            //Scale Button
            for (int i = 0; i < currentPos.Length; i++)
            {
                if (scrollPos < currentPos[i] + (distance / 2) && scrollPos > currentPos[i] - (distance / 2))
                {
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(maxSize, maxSize), 0.1f);
                    transform.GetChild(i).GetComponent<Button>().interactable = true;
                    scrollPos = currentPos[i];
                    swapButton = i;

                    for (int j = 0; j < currentPos.Length; j++)
                    {
                        if (j != i)
                        {
                            transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(minSize, minSize), 0.1f);
                            transform.GetChild(j).GetComponent<Button>().interactable = false;
                        }
                    }
                }
            }
        }

        if (swapButton <= 0)
            changingButtons[0].SetActive(false);
        else
            changingButtons[0].SetActive(true);

        if (swapButton >= (currentPos.Length - 1))
            changingButtons[1].SetActive(false);
        else
            changingButtons[1].SetActive(true);
    }

    public void swapAddButon()
    {
        swapButton++;
        scrollPos = currentPos[swapButton];
    }

    public void swapLessButon()
    {
        swapButton--;
        scrollPos = currentPos[swapButton];
    }

    private SwipeDetection swipeDetection;
    [SerializeField] private bool active;

    private void OnEnable()
    {
        swipeDetection.onTapped += TapScreen;
    }

    private void OnDisable() 
    {
        swipeDetection.onTapped -= TapScreen;
    }

    public void TapScreen()
    {
        active = !active;
    }
}
