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
            LevelManager.instance.ChangeColor(Color.white);
        }
    }

    private IEnumerator<float> IsCorrect(GameObject obj)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        obj.GetComponent<BoxCollider>().enabled = false;

        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale;

        LevelManager.instance.ChangeColor(Color.green);

        yield return Timing.WaitForSeconds(0.7f);

        gameObject.GetComponent<BoxCollider>().enabled = false;
        LevelManager.instance.ChangeColor(Color.white);

        bodySelector.placedObjectEvent.Invoke();
    }

    private IEnumerator<float> IsNotCorrect(GameObject obj)
    {
        LevelManager.instance.ChangeColor(Color.red);

        yield return Timing.WaitForSeconds(1f);

        obj.transform.position = obj.GetComponent<BodyPartInfo>().startPos;
        LevelManager.instance.ChangeColor(Color.white);
        TouchMananger.instance.UnSelectAll();
    }
}
