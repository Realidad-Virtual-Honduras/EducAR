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
    public TextMeshProUGUI selectedObject;
    public TextMeshProUGUI timerText;
    public GameObject scanFloorInstruction;

    [Header("Time")]
    [SerializeField, Range(0,1)] private float timeToWait;
    [SerializeField] private float timer;
    private float curTime;

    [Header("Instuctions")]
    public string instruccion;

    [Header("Events")]
    public UnityEvent winEvents;
    public UnityEvent loseEvents;

    [SerializeField] private ARSession aRSession;
    [HideInInspector] public bool canInteract;
    [HideInInspector] public bool isComplete;

    void Awake()
    {
        if(instance == null)
            instance = this;

        uiManager = FindAnyObjectByType<UiManager>();
        uiManager.instructionsText.text = instruccion;

        aRSession = FindAnyObjectByType<ARSession>();
        //aRSession.Reset();

        ScanObject(true);
        timerText.text = "";
    }

    private void Start()
    {
        //aRSession.Reset();
    }

    #region Timer
    public void StarTimer()
    {
        canInteract = true;
        Timing.RunCoroutine(TimerGlobal(timer));
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

        EndGame();
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
    }

    public void ScanObject(bool active)
    {
        scanFloorInstruction.SetActive(active);
    }

    #region Win And Lose
    public void WinGame()
    {
        EndGame();
        Timing.PauseCoroutines();
        UpdateTimer(curTime);
        winEvents.Invoke();
    }

    public void LoseGame()
    {
        EndGame();
        loseEvents.Invoke();
    }
    #endregion
}
