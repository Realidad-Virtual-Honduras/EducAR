using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;

public class EyePart_Detector : MonoBehaviour
{
    public static EyePart_Detector instance;

    public BodyPart_Eye bodyPartEye;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if(obj.GetComponent<EyePart_Interactor>() != null)
        {
            if (BodySelector.instance.nameSelected == obj.GetComponent<EyePart_Interactor>().bodyPartEye.ToString())
            {
                Timing.RunCoroutine(IsCorrect(obj));
                BodySelector.instance.placedObjectEvent.Invoke();
            }
            else
                Timing.RunCoroutine(IsNotCorrect(obj));   
            
            if (bodyPartEye == obj.GetComponent<EyePart_Interactor>().bodyPartEye)
            {
            }
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;
        if(obj.GetComponent<EyePart_Interactor>() != null)
        {
            if (bodyPartEye == obj.GetComponent<EyePart_Interactor>().bodyPartEye)
            {
                if (BodySelector.instance.nameSelected == bodyPartEye.ToString())
                {
                    BodySelector.instance.Checkcolision.text = "<color=green>" + gameObject.name + "</color>";
                    Timing.RunCoroutine(IsCorrect(obj));
                }

                BodySelector.instance.Checkcolision.text = "<color=#0153ff>" + gameObject.name + "</color>";
            }
            else
            {
                BodySelector.instance.Checkcolision.text = "<color=red>" + gameObject.name + "</color>";
            }
        }
    }*/

    private void OnTriggerExit(Collider other) 
    {
        GameObject obj = other.gameObject;
        if(obj.GetComponent<EyePart_Interactor>() != null)
        {
            BodySelector.instance.bodyMaterialSelected.color = Color.white;
        }
    }

    private IEnumerator<float> IsCorrect(GameObject obj)
    {

        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale;

        obj.GetComponent<BoxCollider>().enabled = false;

        obj.GetComponent<ObjectSelector>().OnSelectObject();
        obj.GetComponent<BodyPartInfo>().ShowInfo();

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;        

        TouchMananger.instance.UnSelectAll();

        yield return Timing.WaitForSeconds(0.5f);
    }

    private IEnumerator<float> IsNotCorrect(GameObject obj)
    {
        BodySelector.instance.bodyMaterialSelected.color = Color.red;
        //obj.GetComponent<ObjectSelector>().OnSelectObject();
        obj.GetComponent<BodyPartInfo>().ShowInfo();

        yield return Timing.WaitForSeconds(1f);
        BodySelector.instance.bodyMaterialSelected.color = Color.white;

        obj.transform.position = obj.GetComponent<BodyPartInfo>().startPos;
        TouchMananger.instance.UnSelectAll();
    }
}
