using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Application;
using Game.GameScenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private GameSceneManager currentGameSceneManager;
        private Rules rules;
        private List<Thing> things;
        private bool level1Bool = true;
        private Image loadingBar;
        private Transform activeBar;
        private int sizeThings, rand, score, maxScore, scoreLevel, curSceneNumber, sceneCount, timer;
        private ScrollScript scrollScript;
        private SwipeInput swipeScript;
        private Panel panelImage, panelText;

        [SerializeField] private Image room;
        [SerializeField] private Text scoreText;
        [SerializeField] private Button startGameButton;
        [SerializeField] private Text gameTitle;
        [SerializeField] private GameObject timerObj;
        [SerializeField] private Text timerText;
        [SerializeField] private GameObject gg;
        [SerializeField] private Animator animatorController;
        

        private void Awake()
        {
            ApplicationManager.Instance.GameManager = this;
        }

        private void Start()
        {
            startGameButton.onClick.AddListener(StartButtonClicked);
            activeBar = timerObj.transform.Find("ActiveBar");
            loadingBar = activeBar.GetComponent<Image>();
            scrollScript = ApplicationManager.Instance.ScrollScript;
            swipeScript = ApplicationManager.Instance.SwipeInput;
        }

        private void StartButtonClicked()
        {
            LoadScene();
            startGameButton.gameObject.SetActive(false);
            gameTitle.gameObject.SetActive(false);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(ApplicationManager.Instance.GameScene, LoadSceneMode.Additive);
            sceneCount = ApplicationManager.Instance.SceneCount;
            curSceneNumber = ApplicationManager.Instance.SceneNum;
        }

        public void RegisterGameSceneManager(GameSceneManager gameSceneManager)
        {
            currentGameSceneManager = gameSceneManager;
            StartGame();
        }
        
        private void StartGame()
        {
            rules = currentGameSceneManager.Rules;
            panelImage = rules.PanelImage;
            panelText = rules.PanelText;
            StartCoroutine(ViewRoom());
        }

        private IEnumerator StartTimer()
        {
            timer = (int)rules.Timer;
            timerObj.SetActive(true);
            while (0 < timer)
            {
                timerText.text = timer.ToString();
                yield return new WaitForSeconds(1);
                timer--;
                loadingBar.fillAmount = (float)timer / rules.Timer;
            }

            timerObj.SetActive(false);
            yield return null;
        }
        
        private IEnumerator ViewRoom()
        {
            room.sprite = rules.Room;
            room.gameObject.SetActive(true);
            StartCoroutine(StartTimer());
            yield return new WaitForSeconds(rules.Timer);
            Load();
            room.gameObject.SetActive(false);
            yield return null;
        }
        
        private void Load()
        {
            scrollScript.InitLists();
            things = new List<Thing>(rules.Things);
            things = rules.Things.Shuffle().ToList();
            sizeThings = things.Count;
            maxScore += sizeThings;
            scoreLevel = 0;
            
            foreach (var thing in things)
                scrollScript.InstPanel(level1Bool ? panelImage : panelText, thing);

            level1Bool = !level1Bool;
            scrollScript.ActivateScript();
            swipeScript.ActivateScript(); 
            animatorController.SetBool("isStartSwipeHelp", true);
        }


        public void OnSwipeUp(int selPanID)
        {
            if (scrollScript.ScenePanels[selPanID].Thing.correct)
                TrueSwipe(selPanID);
            else
                FalseSwipe(selPanID);
        }
        
        public void OnSwipeDown(int selPanID)
        {
            if (!scrollScript.ScenePanels[selPanID].Thing.correct)
                TrueSwipe(selPanID);
            else
                FalseSwipe(selPanID);
        }

        private void TrueSwipe(int selPanID)
        {
            scrollScript.DelPan(selPanID);
            scoreLevel++;
            if (scrollScript.ScenePanels.Count == 0)
                StartCoroutine(ViewScore());
        }

        private void FalseSwipe(int selPanID)
        {
            scrollScript.DefaultPosPan(selPanID);
            Debug.Log(scrollScript.ScenePanels[selPanID].Thing.SelBool);
            if (scrollScript.ScenePanels[selPanID].Thing.SelBool) return;
            
            scrollScript.ScenePanels[selPanID].Thing.SelBool = true;
            scoreLevel--;
        }

        private IEnumerator ViewScore()
        {
            scrollScript.DeactivateScript();
            swipeScript.DeactivateScript();
            score += scoreLevel;
            if(curSceneNumber < sceneCount || !level1Bool)
                ScoreText(scoreLevel, sizeThings);
            else if (level1Bool && curSceneNumber == sceneCount)
            {
                ScoreText(score, maxScore);
                yield break;
            }
            yield return new WaitForSeconds(3);
            scoreText.gameObject.SetActive(false);
            if (level1Bool)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                LoadScene();
            }
            else
                StartCoroutine(ViewRoom());
            yield return null;
        }

        private void ScoreText(int score, int maxScore)
        {
            scoreText.text = score + "/" + maxScore;
            scoreText.gameObject.SetActive(true);
        }
    }
}
