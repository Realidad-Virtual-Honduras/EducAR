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

        ScanObject(true);
        timerText.text = "";
    }

    private void Start()
    {
    }

    #region Timer
    public void StarTimer()
    {
        canInteract = true;
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
        GameManager.instance.ActiveContinue(true);
    }

    public void ScanObject(bool active)
    {
        scanFloorInstruction.SetActive(active);
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

    public void ChangeColor(Color color)
    {
        selectedMat.SetColor("_OutlineColor", color);
    }
    #endregion
}
