using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class UiMediator : MonoBehaviour
{
    public static UiMediator instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #region Mediator
    [Header("Mediator System")]
    [SerializeField] private Mediator[] mediators;
    private int currentIdx = 0;
    private int lastIdx = 0;
    private bool activeSwitch = false;
    #endregion

    #region Getter
    #region Menus Ui Panels
    public Mediator[] Medators
    {
        get { return mediators; }
    }
    #endregion
    #endregion

    #region Change Ui
    public void GoToUi(int  idx)
    {
        ChangeUiPanel(idx);
    }

    private void ChangeUiPanel(int idx)
    {
        currentIdx = idx;

        mediators[lastIdx].HideUi();
        mediators[currentIdx].ShowUi();

        lastIdx = currentIdx;
    }

    public void ShowPanel(int idx)
    {
        mediators[idx].ShowUi();
    }

    public void HidePanel(int idx) 
    {
        mediators[idx].HideUi();
    }

    public void CanSwitchSamePanel(int idx)
    {
        activeSwitch = !activeSwitch;

        if (activeSwitch)
            mediators[idx].ShowUi();
        else
            mediators[idx].HideUi();
    }
    #endregion
}
