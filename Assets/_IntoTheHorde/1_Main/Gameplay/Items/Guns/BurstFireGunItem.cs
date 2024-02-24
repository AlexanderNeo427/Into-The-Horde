using System.Collections;
using UnityEngine;

namespace IntoTheHorde
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item Data/Gun Data/Burst-Fire Gun Data")]
    public class BurstFireGunItem : BaseGunItem
    {
        [Header("Burst-Fire Gun Data")]
        [SerializeField] int   _shotsPerBurst = 3;
        [SerializeField] float _burstWaitTime = 0.25f;

        WaitForSeconds m_burstBuffer;
        bool           m_burstBufferActive;
        bool           m_shootCoroutineActive;

        public override void Init()
        {
            base.Init();

            m_burstBuffer       = new WaitForSeconds(_burstWaitTime);
            m_burstBufferActive = false;
        }

        public override void OnEquip() {}

        public override void OnUnequip() {}

        public override void OnUseBegin() => TryShoot();

        public override void OnUseHeld() => TryShoot();

        public override void OnUseReleased() {}

        protected override void TryShoot()
        {
            if (!m_shootCoroutineActive)
                m_owner.StartCoroutine(ShootCoroutine());
        }

        IEnumerator ShootCoroutine()
        {
            m_shootCoroutineActive = true;

            for (int i = 0; i < _shotsPerBurst; ++i)
            {
                if (!ReadyToShoot())
                {
                    m_shootCoroutineActive = false;
                    yield break;
                }

                m_numBullets -= _bulletsPerShot;
                _firingSound.Source.Play();
                m_animator.SetTrigger( m_shootAnimHash );
                EventManager.RaiseEvent( GameEvent.OnGunFired, m_gunFiredArgs );
                EventManager.RaiseEvent( GameEvent.OnInventoryChanged, new InventoryChangedEventArgs() );

                if (m_numBullets <= 0)
                {
                    m_shootCoroutineActive = false;
                    Reload();
                    yield break;
                }

                yield return m_firerateBuffer;
            }

            yield return m_burstBuffer;
            m_shootCoroutineActive = false;
            yield return null;
        }

        protected override bool ReadyToShoot()
        {
            if (m_reloading || m_firerateBufferActive || m_burstBufferActive)
                return false;

            if (_bulletsPerShot > m_numBullets)
            {
                Reload();
                return false;
            }

            return true;
        }
    }
}
