using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using MEC;

[Flags] public enum TypeAnimation { None = 0, Fade = 1, Move = 2, Scale = 4, Rotation = 8, }
[RequireComponent(typeof(CanvasGroup))]
public abstract class Mediator : MonoBehaviour
{
    #region Variables
    #region Hide in inspector
    [HideInInspector] public CanvasGroup canvasGroup;
    [HideInInspector] public RectTransform rectTransform;
    [HideInInspector] public Vector3 startPosUi;
    #endregion

    #region Menu Information   
    [Header("Menu information")]
    [SerializeField] private Mediator_Scritable _Scritable;

    [Header("Objects")]
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    #endregion
    #endregion

    #region Getters
    #region Menu Information
    public Mediator_Scritable Scritable
    {
        get { return _Scritable; }
    }
    #endregion
    #endregion

    public bool HasAnimationsType(TypeAnimation typeAnimations)
    {
       // return AnimationType == TypeAnimation.Fade ? false : (typeAnimation & typeAnimations) != 0;
       return (_Scritable.typeAnimation & typeAnimations) != 0;
    }

    public abstract void ShowUi();

    public abstract void HideUi();

    #region Animations
    #region Fade
    //Animation In
    public IEnumerator<float> FadeIn()
    {
        yield return Timing.WaitForSeconds(_Scritable.waitTime);

        canvasGroup.DOFade(1f, _Scritable.animationTime);

        canvasGroup.interactable = _Scritable.isInteractable;
        canvasGroup.blocksRaycasts = _Scritable.isBlocksRaycasts;
    }

    //Animation Out
    public IEnumerator<float> FadeOut()
    {
        yield return Timing.WaitForSeconds(_Scritable.waitTime);

        canvasGroup.DOFade(0f, _Scritable.animationTime);

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    #endregion

    #region Move
    //Animation In
    public IEnumerator<float> MoveIn()
    {
        foreach (GameObject item in items)
        {
            rectTransform.transform.localPosition = startPosUi;
            rectTransform.DOAnchorPos(_Scritable.posUiIn, _Scritable.animationTime, false).SetEase(Ease.OutElastic);
            yield return Timing.WaitForSeconds(_Scritable.waitTime);
        }

        canvasGroup.interactable = _Scritable.isInteractable;
        canvasGroup.blocksRaycasts = _Scritable.isBlocksRaycasts;
    }

    //Animation Out
    public IEnumerator<float> MoveOut()
    {
        foreach (GameObject item in items)
        {
            rectTransform.transform.localPosition = startPosUi;
            rectTransform.DOAnchorPos(_Scritable.posUiOut, _Scritable.animationTime, false).SetEase(Ease.OutElastic);
            yield return Timing.WaitForSeconds(_Scritable.waitTime);
        }

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    #endregion

    #region Scale
    // Animation In
    public IEnumerator<float> ScaleIn()
    {
        foreach (GameObject item in items)
        {
            item.transform.localScale = Vector3.zero;
            item.transform.DOScale(1f, _Scritable.animationTime).SetEase(Ease.OutBounce);
            yield return Timing.WaitForSeconds(_Scritable.waitTime);
        }

        canvasGroup.interactable = _Scritable.isInteractable;
        canvasGroup.blocksRaycasts = _Scritable.isBlocksRaycasts;
    }

    // Animation Out
    public IEnumerator<float> ScaleOut()
    {
        foreach (GameObject item in items)
        {
            item.transform.localScale = Vector3.zero;
            item.transform.DOScale(0f, _Scritable.animationTime).SetEase(Ease.OutBounce);
            yield return Timing.WaitForSeconds(_Scritable.waitTime);
        }

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    #endregion

    #region Rotation
    //Rotation In
    public IEnumerator<float> RotationIn()
    {
        foreach(GameObject item in items)
        {
            item.transform.localRotation = Quaternion.identity;
            item.transform.DORotate(new Vector3(_Scritable.rotationUiIn.x, 0f, _Scritable.rotationUiIn.y), _Scritable.animationTime).SetEase(Ease.OutBounce);
            yield return Timing.WaitForSeconds(_Scritable.waitTime);
        }

        canvasGroup.interactable = _Scritable.isInteractable;
        canvasGroup.blocksRaycasts = _Scritable.isBlocksRaycasts;
    }

    //Rotation Out
    public IEnumerator<float> RotationOut()
    {
        foreach (GameObject item in items)
        {
            item.transform.localRotation = Quaternion.identity;
            item.transform.DORotate(new Vector3(_Scritable.rotationUiOut.x, 0f, _Scritable.rotationUiOut.y), _Scritable.animationTime).SetEase(Ease.OutBounce);
            yield return Timing.WaitForSeconds(_Scritable.waitTime);
        }

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    #endregion
    #endregion
}
