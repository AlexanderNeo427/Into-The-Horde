using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( SphereCollider ))]
    public class Burning : MonoBehaviour
    {
        [SerializeField] ParticleSystem _fireParticleEffect;
        [Header("Damage per second")]
        [SerializeField] float _survivorDamage = 4f;
        [SerializeField] float _zombieDamage   = 25f;

        List<ParticleSystem> m_fireParticles = new List<ParticleSystem>();
        List<Health>         m_damageables   = new List<Health>();

        void Start()
        {
            float radius = GetComponent<SphereCollider>().radius;
            for (int i = 0; i < 8; ++i)
            {
                Vector2 offset = Random.insideUnitCircle * Random.Range(radius * 0.333f, radius);
                Vector3 pos = transform.position + new Vector3( offset.x, 0f, offset.y );

                ParticleSystem fire = Instantiate( _fireParticleEffect, pos, Quaternion.identity, transform );
                m_fireParticles.Add( fire );
            }
        }

        void Update()
        {
            foreach (Health health in m_damageables)
            {
                GameActor actor = health.gameObject.GetComponent<GameActor>();
                if (actor != null)
                {
                    switch ( actor.Team )
                    {
                        case GameActor.TEAM.SURVIVOR: 
                            health.TakeDamage(_survivorDamage * Time.deltaTime); 
                            break;
                        case GameActor.TEAM.ZOMBIE:  
                            health.TakeDamage(_zombieDamage * Time.deltaTime);  
                            break;
                    }
                }
            }

            transform.Rotate( Vector3.up, 18.5f * Time.deltaTime );
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<GameActor>() != null)
            {
                m_damageables.Add(other.GetComponent<Health>());
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<GameActor>() != null)
            {
                Health health = other.GetComponent<Health>();
                if (m_damageables.Contains( health ))
                    m_damageables.Remove( health );
            }
        }

        void OnDestroy()
        {
            foreach (var particleSystem in m_fireParticles)
            {
                Destroy( particleSystem.gameObject );
            }
        }
    }
}
