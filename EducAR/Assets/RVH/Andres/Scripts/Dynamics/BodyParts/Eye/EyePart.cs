using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BodyPart_Eye
{OblicuoSuperior, OblicuoInferior, RectoSuperior, RectoInferior, RectoInterior, RectoExterior,
 IrisExterior, IrisInterior, Cornea, Coroides, Esclerotica, Venas, Cristalino, ArteriaDeRetina,
 NervioOptico, VasoSanguineoSuperior, VasoSanguineoInferior, VenasDeRetina, NervioSuperior, 
 NervioInferior, Nervio}
public class EyePart : MonoBehaviour
{
    [SerializeField] private Animation eyeAnim;
    [SerializeField] private string[] eyeAnimName;
    [SerializeField] private string[] eyeAnimAction;
    [SerializeField] private TextMeshProUGUI eyeActionText;
    [SerializeField] private Image btnBg;
    [SerializeField] private GameObject btnChange;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private int curEyeAnimName;
    [SerializeField] private int curEyeAnimAction;

    // Start is called before the first frame update
    void Awake()
    {
        ActiveButton(false);
        btnBg.color = UiManager.instance.darkTextClassColor[UiManager.instance.selectedIdx];
    }

    public void ActiveButton(bool active)
    {
       btnChange.SetActive(active);
    }

    public void ActiveAnimByName(string name)
    {
        eyeAnim.Play(name);
    }

    public void ChangeAnim()
    { 
        if(btnChange.gameObject.activeSelf)
        {
            curEyeAnimName = (curEyeAnimName + 1) % eyeAnimName.Length;
            curEyeAnimAction = (curEyeAnimAction + 1) % eyeAnimAction.Length;

            eyeAnim.Play(eyeAnimName[curEyeAnimName]);
            eyeActionText.text = eyeAnimAction[curEyeAnimAction];
        }
    }
}
