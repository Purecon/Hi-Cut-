using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlicableObject : MonoBehaviour
{
    [Header("Object assets")]
    public GameObject unslicedObject;
    private GameObject slicedObject;
    [Header("Effect prefab")]
    public GameObject slicedEffect;

    public bool isSlicable = true;

    //Event
    public static event Action<GameObject> OnSliced;

    public void SetSlicedObject(GameObject slicedObject)
    {
        this.slicedObject = slicedObject;
    }

    public GameObject GetSlicedObject()
    {
        return slicedObject;
    }

    public void Slice(Collider2D collision)
    {
        if (isSlicable)
        {
            //Debug.Log("Sliced!");
            //Event
            OnSliced?.Invoke(gameObject);
            unslicedObject.SetActive(false);
            slicedObject.SetActive(true);

            //Effect
            //slicedEffect.transform.position = collision.transform.position;
            slicedEffect.SetActive(true);
        }
    }
}
