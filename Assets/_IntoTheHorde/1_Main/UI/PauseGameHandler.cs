using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Trisibo;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class PauseGameHandler : MonoBehaviour
    {
        [SerializeField] GameObject _pauseMenu;
        [SerializeField] Button     _resumeButton;
        [Header("")]
        [SerializeField] Button     _exitToMainMeuButton;
        [SerializeField] SceneField _sceneMainMenu;

        void Start()
        {
            _pauseMenu.SetActive( false );
            _resumeButton.onClick.AddListener( ResumeGame );
            _exitToMainMeuButton.onClick.AddListener( LoadMainMenu );
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown( KeyCode.M ))
            {
                PauseGame();
            }
#else
            if (Input.GetKeyDown( KeyCode.Escape ))
            {
                PauseGame();
            }
#endif
        }

        void PauseGame()
        {
            if (GameManager.HasValidInstance() &&
                GameManager.Instance.IsGameOver)
                return;

            _pauseMenu.SetActive( true );
            Time.timeScale   = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible   = true;
        }

        void ResumeGame()
        {
            if (GameManager.HasValidInstance() &&
                GameManager.Instance.IsGameOver)
                return;

            _pauseMenu.SetActive( false );
            Time.timeScale   = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }

        void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_sceneMainMenu.BuildIndex);
        }
    }
}
