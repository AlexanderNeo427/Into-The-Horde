using System;
using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( GameActor ))]
    public class Health : MonoBehaviour
    {
        [Header ("Customisations")]
        [SerializeField] bool  _godMode = false;  // For debug
        [SerializeField] float _maxValue = 100f;  // Don't change this during runtime

        GameActor m_actor;
        float     m_currValue;
        float     m_prevValue;

        public float MaxValue => _maxValue;
        public float Value    => m_currValue;

        HealthChangedEventArgs healthChangedArgs;

        void Awake()
        {
            if (TryGetComponent<GameActor>(out m_actor) == false)
                Debug.LogError(gameObject.name + " health component, missing GameActor reference");

            healthChangedArgs = new HealthChangedEventArgs();

            m_prevValue = _maxValue;
            m_currValue = _maxValue;
        }

        void OnEnable() => EventManager.AddListener(GameEvent.OnRestartGame, RestartGameHandler);

        void OnDisable() => EventManager.RemoveListener(GameEvent.OnRestartGame, RestartGameHandler);

        void Update()
        {
            if (m_prevValue != m_currValue)
            {
                healthChangedArgs.Set( this, m_currValue - m_prevValue );
                EventManager.RaiseEvent( GameEvent.OnHealthChanged, healthChangedArgs );
                m_prevValue = m_currValue;
            }
        }

        public void TakeDamage(float dmg)
        {
            if (_godMode) return;

            if (m_currValue - dmg <= 0f)
            {
                m_currValue = 0f;
                EventManager.RaiseEvent(GameEvent.OnActorDeath, new ActorDeathEventArgs( m_actor ));
            }
            else
            {
                m_currValue -= dmg;
            }
        }

        public void Heal(float healAmount) => m_currValue = Mathf.Min( m_currValue + healAmount, _maxValue );

        public void RestoreFullHP() => m_currValue = _maxValue;

        void RestartGameHandler(EventArgs eventArgs) => RestoreFullHP();
    }
}
