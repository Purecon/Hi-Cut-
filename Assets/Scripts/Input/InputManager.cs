using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

//Run before other
[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    private TouchControls touchControls;

    public Camera mainCamera;

    //make event
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    private void Awake()
    {
        touchControls = new TouchControls();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        touchControls.Enable();
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += FingerUp;
    }

    private void OnDisable()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= FingerUp;

        touchControls.Disable();
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }

    private void Start()
    {
        //Input System
        //touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        //touchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch started " + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        if(OnStartTouch != null)
        {
            OnStartTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch ended " + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        if (OnEndTouch != null)
        {
            OnEndTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
        }
    }

    private void FingerDown(Finger finger)
    {
        Debug.Log("(Enhanced)Touch started " + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        if (OnStartTouch != null)
        {
            OnStartTouch(finger.screenPosition, Time.time);
        }
    }

    private void FingerUp(Finger finger)
    {
        Debug.Log("(Enhanced)Touch ended " + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        if (OnEndTouch != null)
        {
            OnEndTouch(finger.screenPosition, Time.time);
        }
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, touchControls.Touch.TouchPosition.ReadValue<Vector2>());
    }

    private void Update()
    {
        //Debug.Log(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches);
        //foreach(UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        //{
        //    Debug.Log(touch.phase == UnityEngine.InputSystem.TouchPhase.Began);
        //}
    }
}
