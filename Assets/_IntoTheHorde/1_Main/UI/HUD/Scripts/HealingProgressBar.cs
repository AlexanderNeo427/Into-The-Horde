using System;
using UnityEngine;
using UnityEngine.UI;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class HealingProgressBar : MonoBehaviour
    {
        [SerializeField] Slider _progressBar;

        float m_healTime;
        float m_timeElapsed;

        void Awake()
        {
            _progressBar.value = 0f;
            m_timeElapsed      = 0f;
            _progressBar.gameObject.SetActive( false );
        }

        void OnEnable()
        {
            EventManager.AddListener( GameEvent.OnHealingBegin,   HealingBeginHandler);
            EventManager.AddListener( GameEvent.OnHealingEnd,     HealingEndHandler  );
            EventManager.AddListener( GameEvent.OnHealingSuccess, HealingEndHandler  );
        }

        void Update()
        {
            if (_progressBar.gameObject.activeSelf)
            {
                m_timeElapsed += Time.deltaTime;
                _progressBar.value = m_timeElapsed / m_healTime;
            }
        }

        void OnDisable()
        {
            EventManager.RemoveListener( GameEvent.OnHealingBegin,   HealingBeginHandler);
            EventManager.RemoveListener( GameEvent.OnHealingEnd,     HealingEndHandler  );
            EventManager.RemoveListener( GameEvent.OnHealingSuccess, HealingEndHandler  );
        }

        void HealingBeginHandler(EventArgs eventArgs)
        {
            HealingBeginEventArgs args = (HealingBeginEventArgs)eventArgs;

            m_timeElapsed         = 0f;
            _progressBar.value    = 0f;
            _progressBar.maxValue = 1f;
            m_healTime            = args.HealingTime;
            _progressBar.gameObject.SetActive( true );
        }

        void HealingEndHandler(EventArgs eventArgs)
        {
            _progressBar.value = 0f;
            m_timeElapsed      = 0f;
            _progressBar.gameObject.SetActive( false );
        }
    }
}
