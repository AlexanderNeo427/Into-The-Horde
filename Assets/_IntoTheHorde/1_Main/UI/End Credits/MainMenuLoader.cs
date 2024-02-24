using UnityEngine;
using UnityEngine.SceneManagement;
using Trisibo;

namespace IntoTheHorde
{
    public class MainMenuLoader : MonoBehaviour
    {
        [SerializeField] SceneField _mainMenu;

        public void LoadMainMenu() => SceneManager.LoadScene(_mainMenu.BuildIndex);
    }
}
