using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    private InputManager inputManager;
    private Camera cameraMain;

    public float speed = 10.0f;
    public Vector2 targetPosition;
    public bool noLimit = true;
    public float minX = -15f;
    public float maxX = 15f;
    public float minY = -9f;
    public float maxY = 3f;

    //Swipe
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = .9f;
    public bool useTrail = true;
    [SerializeField]
    private GameObject trail;

    private Coroutine coroutine;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        cameraMain = Camera.main;
        targetPosition = transform.position;
    }

    private void OnEnable()
    {
        //Simple movement
        //inputManager.OnStartTouch += Move;

        //Swipe
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        //Simple movement
        //inputManager.OnStartTouch -= Move;

        //Swipe
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void Move(Vector2 screenPosition, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, cameraMain.nearClipPlane);
        Vector3 worldCoordinates = cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;
        //transform.position = worldCoordinates;
        targetPosition = worldCoordinates;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = Utils.ScreenToWorld(cameraMain, position);
        startTime = time;
        if (useTrail)
        {
            trail.SetActive(true);
            trail.transform.position = startPosition;
        }
        coroutine = StartCoroutine(Trail());
    }

    private IEnumerator Trail()
    {
        while (true)
        {
            Vector2 inputPosition = inputManager.PrimaryPosition();
            
            //Not update the position if above limit
            if((inputPosition.x <= maxX && inputPosition.x >= minX && inputPosition.y <= maxY && inputPosition.y >= minY)||noLimit)
            {
                if (useTrail)
                {
                    transform.position = targetPosition;
                    trail.transform.position = inputPosition;
                }
                
                targetPosition = inputPosition;
            }

            /*
            Vector2 direction = targetPosition - startPosition;
            if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
            */

            yield return null;
        }
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        //trail.SetActive(false);
        StopCoroutine(coroutine);

        endPosition = Utils.ScreenToWorld(cameraMain, position);
        endTime = time;
        //DetectSwipe();

        Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
        Vector3 direction = endPosition - startPosition;
        Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
        SwipeDirection(direction2D);
    }

    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up,direction) > directionThreshold)
        {
            Debug.Log("Swipe Up");
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe Down");
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe Left");
            //spriteRenderer.flipX = false;
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe Right");
            //spriteRenderer.flipX = true;
        }
    }

    private void Update()
    {
        //float step = speed * Time.deltaTime;
        //transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
        
        //transform.position = targetPosition;
    }
}
