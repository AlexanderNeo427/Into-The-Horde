using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item Data/Health Item/First-Aid Kit")]
    public class FirstAidKitItem : Item
    {
        [SerializeField] [Range(0f, 1f)] float _healRatio = 0.7f; // What percentage of health lost
        [SerializeField]                 float _healTime  = 5f;

        Health m_health;
        float  m_timer;

        public float HealTime => _healTime;

        public override void Init()
        {
            m_timer = 0f;

            m_health = m_owner.GetComponent<Health>();

            if (m_health == null)
                Debug.LogError( "FirstAidKitItem.cs : " + m_owner.name + " missing health component" );
        }

        public override void OnEquip() {}

        public override void OnUnequip() 
        {
            m_timer = 0f;
            EventManager.RaiseEvent(GameEvent.OnHealingEnd, new HealingEndEventArgs( m_owner ));
        }

        public override void OnUseBegin() 
        {
            EventManager.RaiseEvent(GameEvent.OnHealingBegin, new HealingBeginEventArgs( m_owner, HealTime ));
        }

        public override void OnUseHeld()
        {
            m_timer += Time.deltaTime;

            if (m_timer >= _healTime)
            {
                float maxValue   = m_health.MaxValue;
                float currValue  = m_health.Value;
                float diff       = maxValue - currValue;
                float healAmount = diff * _healRatio;

                m_timer = 0f;
                m_health.Heal( healAmount );
                EventManager.RaiseEvent(GameEvent.OnHealingSuccess, new HealingSuccessEventArgs( m_owner ));
            }
        }

        public override void OnUseReleased() 
        {
            m_timer = 0f;
            EventManager.RaiseEvent(GameEvent.OnHealingEnd, new HealingEndEventArgs( m_owner ));
        }
    }
}
