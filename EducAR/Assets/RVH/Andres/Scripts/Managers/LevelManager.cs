using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MEC;
using Unity.Mathematics;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private UiManager uiManager;

    public TextMeshProUGUI selectedObject;
    public TextMeshProUGUI timerText;
    [Space]
    [SerializeField, Range(0,5)] private float curTimer;
    [SerializeField] private float timer;
    [SerializeField] private float Ctimer;
    public string instruccion;

    [HideInInspector] public bool canInteract;
    [HideInInspector] public bool isComplete;

    void Awake()
    {
        if(instance == null)
            instance = this;

        uiManager = FindAnyObjectByType<UiManager>();
        uiManager.instructionsText.text = instruccion;

        UpdateTimer(timer);
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
            yield return Timing.WaitForSeconds(curTimer);
            gTimer -= 1f;
            Ctimer = gTimer;
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
}
