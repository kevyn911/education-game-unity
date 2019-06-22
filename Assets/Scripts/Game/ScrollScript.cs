using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollScript : MonoBehaviour
{
    [Range(1,50)]
    [Header("Controllers")]
    [SerializeField] private int panCount;
    [Range(1, 500)]
    [SerializeField] private int offset;
    [Header("GameObjects")] 
    [SerializeField] private GameObject panPrefab;
    [Range(0f, 20f)]
    [SerializeField] private float magnetSpeed;
    [Range(0f, 5f)]
    [SerializeField] private float scaleOffset;
    [Range(0f, 20f)]
    [SerializeField] private float scaleSpeed;

    [SerializeField] private ScrollRect scrollRect;

    private GameObject[] scenePanels;
    private Vector2[] pansPos;
    private Vector2[] pansScale;
    private Vector2 contentVector;
    private RectTransform contentRect;
    private int selectedPanelId;
    private bool isScrolling;
    
    void Start()
    {
        contentRect = GetComponent<RectTransform>();
        scenePanels = new GameObject[panCount];
        pansScale = new Vector2[panCount];
        pansPos = new Vector2[panCount];
        for (int i = 0; i < panCount; i++)
        {
            scenePanels[i] = Instantiate(panPrefab, transform, false);
            if(i==0) continue;
            scenePanels[i].transform.localPosition = new Vector2(
                scenePanels[i - 1].transform.localPosition.x + panPrefab.GetComponent<RectTransform>().sizeDelta.x +
                offset, scenePanels[i].transform.localPosition.y);
            pansPos[i] = -scenePanels[i].transform.localPosition;
        }
    }

    private void FixedUpdate()
    {
        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= pansPos[pansPos.Length - 1].x && !isScrolling)
        {
            scrollRect.inertia = false;
        }
        float nearestPos = float.MaxValue;
        float scale;
        for (int i = 0; i < panCount; i++)
        {
            float distance = Math.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanelId = i;
            }

            scale = Mathf.Clamp(1 / (distance / offset) * scaleOffset, 0.5f, 1f);
            pansScale[i].x = Mathf.SmoothStep(scenePanels[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            pansScale[i].y = Mathf.SmoothStep(scenePanels[i].transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime);
            scenePanels[i].transform.localScale = pansScale[i];
        }

        float scrollVelocity = Math.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;
        if (isScrolling || scrollVelocity > 400) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanelId].x, magnetSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;

        
    }
    
    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }

    private void OnSwipeUp()
    {
        Debug.Log("Is swiped up");
    }
}
