using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicableObject : MonoBehaviour
{
    public GameObject unslicedObject;
    private GameObject slicedObject;

    public void SetSlicedObject(GameObject slicedObject)
    {
        this.slicedObject = slicedObject;
    }

    public GameObject GetSlicedObject()
    {
        return slicedObject;
    }

    public void Slice()
    {
        Debug.Log("Sliced!");
        unslicedObject.SetActive(false);
        slicedObject.SetActive(true);
    }
}
