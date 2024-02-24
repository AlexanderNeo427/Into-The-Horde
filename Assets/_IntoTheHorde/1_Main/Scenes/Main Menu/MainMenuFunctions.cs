using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Trisibo;

namespace IntoTheHorde
{
    public class MainMenuFunctions : MonoBehaviour
    {
        [SerializeField] SceneField _gameScene; 
        [SerializeField] SceneField _creditsScene;

        AsyncOperation m_asyncOp;

        void Start() => StartCoroutine(PreloadSceneAsyncCoroutine(_gameScene));

        public void Play()
        {
            EventManager.RaiseEvent( GameEvent.OnGameBegin, EventArgs.Empty );
            m_asyncOp.allowSceneActivation = true;
        }

        public void Credits() => SceneManager.LoadScene(_creditsScene.BuildIndex);

        public void ExitGame() => Application.Quit();

        IEnumerator PreloadSceneAsyncCoroutine(SceneField scene)
        {
            m_asyncOp = SceneManager.LoadSceneAsync(scene.BuildIndex, LoadSceneMode.Single);
            m_asyncOp.allowSceneActivation = false;
            yield return null;
        }
    }
}
