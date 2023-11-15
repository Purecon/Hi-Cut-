using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public SlicableObject slicable;
    public GameObject sliced;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Came from " + transform.name);
            if (slicable.GetSlicedObject() == null)
            {
                slicable.SetSlicedObject(sliced);
                slicable.Slice();
            }
        }
    }
}
