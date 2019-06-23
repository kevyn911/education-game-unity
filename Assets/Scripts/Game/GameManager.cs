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
        public GameSceneManager GameSceneManager { get; set; }
        private GameSceneManager currentGameSceneManager;
        private Rules rules;
        private List<Thing> things,thingsSort;
        private List<Sprite> panSprite;
        private List<String> panText;
        private List<bool> selectThing;
        private bool level1Bool = true;
        private int sizeThings, rand, score, maxScore, scoreLevel;
        private uint sceneCount, curSceneNumber;

        private void Awake()
        {
            ApplicationManager.Instance.GameManager = this;
        }

        private void Start()
        {
            LoadScene();
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
            if(!rules)
                rules = currentGameSceneManager.Rules;
            StartCoroutine(ViewRoom());
        }

        private IEnumerator ViewRoom()
        {
            currentGameSceneManager.Room.sprite = rules.Room;
            currentGameSceneManager.Room.gameObject.SetActive(true);
            yield return new WaitForSeconds(rules.Timer);
            Load();
            currentGameSceneManager.Room.gameObject.SetActive(false);
            yield return null;
        }
        
        private void Load()
        {
            thingsSort = new List<Thing>(rules.Things);
            things = new List<Thing>();
            selectThing = new List<bool>();
            sizeThings = thingsSort.Count;
            scoreLevel = 0;
            maxScore += sizeThings;
            
            if (level1Bool)
            {
                LoadPanelImage();
                level1Bool = false;
            }
            else
            {
                LoadPanelText();
                level1Bool = true;
            }
            ApplicationManager.Instance.ScrollScript.ActivateScript();
            ApplicationManager.Instance.SwipeInput.ActivateScript();
        }

        private void LoadPanelImage()
        {
            panSprite = new List<Sprite>();
            for (int i = 0; i < sizeThings; i++)
            {
                rand = Random.Range(0, thingsSort.Count);
                panSprite.Add(thingsSort[rand].sprite);
                things.Add(thingsSort[rand]);
                selectThing.Add(false);
                thingsSort.RemoveAt(rand);
            }
            
            ApplicationManager.Instance.ScrollScript.PanPrefab = rules.PanelImage;
            ApplicationManager.Instance.ScrollScript.StartLevel1(panSprite);
        }
        
        private void LoadPanelText()
        {
            panText = new List<string>();
            for (int i = 0; i < sizeThings; i++)
            {
                rand = Random.Range(0, thingsSort.Count);
                panText.Add(thingsSort[rand].thingName);
                things.Add(thingsSort[rand]);
                selectThing.Add(false);
                thingsSort.RemoveAt(rand);
            }
            
            ApplicationManager.Instance.ScrollScript.PanPrefab = rules.PanelText;
            ApplicationManager.Instance.ScrollScript.StartLevel2(panText);
        }

        public void OnSwipeUp(int selPanID)
        {
            if (things[selPanID].correct)
            {
                ApplicationManager.Instance.ScrollScript.DelPan(selPanID);
                scoreLevel++;
                things.RemoveAt(selPanID);
                selectThing.RemoveAt(selPanID);
            }
            else
            {
                ApplicationManager.Instance.ScrollScript.DefaultPosPan(selPanID);
                Debug.Log(selectThing[selPanID]);
                if (!selectThing[selPanID])
                {
                    selectThing[selPanID] = true;
                    scoreLevel--;
                }
            }

            if (things.Count == 0)
                StartCoroutine(ViewScore());
        }
        
        public void OnSwipeDown(int selPanID)
        {
            if (!things[selPanID].correct)
            {
                ApplicationManager.Instance.ScrollScript.DelPan(selPanID);
                scoreLevel++;
                things.RemoveAt(selPanID);
                selectThing.RemoveAt(selPanID);
            }
            else
            {
                ApplicationManager.Instance.ScrollScript.DefaultPosPan(selPanID);
                Debug.Log(selectThing[selPanID]);
                if (!selectThing[selPanID])
                {
                    selectThing[selPanID] = true;
                    scoreLevel--;
                }
            }

            if (things.Count == 0)
                StartCoroutine(ViewScore());
        }

        private IEnumerator ViewScore()
        {
            ApplicationManager.Instance.ScrollScript.DeactivateScript();
            ApplicationManager.Instance.SwipeInput.DeactivateScript();
            score += scoreLevel;
            if(!level1Bool ||  curSceneNumber < sceneCount)
                ScoreText(scoreLevel, sizeThings);
            else if (level1Bool && curSceneNumber == sceneCount)
            {
                ScoreText(score, maxScore);
                yield break;
            }
            yield return new WaitForSeconds(5);
            currentGameSceneManager.ScoreText.gameObject.SetActive(false);
            StartCoroutine(ViewRoom());
            yield return null;
        }

        private void ScoreText(int score, int maxScore)
        {
            currentGameSceneManager.ScoreText.text = score + "/" + maxScore;
            currentGameSceneManager.ScoreText.gameObject.SetActive(true);
        }
    }
}
