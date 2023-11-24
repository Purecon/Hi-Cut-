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
    public bool isTutorial = false;

    //Event
    public static event Action<GameObject> OnSliced;

    private void Start()
    {
        //Tweening
        Vector3 originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        float scaleSpeed = 0.25f;
        transform.DOScale(originalScale, scaleSpeed);
    }

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

            SoundManager.Instance.PlaySound("QuizRight",0.6f);
        }
    }

    public void WrongSlice()
    {
        if (isSlicable)
        {
            unslicedSprite.color = new Color(0.75f,0,0,0.5f);
            //Tweening
            Vector3 originalScale = transform.localScale;
            unslicedSprite.transform.DOShakeRotation(0.75f,new Vector3(0,0,30f),5);
            float scaleSpeed = 0.5f;
            var sequence = DOTween.Sequence()
                .Append(transform.DOScale(new Vector3(originalScale.x + 0.5f, originalScale.y + 0.5f, originalScale.z + 0.5f), scaleSpeed))
                .Append(transform.DOScale(originalScale, scaleSpeed));
            //sequence.SetLoops(-1, LoopType.Restart);

            isSlicable = false;

            SoundManager.Instance.PlaySound("QuizWrong", 0.5f);
        }
    }
}
