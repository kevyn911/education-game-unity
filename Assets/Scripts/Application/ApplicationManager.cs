using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application
{
    public class ApplicationManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static ApplicationManager Instance { get; private set; }
        public GameManager GameManager { get; set; }
        private uint sceneNum = 0;

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
