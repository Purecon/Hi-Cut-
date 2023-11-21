using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class SlicableObject : MonoBehaviour
{
    [Header("Object assets")]
    public GameObject unslicedObject;
    private GameObject slicedObject;
    [Header("Effect prefab")]
    public GameObject slicedEffect;
    public SpriteRenderer unslicedSprite;

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

    public void CollisionDetected()
    {
        OnSliced?.Invoke(gameObject);
    }

    public void Slice()
    {
        if (isSlicable)
        {
            //Debug.Log("Sliced!");
            //Event
            unslicedObject.SetActive(false);
            slicedObject.SetActive(true);

            //Effect
            //slicedEffect.transform.position = collision.transform.position;
            slicedEffect.SetActive(true);

            SoundManager.Instance.PlaySound("QuizRight",0.75f);
        }
    }

    public void WrongSlice()
    {
        if (isSlicable)
        {
            unslicedSprite.color = new Color(0.75f,0,0,0.5f);
            unslicedSprite.transform.DOShakeRotation(0.75f,new Vector3(0,0,30f),5);
            isSlicable = false;

            SoundManager.Instance.PlaySound("QuizWrong", 0.75f);
        }
    }
}
