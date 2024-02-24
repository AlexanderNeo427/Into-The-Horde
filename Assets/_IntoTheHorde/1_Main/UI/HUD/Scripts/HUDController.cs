using System;
using UnityEngine;
using UnityEngine.UI;

namespace IntoTheHorde
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] GameObject _HUD;
        [SerializeField] Image      _scopeImage;
        [SerializeField] GameObject _gameOverScreen;

        void Awake()
        {
            _HUD.SetActive( true );
            _scopeImage.color = new Color(0f, 0f, 0f, 0f);
            _gameOverScreen.SetActive( false );
        }

        void Start()
        {
            Cursor.visible   = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void OnEnable()
        {
            EventManager.AddListener(GameEvent.OnGunScope,    OnGunScopeHandler);
            EventManager.AddListener(GameEvent.OnGunUnscope,  OnGunUnscopeHandler);
            EventManager.AddListener(GameEvent.OnActorDeath,  ActorDeathHandler);
            EventManager.AddListener(GameEvent.OnRestartGame, RestartGameHandler);
        }

        void OnDisable()
        {
            EventManager.RemoveListener(GameEvent.OnGunScope,    OnGunScopeHandler);
            EventManager.RemoveListener(GameEvent.OnGunUnscope,  OnGunUnscopeHandler);
            EventManager.RemoveListener(GameEvent.OnActorDeath,  ActorDeathHandler);
            EventManager.RemoveListener(GameEvent.OnRestartGame, RestartGameHandler);
        }

        void OnGunScopeHandler(EventArgs eventArgs)
        {
            GunScopeEventArgs args = eventArgs as GunScopeEventArgs;
            if (args != null)
            {
                _scopeImage.color = new Color( 1f, 1f, 1f, 1f );
                _scopeImage.sprite = args.ScopeSprite;
            }

            _HUD.SetActive( false );
        }

        void OnGunUnscopeHandler(EventArgs eventArgs)
        {
            _scopeImage.color = new Color(0f, 0f, 0f, 0f);
            _HUD.SetActive( true );
        }

        void ActorDeathHandler(EventArgs eventArgs)
        {
            ActorDeathEventArgs args = eventArgs as ActorDeathEventArgs;

            if (args != null && args.Actor.CompareTag("Player"))
            {
                _gameOverScreen.SetActive( true );

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        void RestartGameHandler(EventArgs eventArgs)
        {
            Cursor.visible   = false;
            Cursor.lockState = CursorLockMode.Locked;

            _gameOverScreen.SetActive( false );
        }
    }
}
