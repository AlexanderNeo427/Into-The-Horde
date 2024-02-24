using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class PipeBombBehavior : BaseThrowableBehavior
    {
        // [SerializeField] AudioSource    _;
        [SerializeField] ParticleSystem _explosionEffect;
        [SerializeField] Light          _light;
        [Header("")]
        [SerializeField] float _blinkRate  = 0.333f;
        [SerializeField] float _soundRange = 15f;
        [Header("")]
        [SerializeField] float _lifeTime       = 5f;
        [SerializeField] float _blastRadius    = 5f;
        [SerializeField] float _zombieDamage   = 9999f;
        [SerializeField] float _survivorDamage = 8f;

        float m_timeElapsed = 0f;
        float m_blinkTimer  = 0f;

        protected override void Awake()
        {
            base.Awake();

            _light.enabled = false;
            GetComponent<Collider>().isTrigger = false;
        }

        void Update()
        {
            // Life time
            m_timeElapsed += Time.deltaTime;

            if (m_timeElapsed >= _lifeTime)
                OnExplode();

            // Light blinking behavior
            switch (_light.enabled)
            {
                case true:
                    _light.enabled = false;
                    break;
                case false:
                    m_blinkTimer += Time.deltaTime;

                    // Blink light, alert nearby zombies
                    if (m_blinkTimer >= _blinkRate)
                    {
                        m_blinkTimer = 0f;
                        _light.enabled = true;

                        // Alert all zombies within sound range
                        Collider[] colliders = Physics.OverlapSphere(transform.position, _soundRange);
                        foreach (Collider collider in colliders)
                        {
                            RegularZombieController zombie = collider.GetComponent<RegularZombieController>();
                            zombie?.OnHearSound( AI_SOUND_TYPE.PIPE_BOMB, transform.position );
                        }
                    }
                    break;
            }
        }

        void OnExplode()
        {
            // Damage game actors
            Collider[] colliders = Physics.OverlapSphere( transform.position, _blastRadius );
            foreach (Collider collider in colliders)
            {
                GameActor actor = collider.GetComponent<GameActor>();
                if (actor != null)
                {
                    Health health = actor.GetComponent<Health>();
                    switch (actor.Team)
                    {
                        case GameActor.TEAM.SURVIVOR: health?.TakeDamage(_survivorDamage); break;
                        case GameActor.TEAM.ZOMBIE:   health?.TakeDamage(_zombieDamage);   break;
                    }
                }
            }
            
            // Fancy visuals
            ParticleSystem explosion = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy( explosion.gameObject, 1.85f );

            Destroy( this.gameObject );
        }
    }
}
