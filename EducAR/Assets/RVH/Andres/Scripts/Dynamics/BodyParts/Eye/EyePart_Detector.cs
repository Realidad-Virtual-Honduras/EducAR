using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;
using Lean.Touch;

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

    public void CorrectDetection(GameObject obj)
    {
        Timing.RunCoroutine(IsCorrect(obj));
    }

    public void IncorrectDetection(GameObject obj)
    {
        Timing.RunCoroutine(IsNotCorrect(obj));
    }

    private IEnumerator<float> IsCorrect(GameObject obj)
    {
        LevelManager.instance.ChangeColor(Color.green);
        LevelManager.instance.ShowQuestion(false);

        obj.GetComponent<BoxCollider>().enabled = false;
        obj.GetComponent<LeanPinchScale>().enabled = true;

        LevelManager.instance.AlphaInMat(1.1f);
        bodySelector.OnCorrect();

        Timing.PauseCoroutines("Timer");

        yield return Timing.WaitForSeconds(10f);


        Timing.ResumeCoroutines("Timer");

        obj.GetComponent<LeanPinchScale>().enabled = false;

        LevelManager.instance.AlphaInMat(0f);

        LevelManager.instance.ChangeColor(Color.white);

        obj.GetComponent<EyePart_Interactor>().lookAt.startUi(true);

        for (int i = 0; i < bodySelector.finalPos.Length; i++)
        {
            if (obj.GetComponent<EyePart_Interactor>().bodyPartEye == bodyPartEye)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;

                obj.transform.SetParent(transform);

                obj.transform.position = Vector3.Lerp(obj.transform.position, transform.position, 10f * Time.fixedDeltaTime);
                //obj.transform.position = transform.position;
                obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, transform.rotation, 10f * Time.fixedDeltaTime);
                //obj.transform.rotation = transform.rotation;
                obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, transform.localScale, 10f * Time.fixedDeltaTime);
                //obj.transform.localScale = transform.localScale;
            }
        }        

        bodySelector.GenerateQuestion();
        bodySelector.placedObjectEvent.Invoke();
    }



    private IEnumerator<float> IsNotCorrect(GameObject obj)
    {
        LevelManager.instance.ChangeColor(Color.red);
        obj.transform.position = obj.GetComponent<BodyPartInfo>().startPos;

        yield return Timing.WaitForSeconds(0.05f);

        LevelManager.instance.ChangeColor(Color.white);
        TouchMananger.instance.UnSelectAll();
    }
}
