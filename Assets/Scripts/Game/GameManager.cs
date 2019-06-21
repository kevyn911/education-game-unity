using System;
using System.Collections;
using System.Collections.Generic;
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
        private List<Thing> things;
        private GameObject content;
        
        [Range(1,50)]
        [Header("Controllers")]
        [SerializeField] private int panCount;
        [Header("GameObjects")]
        [SerializeField] private GameObject panPrefab;
        
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
            yield return new WaitForSeconds(rules.Timer);
            LoadThings();
            currentGameSceneManager.Room.gameObject.SetActive(false);
            yield return null;
        }

        // Тестовий вивід картинок.
        private void LoadThings()
        {
            //Image[] thingsArr;
//            int sizeThings = currentGameSceneManager.Things.Length, rand;
//            
//            currentGameSceneManager.Canvas.gameObject.SetActive(true);
//            things = new List<Thing>(rules.Things);
//
//            for (int i = 0; i < sizeThings; i++)
//            {
//                rand = Random.Range(0, things.Count);
//                currentGameSceneManager.Things[i].sprite = things[rand].sprite;
//                things.RemoveAt(rand);
//            }
            
        }
        
        
    }
}
