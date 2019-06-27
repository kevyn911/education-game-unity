using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Application;
using Game.GameScenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private GameSceneManager currentGameSceneManager;
        private Rules rules;
        private List<Thing> things;
        private Image loadingBar;
        private Transform activeBar;
        private int sizeThings, rand, score, maxScore, scoreLevel, curSceneNumber, sceneCount, timer;
        private ScrollScript scrollScript;
        private SwipeInput swipeScript;
        private Panel panelImage;
        
        [SerializeField]private AudioClip correctAnswer, inCorrectAnswer;
        [SerializeField] private Image room;
        [SerializeField] private Text scoreText, finishScoreText;
        [SerializeField] private Button startGameButton, restartGameButton;
        [SerializeField] private Text gameTitle;
        [SerializeField] private GameObject timerObj;
        [SerializeField] private Text timerText;
        [SerializeField] private Animator animatorController, finishGameAnimator;
        [SerializeField] private AudioSource soundAnswer;
        
        private void Awake()
        {
            ApplicationManager.Instance.GameManager = this;
        }

        private void Start()
        {
            startGameButton.onClick.AddListener(StartButtonClicked);
            restartGameButton.onClick.AddListener(OnRestart);
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

        private void OnRestart()
        {
            SceneManager.LoadScene("Game");
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
            //panelText = rules.PanelText;
            StartCoroutine(ViewRoom());
        }

        private IEnumerator StartTimer()
        {
            timer = (int)rules.Timer;
            loadingBar.fillAmount = 1;
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
            LoadPanels();
            room.gameObject.SetActive(false);
            yield return null;
        }
        
        private void LoadPanels()
        {
            scrollScript.InitLists();
            things = new List<Thing>(rules.Things);
            things = rules.Things.Shuffle().ToList();
            sizeThings = things.Count;
            maxScore += sizeThings;
            scoreLevel = 0;

            foreach (var thing in things)
            {
                scrollScript.InstPanel(panelImage, thing);
            }
            
            scrollScript.ActivateScript();
            swipeScript.ActivateScript(); 
            animatorController.SetBool("isStartSwipeHelp", true);
        }

        public void OnSwipeUpDown(int selPanID, bool swipeUpBool)
        {
            if (scrollScript.ScenePanels[selPanID].Thing.correct == swipeUpBool)
                TrueSwipe(selPanID);
            else
                FalseSwipe(selPanID);
        }

        private void TrueSwipe(int selPanID)
        {
            scrollScript.DelPan(selPanID);
            soundAnswer.clip = correctAnswer;
            soundAnswer.Play();
            scoreLevel++;
            if (scrollScript.ScenePanels.Count == 0)
                StartCoroutine(ViewScore());
        }

        private void FalseSwipe(int selPanID)
        {
            scrollScript.DefaultPosPan(selPanID);
            if (scrollScript.ScenePanels[selPanID].Thing.SelBool) return;

            soundAnswer.clip = inCorrectAnswer;
            soundAnswer.Play();
            Handheld.Vibrate();
            scrollScript.ScenePanels[selPanID].Thing.SelBool = true;
            scoreLevel--;
        }

        private IEnumerator ViewScore()
        {
            scrollScript.DeactivateScript();
            swipeScript.DeactivateScript();
            score += scoreLevel;
            if(curSceneNumber < sceneCount)
                ScoreText(scoreLevel, sizeThings);
            else if (curSceneNumber == sceneCount)
            {
                ShowFinishScore(score, maxScore);
                yield break;
            }
            yield return new WaitForSeconds(3);
            scoreText.gameObject.SetActive(false);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                LoadScene();
                yield return null;
        }

        private void ShowFinishScore(int score, int maxScore)
        {
            finishScoreText.text = score + "/" + maxScore;
            finishGameAnimator.gameObject.SetActive(true);
            finishGameAnimator.SetBool("isStartFinishPanelAnim", true);
        }
        private void ScoreText(int score, int maxScore)
        {
            scoreText.text = score + "/" + maxScore;
            scoreText.gameObject.SetActive(true);
        }
    }
}
