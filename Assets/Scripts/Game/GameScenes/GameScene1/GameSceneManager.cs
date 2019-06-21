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
        

        public Rules Rules => rules;

        public Image Room
        {
            get => room;
            set => room = value;
        }

        //Тестове завантаження картинок. Зроби норм.
        [SerializeField] private Image[] things;
        [SerializeField] private Canvas canvas;
        public Image[] Things
        {
            get => things;
            set => things = value;
        }

        public Canvas Canvas
        {
            get => canvas;
            set => canvas = value;
        }
        // Вот так.

        private void Start()
        {
            SceneManager.SetActiveScene(gameObject.scene);
            ApplicationManager.Instance.GameManager.RegisterGameSceneManager(this);
        }
    }
}
