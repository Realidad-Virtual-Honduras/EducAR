using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;

public class EyePart_Detector : MonoBehaviour
{
    public static EyePart_Detector instance;

    public BodyPart_Eye bodyPartEye;

    private BodySelector bodySelector;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        bodySelector = FindObjectOfType<BodySelector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if(obj.GetComponent<EyePart_Interactor>() != null)
        {
            if (BodySelector.instance.nameSelected == obj.GetComponent<EyePart_Interactor>().bodyPartEye.ToString())
                Timing.RunCoroutine(IsCorrect(obj));
            else
                Timing.RunCoroutine(IsNotCorrect(obj));
        }
    }

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
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        obj.GetComponent<BoxCollider>().enabled = false;

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale;

        yield return Timing.WaitForSeconds(0.3f);

        gameObject.GetComponent<BoxCollider>().enabled = false;

        bodySelector.placedObjectEvent.Invoke();
        TouchMananger.instance.UnSelectAll();
    }

    private IEnumerator<float> IsNotCorrect(GameObject obj)
    {
        BodySelector.instance.bodyMaterialSelected.color = Color.red;

        yield return Timing.WaitForSeconds(1f);
        BodySelector.instance.bodyMaterialSelected.color = Color.white;

        obj.transform.position = obj.GetComponent<BodyPartInfo>().startPos;
        TouchMananger.instance.UnSelectAll();
    }
}
