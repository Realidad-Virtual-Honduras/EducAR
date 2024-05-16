using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Mediator_PanelUi : Mediator
{
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        if (!Scritable.isStartHidden)
            ShowUi();
        else
            HideUi();
    }

    public override void ShowUi()
    {
        if (HasAnimationsType(TypeAnimation.Fade))
            Timing.RunCoroutine(FadeIn());

        if (HasAnimationsType(TypeAnimation.Move))
            Timing.RunCoroutine(MoveIn());

        if (HasAnimationsType(TypeAnimation.Scale))
            Timing.RunCoroutine(ScaleIn());

        if (HasAnimationsType(TypeAnimation.Rotation))
            Timing.RunCoroutine(RotationIn());
    }

    public override void HideUi()
    {
        if (HasAnimationsType(TypeAnimation.Fade))
            Timing.RunCoroutine(FadeOut());

        if (HasAnimationsType(TypeAnimation.Move))
            Timing.RunCoroutine(MoveOut());

        if (HasAnimationsType(TypeAnimation.Scale))
            Timing.RunCoroutine(ScaleOut());

        if (HasAnimationsType(TypeAnimation.Rotation))
            Timing.RunCoroutine(RotationOut());
    }
}
