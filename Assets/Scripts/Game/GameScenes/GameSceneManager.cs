using Application;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Game.GameScenes
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private Rules rules;

        public Rules Rules => rules;

        private void Start()
        {
            SceneManager.SetActiveScene(gameObject.scene);
            ApplicationManager.Instance.GameManager.RegisterGameSceneManager(this);
        }
    }
}
