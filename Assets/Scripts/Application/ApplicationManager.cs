using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application
{
    public class ApplicationManager : MonoBehaviour
    {
        public static ApplicationManager Instance { get; private set; }
        public GameManager GameManager { get; set; }
        public ScrollScript ScrollScript { get; set; }
        public SwipeInput SwipeInput { get; set; }
        
        private int sceneNum = 0;
        [SerializeField] private int sceneCount;

        public int SceneCount => sceneCount;

        public int SceneNum => sceneNum;

        public string GameScene
        {
            get
            {
                sceneNum++;
                PlayerPrefs.SetInt("SceneNum", sceneNum-1);
                return "GameScene" + sceneNum;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
            PlayerPrefs.SetInt("SceneNum", 0);
            if (PlayerPrefs.GetInt("SceneNum") >= 0)
                sceneNum = PlayerPrefs.GetInt("SceneNum");
        }

        private void Start()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
