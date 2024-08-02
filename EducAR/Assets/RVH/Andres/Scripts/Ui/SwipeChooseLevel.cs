using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeChooseLevel : MonoBehaviour
{
    [Header("Elementos necesarios")]
    [SerializeField] [Tooltip("Para saber cuando se debe activar")] private bool canBeUse;
    [SerializeField] [Tooltip("Para saber en que dicreccion se mueve (Horizontal)")] private Scrollbar scrollbar = null;
    
    [Header("Tamaño maximo y minimo de los botones")]
    [SerializeField] private float maxSize = 0f;
    [SerializeField] private float minSize = 0f;

    [Header("Posiciones actuales del selector")]
    [SerializeField] private float scrollPos = 0f;
    [SerializeField] private float[] currentPos;

    [Header("Posiciones actuales del selector")]
    [SerializeField] private int swapButton = 0;
    [SerializeField] private GameObject[] changingButtons = null;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(canBeUse)
        {
            currentPos = new float[transform.childCount];
            float distance = 1f / (currentPos.Length - 1f);
            for (int i = 0; i < currentPos.Length; i++)
                currentPos[i] = distance * i;

            if (Input.GetMouseButton(0))
            {
                scrollPos = scrollbar.value;
            }
            else
            {
                for (int i = 0; i < currentPos.Length; i++)
                {
                    if (scrollPos < currentPos[i] + (distance / 2) && scrollPos > currentPos[i] - (distance / 2))
                        scrollbar.value = Mathf.Lerp(scrollbar.value, currentPos[i], 0.1f);
                }
            }

            //Scale Button
            for (int i = 0; i < currentPos.Length; i++)
            {
                if (scrollPos < currentPos[i] + (distance / 2) && scrollPos > currentPos[i] - (distance / 2))
                {
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(maxSize, maxSize), 0.1f);
                    transform.GetChild(i).GetComponent<Button>().interactable = true;
                    scrollPos = currentPos[i];
                    swapButton = i;                    
                    //Debug.Log("Current Selected Level" + i);
                    //Debug.Log(transform.GetChild(i).GetComponent<Button>());

                    for (int j = 0; j < currentPos.Length; j++)
                    {
                        if (j != i)
                        {
                            transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(minSize, minSize), 0.1f);
                            transform.GetChild(j).GetComponent<Button>().interactable = false;
                        }
                    }
                }
            }
        }

        if (swapButton <= 0)
            changingButtons[0].SetActive(false);
        else
            changingButtons[0].SetActive(true);

        if (swapButton >= (currentPos.Length - 1))
            changingButtons[1].SetActive(false);
        else
            changingButtons[1].SetActive(true);
    }

    public void swapAddButon()
    {
        swapButton++;
        scrollPos = currentPos[swapButton];
    }

    public void swapLessButon()
    {
        swapButton--;
        scrollPos = currentPos[swapButton];
    }
}
