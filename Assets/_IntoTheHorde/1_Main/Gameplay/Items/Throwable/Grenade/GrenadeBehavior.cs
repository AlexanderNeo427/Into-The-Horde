using UnityEngine;

/*
 *  Attach this script to a any object and boom (no pun intended)
 *  you have got yourself a grenade
 */
namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class GrenadeBehavior : BaseThrowableBehavior
    {
        [SerializeField] float          _explosionRadius = 5f;
        [SerializeField] float          _zombieDamage    = 999f;
        [SerializeField] float          _survivorDamage  = 8f;
        [SerializeField] ParticleSystem _explosionEffect;

        protected override void Awake() => base.Awake();

        public override void InitThrowForce(Vector3 throwForce) => base.InitThrowForce( throwForce );

        /*
         * TODO : Go with an event-based approach?
         *        then each type of collider that takes damage
         *        can decide how much damage they take
         *        
         *        i.e. Zombies take more damage from 
         *             explosions than survivors
         *             
         *        This will suffice for now.......
         */
        void OnTriggerEnter(Collider other)
        {
            Collider[] colliders = Physics.OverlapSphere( transform.position, _explosionRadius );

            foreach (Collider collider in colliders)
            {
                GameActor actorComp = collider.GetComponent<GameActor>();

                if (actorComp != null)
                {
                    switch ( actorComp.Team )
                    {
                        case GameActor.TEAM.ZOMBIE:
                            collider.GetComponent<Health>()?.TakeDamage(_zombieDamage);
                            break;
                        case GameActor.TEAM.SURVIVOR:
                            collider.GetComponent<Health>()?.TakeDamage(_survivorDamage);
                            break;
                    }
                }
            }

            ParticleSystem explosion = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy( explosion.gameObject, 1.85f );
            Destroy( this.gameObject );
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere( transform.position, _explosionRadius );
        }
    }
}
