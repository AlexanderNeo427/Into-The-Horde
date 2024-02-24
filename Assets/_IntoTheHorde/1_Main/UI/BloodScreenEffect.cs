using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class BloodScreenEffect : MonoBehaviour
    {
        static readonly float MIN_ALPHA = 0.001f;

        [SerializeField] Health _health;

        Image m_image;

        void Awake()
        {
            m_image = GetComponent<Image>();

            Color color = m_image.color;
            color.a = MIN_ALPHA;
            m_image.color = color;
        }

        void OnEnable() => EventManager.AddListener(GameEvent.OnHealthChanged, HealthChangedHandler);

        void OnDisable() => EventManager.RemoveListener(GameEvent.OnHealthChanged, HealthChangedHandler);

        void Update()
        {
            float currAlpha = m_image.color.a;

            if (currAlpha >= MIN_ALPHA)
            {
                float fadeSpeed = 0.185f;
                float targetAlpha = currAlpha - fadeSpeed * Time.deltaTime;

                Color color = m_image.color;
                color.a = targetAlpha;
                m_image.color = color;
            }
        }

        void HealthChangedHandler(EventArgs eventArgs)
        {
            HealthChangedEventArgs args = eventArgs as HealthChangedEventArgs;

            if (args != null)
            {
                if (args.AffectedHealth == this._health && args.Delta < 0f)
                {
                    float damage = Mathf.Abs( args.Delta );
                    float maxDamage = 20; // Some arbitrary number, from Epsilon-MaxHP
                    float dmgRatio = Mathf.Min(damage / maxDamage, 1f); // Damage normalized from 0-1

                    float maxAlpha = 0.7f;
                    float currAlpha = m_image.color.a;
                    float targetAlpha = Mathf.Min( currAlpha + dmgRatio, maxAlpha );

                    Color color = m_image.color;
                    color.a = targetAlpha;
                    m_image.color = color;
                }
            }
        }
    }
}
