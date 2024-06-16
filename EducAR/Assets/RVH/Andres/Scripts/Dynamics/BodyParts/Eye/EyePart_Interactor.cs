using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyePart_Interactor : MonoBehaviour
{
    public BodyPart_Eye bodyPartEye;
    public UiLookAt lookAt;

    public void CheckCorrect()
    {
        for (int i = 0; i < BodySelector.instance.finalPos.Length; i++)
        {
            if (bodyPartEye.ToString() == BodySelector.instance.nameSelected)
            {
                BodySelector.instance.finalPos[i].GetComponent<EyePart_Detector>().CorrectDetection(gameObject);                
            }
            else
                BodySelector.instance.finalPos[i].GetComponent<EyePart_Detector>().IncorrectDetection(gameObject);
        }
    }
}
