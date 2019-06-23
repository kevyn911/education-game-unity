using System;
using System.Collections;
using System.Collections.Generic;
using Application;
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
    //[SerializeField] private GameObject panPrefab;
    [Range(0f, 20f)]
    [SerializeField] private float magnetSpeed;
    [Range(0f, 5f)]
    [SerializeField] private float scaleOffset;
    [Range(0f, 20f)]
    [SerializeField] private float scaleSpeed;

    [SerializeField] private ScrollRect scrollRect;

    private List<GameObject> scenePanels;
    private Vector2[] pansPos,pansScale;
    private Vector2 contentVector;
    private RectTransform contentRect;
    private int selectedPanelId, t=0;
    private bool isScrolling;
    private float  panPosY,panLocPosY, panSize;
    private GameObject panPrefab;

    public GameObject PanPrefab
    {
        get => panPrefab;
        set => panPrefab = value;
    }
    public int SelectedPanelId => selectedPanelId;
    
    private void Awake()
    {
        ApplicationManager.Instance.ScrollScript = this;
        DeactivateScript();
    }

    private void Start()
    {
        contentRect = GetComponent<RectTransform>();
    }

    public void StartLevel1(List<Sprite> panSprite)
    {
        scenePanels = new List<GameObject>();
        panSize = panPrefab.GetComponent<RectTransform>().sizeDelta.x;
        panCount = panSprite.Count;
        pansScale = new Vector2[panCount];
        pansPos = new Vector2[panCount];
        for (int i = 0; i < panCount; i++)
        {
            panPrefab.transform.Find("ImageC").GetComponent<Image>().sprite = panSprite[i];
            InstPanel(panPrefab,i);
        }
    }
    
    public void StartLevel2(List<String> panText)
    {
        scenePanels = new List<GameObject>();
        panSize = panPrefab.GetComponent<RectTransform>().sizeDelta.x;
        panCount = panText.Count;
        pansScale = new Vector2[panCount];
        pansPos = new Vector2[panCount];
        for (int i = 0; i < panCount; i++)
        {
            panPrefab.transform.Find("Text").GetComponent<Text>().text = panText[i];
            InstPanel(panPrefab,i);
        }
    }

    private void InstPanel(GameObject panel, int i)
    {
        scenePanels.Add(Instantiate(panel, transform, false));
        if (i == 0)
        {
            panLocPosY = scenePanels[0].transform.localPosition.y;
            panPosY = scenePanels[0].transform.position.y;
        }
        else
        {
            scenePanels[i].transform.localPosition = new Vector2(
                scenePanels[i - 1].transform.localPosition.x + panSize + offset, panLocPosY);
            pansPos[i] = -scenePanels[i].transform.localPosition;
        }
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
        Destroy(scenePanels[selPanID]);
        scenePanels.RemoveAt(selPanID);
        panCount--;
    }

    public void DefaultPosPan(int selPanID)
    {
        scenePanels[selPanID].transform.localPosition = new Vector2(scenePanels[selPanID].transform.localPosition.x, panLocPosY);
    }
}
