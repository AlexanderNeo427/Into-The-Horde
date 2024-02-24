using UnityEngine;
using UnityEngine.SceneManagement;
using Trisibo;

namespace IntoTheHorde
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] SceneField _sceneMainMenu;

        public void Retry()
        {
            int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currSceneIndex, LoadSceneMode.Single);
            Time.timeScale = 1f;
        }

        public void ExitToMainMenu() => SceneManager.LoadScene(_sceneMainMenu.BuildIndex);
    }
}
