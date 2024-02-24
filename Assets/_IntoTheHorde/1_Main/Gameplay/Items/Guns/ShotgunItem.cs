using UnityEngine;

namespace IntoTheHorde
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item Data/Gun Data/Shotgun Data")]
    public class ShotgunItem : BaseGunItem
    {
        public override void Init() => base.Init();

        public override void OnEquip() {}

        public override void OnUnequip() {}

        public override void OnUseBegin() => this.TryShoot();

        public override void OnUseHeld() {}

        public override void OnUseReleased() {}

        protected override bool ReadyToShoot()
        {
            if (m_reloading || m_firerateBufferActive)
                return false;

            if (m_numBullets <= 0)
            {
                Reload();
                return false;
            }

            return true;
        }

        protected override void TryShoot()
        {
            if (!ReadyToShoot()) return;

            for (int i = 0; i < _bulletsPerShot; ++i)
            {
                EventManager.RaiseEvent(GameEvent.OnGunFired, m_gunFiredArgs);
            }
            m_numBullets -= 1;
            m_animator.SetTrigger( m_shootAnimHash );
            _firingSound.Source.Play();

            m_owner.StartCoroutine(FirerateBufferCoroutine());
        }
    }
}
