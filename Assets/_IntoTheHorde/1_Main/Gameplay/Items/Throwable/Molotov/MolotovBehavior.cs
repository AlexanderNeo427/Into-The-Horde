using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class MolotovBehavior : BaseThrowableBehavior
    {
        [SerializeField] ParticleSystem _explosionEffect;
        [Header("")]
        [SerializeField] Burning _firePrefab;
        [SerializeField] float   _burningDuration = 5.5f;

        protected override void Awake() => base.Awake();

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) return;

            Burning firePrefab = Instantiate(_firePrefab, transform.position, Quaternion.identity);
            Destroy( firePrefab.gameObject, _burningDuration );

            ParticleSystem explosion = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy( explosion.gameObject, 1.85f );

            Destroy( this.gameObject );
        }
    }
}
