using System.Collections.Generic;
using UnityEngine;

public class RaycastHitObjects : MonoBehaviour
{
    public static RaycastHitObjects instance;

    [SerializeField] private float distance;
    [SerializeField] private ObjectSelector objectSelector;
    public ObjectSelector[] objectSelectors;

    private void Awake()
    {
        if(instance == null) 
            instance = this;
    }

    void Update()
    {
        HitObject();
    }

    public void HitObject()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward, out hit, distance))
        {
            objectSelector = hit.transform.GetComponent<ObjectSelector>();
            Debug.DrawRay(transform.position, transform.forward * distance, Color.green);

            if (objectSelector != null)
                objectSelector.OnSelectObject(true);

        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * distance, Color.red);
            objectSelector = null;

            for (int i = 0; i < objectSelectors.Length; i++)
            {
                objectSelectors[i].OnSelectObject(false);
            }
        }
    }
}
