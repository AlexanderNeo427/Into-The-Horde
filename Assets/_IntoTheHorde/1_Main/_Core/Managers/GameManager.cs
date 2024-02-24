using System;
using UnityEngine;

namespace IntoTheHorde
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] GameActor _player;

        Vector3    m_originalPosition;
        Quaternion m_originalRotation;

        public bool IsGameOver { get; private set; }

        void OnEnable()
        {
            EventManager.AddListener(GameEvent.OnActorDeath,    ActorDeathHandler);
            EventManager.AddListener(GameEvent.OnRestartGame,   RestartGameHandler);
            EventManager.AddListener(GameEvent.OnEnterMainMenu, EnterMainMenuHandler);
        }

        void OnDisable()
        {
            EventManager.RemoveListener(GameEvent.OnActorDeath,    ActorDeathHandler);
            EventManager.RemoveListener(GameEvent.OnRestartGame,   RestartGameHandler);
            EventManager.RemoveListener(GameEvent.OnEnterMainMenu, EnterMainMenuHandler);
        }

        void Start()
        {
            IsGameOver = false;

            m_originalPosition = _player.transform.position;
            m_originalRotation = _player.transform.rotation;
        }

        void ActorDeathHandler(EventArgs eventArgs)
        {
            ActorDeathEventArgs args = eventArgs as ActorDeathEventArgs;

            if (args != null && args.Actor == this._player)
            {
                Time.timeScale = 0f;
                IsGameOver = true;
            }
        }

        void RestartGameHandler(EventArgs eventArgs)
        {
            Time.timeScale = 1f;
            _player.GetComponent<Health>().RestoreFullHP();
            _player.GetComponent<CharacterController>().enabled = false;
            _player.transform.SetPositionAndRotation(m_originalPosition, m_originalRotation);
            _player.GetComponent<CharacterController>().enabled = true;

            RegularZombieController[] zombieControllers = FindObjectsOfType<RegularZombieController>();

            foreach (var zombie in zombieControllers)
                Destroy( zombie.gameObject );

            IsGameOver = false;
        }

        void EnterMainMenuHandler(EventArgs eventArgs) => Time.timeScale = 1f;
    }
}
