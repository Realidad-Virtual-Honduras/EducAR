using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MEC;
using UnityEngine.XR.ARFoundation;
using Unity.Mathematics;

public class LevelManager : MonoBehaviour
{
    #region Scripts
    public static LevelManager instance;
    private UiManager uiManager;
    #endregion

    [Header("Ui")]
    public GameObject selectedObjectInfo;
    public Image selectedObjectBg;
    public TextMeshProUGUI selectedObjectTitle;
    public TextMeshProUGUI selectedObjectDescription;
    [Space]
    public GameObject timerObj;
    public TextMeshProUGUI timerText;
    [Space]
    public GameObject scanFloorInstruction;

    [Header("Time")]
    [SerializeField, Range(0,1)] private float timeToWait;
    [SerializeField] private float timer;

    [Header("Instuctions")]
    public string instruccion;

    [Header("Events")]
    public UnityEvent winEvents;
    public UnityEvent loseEvents;

    [Header("FX")]
    public Material selectedMat;

    [HideInInspector] public float curTime;
    [HideInInspector] public bool canInteract;
    [HideInInspector] public bool isComplete;

    void Awake()
    {
        if(instance == null)
            instance = this;

        uiManager = FindAnyObjectByType<UiManager>();
        uiManager.instructionsText.text = instruccion;
        selectedObjectBg.color = uiManager.lightClassColor[uiManager.selectedIdx];
        selectedObjectTitle.color = uiManager.darkTextClassColor[uiManager.selectedIdx];

        ScanObject(true);
        timerText.text = "";

        ChangeColor(Color.white);
    }

    private void Start()
    {
        selectedObjectTitle.color = uiManager.darkTextClassColor[uiManager.selectedIdx];
    }

    #region Timer
    public void StarTimer()
    {
        canInteract = true;
        timerObj.SetActive(canInteract);
        Timing.RunCoroutine(TimerGlobal(timer), "Timer");
    }

    private IEnumerator<float> TimerGlobal(float gTimer)
    {
        while (gTimer >= 0 && !isComplete)
        {
            UpdateTimer(gTimer);
            yield return Timing.WaitForSeconds(timeToWait);
            gTimer -= 1f;
            curTime = gTimer;
        }

        LoseGame();
    }

    private void UpdateTimer(float uTimer)
    {
        float minutes = Mathf.FloorToInt(uTimer / 60);
        float seconds = Mathf.FloorToInt(uTimer % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion

    private void EndGame()
    {
        canInteract = false;
        timerObj.SetActive(canInteract);
        GameManager.instance.ActiveContinue(true);
    }

    public void ScanObject(bool active)
    {
        timerObj.SetActive(false);
        scanFloorInstruction.SetActive(active);
    }

    public void SelectedObjectInfoShow(bool active, string title, string description)
    {
        selectedObjectInfo.SetActive(active);
        selectedObjectTitle.text = title;
        selectedObjectDescription.text = description;
    }

    #region Win And Lose
    public void WinGame()
    {
        EndGame();
        Timing.PauseCoroutines("Timer");
        UpdateTimer(curTime);
        winEvents.Invoke();
    }

    public void LoseGame()
    {
        EndGame();
        loseEvents.Invoke();
    }
    #endregion

    #region ActiveMaterial
    public void AlphaOnMat()
    {
        selectedMat.SetFloat("_Alpha", 1);
    }

    public void AlphaOffMat()
    {
        selectedMat.SetFloat("_Alpha", 0);
    }

    public void AlphaInMat(float alpha)
    {
        selectedMat.SetFloat("_AlphaCliping", alpha);
    }

    public void ChangeColor(Color color)
    {
        selectedMat.SetColor("_OutlineColor", color);
    }
    #endregion
}
