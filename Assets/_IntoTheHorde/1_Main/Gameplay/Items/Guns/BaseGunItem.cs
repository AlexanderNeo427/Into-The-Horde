using System.Collections;
using UnityEngine;

namespace IntoTheHorde
{
    public abstract class BaseGunItem : Item, IReloadable
    {
        [Header("Gun data")]
        [SerializeField] protected Sound  _firingSound;
        [SerializeField] protected Sound  _reloadSound;
        [SerializeField] protected float  _reloadTime     = 1.5f;
        [SerializeField] protected int    _magazineSize   = 30;
        [SerializeField] protected int    _maxMagazines   = 3;
        [SerializeField] protected float  _bulletDamage   = 3;
        [SerializeField] protected int    _bulletsPerShot = 1;
        [SerializeField] protected float  _fireRate       = 7;   // Shots fired per second
        [SerializeField] public    float  _gunRange       = 100f;
        [Header("")]
        [SerializeField] protected string                    _shootAnimParam;
        [SerializeField] protected string                    _idleAnimParam;
        [SerializeField] protected RuntimeAnimatorController _gunAnimController;
        [Header("")]
        [SerializeField] public float _minGunSpread    = 10f;
        [SerializeField] public float _maxGunSpread    = 30f;
        [SerializeField] public float _spreadDecayRate = 500f;

        protected GunFiredEventArgs m_gunFiredArgs;
        protected WaitForSeconds    m_firerateBuffer;
        protected WaitForSeconds    m_reloadBuffer;
        protected bool              m_firerateBufferActive;
        protected bool              m_reloading;
        protected int               m_numBullets;  // Bullets left in current mag
        protected int               m_numMags;     // Number of mags remaining

        // Animation
        protected Animator m_animator;
        protected int      m_shootAnimHash;
        protected int      m_idleAnimHash;

        public RuntimeAnimatorController GunAnimController => _gunAnimController;

        public override void Init()
        {
            // Init gun stats
            m_gunFiredArgs   = new GunFiredEventArgs(m_owner, _gunRange, 
                                                     _bulletDamage, _minGunSpread,
                                                     _maxGunSpread, _spreadDecayRate);

            m_firerateBuffer = new WaitForSeconds(1f / _fireRate);
            m_reloadBuffer   = new WaitForSeconds(_reloadTime);

            m_numBullets = _magazineSize;
            m_numMags    = _maxMagazines;

            // Init sounds
            _firingSound.Source              = m_owner.gameObject.AddComponent<AudioSource>();
            _firingSound.Source.clip         = _firingSound.Clip;
            _firingSound.Source.volume       = _firingSound.Volume;
            _firingSound.Source.pitch        = _firingSound.Pitch;
            _firingSound.Source.loop         = _firingSound.Loop;
            _firingSound.Source.spatialBlend = _firingSound.SpatialBlend;

            _reloadSound.Source              = m_owner.gameObject.AddComponent<AudioSource>();
            _reloadSound.Source.clip         = _reloadSound.Clip;
            _reloadSound.Source.volume       = _reloadSound.Volume;
            _reloadSound.Source.pitch        = _reloadSound.Pitch;
            _reloadSound.Source.loop         = _reloadSound.Loop;
            _reloadSound.Source.spatialBlend = _reloadSound.SpatialBlend;

            m_shootAnimHash = Animator.StringToHash(_shootAnimParam);
            m_idleAnimHash  = Animator.StringToHash(_idleAnimParam);
        }

        public void SetAnimator(Animator animator)
        {
            m_animator       = animator;
            m_animator.speed = _fireRate;
        }

        protected virtual void TryShoot()
        { 
            if (!ReadyToShoot()) return;

            m_numBullets -= _bulletsPerShot;
            _firingSound.Source.Play();
            m_animator.SetTrigger( m_shootAnimHash );
            EventManager.RaiseEvent( GameEvent.OnGunFired, m_gunFiredArgs );

            m_owner.StartCoroutine(FirerateBufferCoroutine());
        }

        public void Reload()
        {
            if (m_numMags <= 0) return;
            if (m_reloading)    return;

            _reloadSound.Source.Play();
            m_owner.StartCoroutine(ReloadCoroutine());
        }

        protected virtual IEnumerator FirerateBufferCoroutine()
        {
            m_firerateBufferActive = true;
            yield return m_firerateBuffer;
            m_firerateBufferActive = false;
        }

        protected virtual IEnumerator ReloadCoroutine()
        {
            m_reloading = true;

            yield return m_reloadBuffer;
            --m_numMags;
            m_numBullets = _magazineSize;
            EventManager.RaiseEvent(GameEvent.OnInventoryChanged, new InventoryChangedEventArgs());

            m_reloading = false;
        }

        protected virtual bool ReadyToShoot()
        {
            if (m_reloading || m_firerateBufferActive)
                return false;

            if (_bulletsPerShot > m_numBullets)
            {
                Reload();
                return false;
            }

            return true;
        }

        public int GetNumBullets()   => m_numBullets;
        public int GetMagazineSize() => _magazineSize;
        public int GetNumMags()      => m_numMags;

        public void RefillAmmo()
        {
            m_numBullets = _magazineSize;
            m_numMags    = _maxMagazines;

            EventManager.RaiseEvent(GameEvent.OnInventoryChanged, new InventoryChangedEventArgs());
        }
    }
}
