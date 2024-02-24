using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class HealthBarUI : MonoBehaviour
    {
        public const float MEDIUM_HP_RATIO = 0.5f;
        public const float LOW_HP_RATIO    = 0.25f;

        [SerializeField] Health _health;
        [Space( 1 )]
        [SerializeField] Slider   _healthBarSlider;
        [SerializeField] Image    _healthBarFill;
        [SerializeField] TMP_Text _healthText;
        [Header("Options")]
        [SerializeField] Color _highHealthColor   = Color.green;
        [SerializeField] Color _mediumHealthColor = Color.yellow;
        [SerializeField] Color _lowHealthColor    = Color.red;

        void Awake()
        {
            if (_health != null)
                _healthText.text = _health.MaxValue.ToString();
        }

        void OnEnable() => EventManager.AddListener( GameEvent.OnHealthChanged, UpdateHealthBarUI );

        void OnDisable() => EventManager.RemoveListener( GameEvent.OnHealthChanged, UpdateHealthBarUI );

        public void UpdateHealthBarUI(EventArgs eventArgs)
        {
            HealthChangedEventArgs args = (HealthChangedEventArgs)eventArgs;

            if (args.AffectedHealth != this._health)
                return;

            if (_health != null)
            {
                float ratio = _health.Value / _health.MaxValue;
                _healthBarSlider.value = ratio;

                int healthValue = (int)_health.Value;
                _healthText.text = healthValue.ToString();

                if (ratio <= MEDIUM_HP_RATIO)
                {
                    if (ratio <= LOW_HP_RATIO)
                        _healthBarFill.color = _lowHealthColor;
                    else
                        _healthBarFill.color = _mediumHealthColor;
                }
                else
                    _healthBarFill.color = _highHealthColor;
            }
        }
    }
}
