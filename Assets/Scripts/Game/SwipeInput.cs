using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;

public class SwipeInput : MonoBehaviour
{
    
    private Vector2 swipeDelta, startTouch;
    private int selPanID;
    private float swipeZone = 300f;
    
    public Vector2 SwipeDelta
    {
        get => swipeDelta;
        set => swipeDelta = value;
    }

    public Vector2 StartTouch
    {
        get => startTouch;
        set => startTouch = value;
    }
    
    private void Awake()
    {
        ApplicationManager.Instance.SwipeInput = this;
        DeactivateScript();
    }
    
    public void ActivateScript()
    {
        GetComponent<SwipeInput>().enabled = true;
    }
    
    public void DeactivateScript()
    {
        GetComponent<SwipeInput>().enabled = false;
    }
    private void Update()
    {
        UpdateStandalone();
    }
    

    private void UpdateStandalone()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            selPanID = ApplicationManager.Instance.ScrollScript.SelectedPanelId;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(swipeDelta.y > swipeZone)
                ApplicationManager.Instance.GameManager.OnSwipeUpDown(selPanID, true);
            else if (swipeDelta.y < -swipeZone)
                ApplicationManager.Instance.GameManager.OnSwipeUpDown(selPanID, false);
            else
                ApplicationManager.Instance.ScrollScript.OnSwipeUpDown(0f, selPanID);
            startTouch = swipeDelta = Vector2.zero;
        }

        if (startTouch != Vector2.zero && Input.GetMouseButton(0))
        {
            swipeDelta = (Vector2) Input.mousePosition - startTouch;
            ApplicationManager.Instance.ScrollScript.OnSwipeUpDown(swipeDelta.y * 2f, selPanID);
        }
    }
}