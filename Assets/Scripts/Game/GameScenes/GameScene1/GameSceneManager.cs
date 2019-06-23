using Application;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Game.GameScenes
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private Rules rules;
        [SerializeField] private Image room;
        [SerializeField] private Text scoreText;
        
        public Rules Rules => rules;

        public Image Room
        {
            get => room;
            set => room = value;
        }

        public Text ScoreText
        {
            get => scoreText;
            set => scoreText = value;
        }

        private void Start()
        {
            SceneManager.SetActiveScene(gameObject.scene);
            ApplicationManager.Instance.GameManager.RegisterGameSceneManager(this);
        }
    }
}
