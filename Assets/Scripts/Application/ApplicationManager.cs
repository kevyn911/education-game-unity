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
        
        private uint sceneNum = 0;
        [SerializeField] private uint sceneCount;

        public uint SceneCount => sceneCount;

        public uint SceneNum => sceneNum;

        public string GameScene
        {
            get
            {
                sceneNum++;
                return "GameScene" + sceneNum;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }

        private void Start()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
