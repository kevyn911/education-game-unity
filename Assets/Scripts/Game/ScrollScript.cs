using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Application;
using Game.GameScenes;
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
    [Range(0f, 20f)]
    [SerializeField] private float magnetSpeed;
    [Range(0f, 5f)]
    [SerializeField] private float scaleOffset;
    [Range(0f, 20f)]
    [SerializeField] private float scaleSpeed;

    [SerializeField] private ScrollRect scrollRect;

    private List<Panel> scenePanels = new List<Panel>();
    private List<Vector2> pansPos;
    private Vector2 contentVector;
    private RectTransform contentRect;
    private int selectedPanelId, t=0;
    private bool isScrolling;
    private float panPosY,panLocPosY, panSize;
    public int SelectedPanelId => selectedPanelId;

    public List<Panel> ScenePanels
    {
        get => scenePanels;
        set => scenePanels = value;
    }

    private void Awake()
    {
        ApplicationManager.Instance.ScrollScript = this;
        DeactivateScript();
    }

    private void Start()
    {
        contentRect = GetComponent<RectTransform>();
    }

    public void InitLists()
    {
        pansPos = new List<Vector2>();
    }

    public void InstPanel(Panel panel, Thing thing)
    {
        scenePanels.Add(Instantiate(panel, transform, false));
        scenePanels.Last().Show(thing);
        scenePanels.Last().Thing.SelBool = false;
        panCount = scenePanels.Count;
        if (panCount == 1)
        {
            panLocPosY = scenePanels[0].transform.localPosition.y;
            panPosY = scenePanels[0].transform.position.y;
            panSize = scenePanels[0].GetComponent<RectTransform>().sizeDelta.x;
        }
        else
        {
            scenePanels[panCount - 1].transform.localPosition = new Vector2(
                scenePanels[panCount - 2].transform.localPosition.x + panSize + offset, panLocPosY);
        }
        pansPos.Add(-scenePanels.Last().transform.localPosition);
    }

    public void ActivateScript()
    {
        GetComponent<ScrollScript>().enabled = true;
    }
    
    public void DeactivateScript()
    {
        GetComponent<ScrollScript>().enabled = false;
    }

    private void FixedUpdate()
    {
        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= pansPos.Last().x && !isScrolling)
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
            scenePanels[i].transform.localScale = new Vector2(
                Mathf.SmoothStep(scenePanels[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime),
                Mathf.SmoothStep(scenePanels[i].transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime));
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

    public void OnSwipeUpDown(float swipeDeltaY, int selPanId)
    {
        if (selPanId != selectedPanelId)
        {
            scenePanels[selPanId].transform.position = new Vector2(scenePanels[selPanId].transform.position.x, panPosY);
            ApplicationManager.Instance.SwipeInput.StartTouch = ApplicationManager.Instance.SwipeInput.SwipeDelta = Vector2.zero;
            swipeDeltaY = 0f;
        }
        scenePanels[selectedPanelId].transform.position = new Vector2(scenePanels[selectedPanelId].transform.position.x, panPosY + swipeDeltaY);
    }

    public void DelPan(int selPanID)
    {
        for (int i = selPanID + 1; i < panCount; i++)
        {
            scenePanels[i].transform.localPosition = new Vector2(
                scenePanels[i].transform.localPosition.x - panSize - offset, panLocPosY);
        }
        Destroy(scenePanels[selPanID].gameObject);
        scenePanels.RemoveAt(selPanID);
        panCount--;
    }

    public void DefaultPosPan(int selPanID)
    {
        scenePanels[selPanID].transform.localPosition = new Vector2(scenePanels[selPanID].transform.localPosition.x, panLocPosY);
    }
}
