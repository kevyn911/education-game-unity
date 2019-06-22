using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    private static SwipeInput instance;
    public static SwipeInput Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SwipeInput>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned SwipeInput", typeof(SwipeInput)).GetComponent<SwipeInput>();
                }
            }

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    [Header("Tweaks")]
    [SerializeField] private float deadzone = 100.0f;

    [Header("Logic")]
    private bool swipeUp, swipeDown;
    private Vector2 swipeDelta, startTouch;

    #region Public properties
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    #endregion

    private void Update()
    {
        swipeUp = swipeDown = false;
        UpdateStandalone();
    }

    private void UpdateStandalone()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }
        
        swipeDelta = Vector2.zero;

        if (startTouch != Vector2.zero && Input.GetMouseButton(0))
            swipeDelta = (Vector2)Input.mousePosition - startTouch;
        
        if (swipeDelta.y > deadzone)
        {
            swipeDelta = (Vector2)Input.mousePosition - startTouch;
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) <= Mathf.Abs(y))
            {
                if (y < 0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;  
                }
            }
            startTouch = swipeDelta = Vector2.zero;
        }
    }
}