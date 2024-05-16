using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DarkModeManager : MonoBehaviour
{
    [SerializeField] private Toggle activeDarkMode;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI[] allTexts;
    [SerializeField] private TextMeshProUGUI[] titleText;

    [Header("Ui")]
    [SerializeField] private Image[] logo;
    [SerializeField] private Image[] allBG;
    [SerializeField] private Image[] allScrollView;
    [SerializeField] private Image[] darkModeImg;

    [Header("Colors")]
    [SerializeField] private Color[] colorTextNLogo;
    [SerializeField] private Color[] colorTitleText;
    [SerializeField] private Color[] colorScrollView;

    // Start is called before the first frame update
    void Start()
    {
        ChangeMode();
    }

    private void ActiveLightMode()
    {
        //Texts
        for (int i = 0; i < allTexts.Length; i++)
            allTexts[i].color = colorTextNLogo[0];

        for (int i = 0; i < titleText.Length; i++)
            titleText[i].color = colorTitleText[0];

        //Logo
        for (int i = 0; i < logo.Length; i++)
            logo[i].color = colorTextNLogo[0];

        //BGs
        for (int i = 0; i < allBG.Length; i++)
            allBG[i].color = colorTextNLogo[1];

        for (int i = 0; i < allScrollView.Length; i++)
            allScrollView[i].color = colorScrollView[0];

        //Ui
        for (int i = 0; i < darkModeImg.Length; i++)
            darkModeImg[i].color = colorTitleText[0];
    }

    private void ActiveDarkMode()
    {
        //Texts
        for (int i = 0; i < allTexts.Length; i++)
            allTexts[i].color = colorTextNLogo[1];

        for (int i = 0; i < titleText.Length; i++)
            titleText[i].color = colorTitleText[1];

        //Logo
        for (int i = 0; i < logo.Length; i++)
            logo[i].color = colorTextNLogo[1];

        //BGs
        for (int i = 0; i < allBG.Length; i++)
            allBG[i].color = colorTextNLogo[0];

        for (int i = 0; i < allScrollView.Length; i++)
            allScrollView[i].color = colorScrollView[1];

        //Ui
        for (int i = 0; i < darkModeImg.Length; i++)
            darkModeImg[i].color = colorTitleText[1];
    }

    public void ChangeMode()
    {
        if (activeDarkMode.isOn)
            ActiveDarkMode();
        else
            ActiveLightMode();
    }
}
