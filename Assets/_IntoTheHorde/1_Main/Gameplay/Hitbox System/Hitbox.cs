using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( Collider ))]
    public class Hitbox : MonoBehaviour
    {
        enum HITBOX_TYPE { HEAD, CHEST, ARM, LEG }

        [SerializeField] Health      _health;
        [SerializeField] HITBOX_TYPE _hitboxType;

        static Dictionary<HITBOX_TYPE, float> m_damageMultiplierMap = new Dictionary<HITBOX_TYPE, float>()
        {
            { HITBOX_TYPE.HEAD,  4f    },
            { HITBOX_TYPE.CHEST, 1.25f },
            { HITBOX_TYPE.ARM,   1f    },
            { HITBOX_TYPE.LEG,   0.75f },
        };

        public void OnHit(float damage)
        {
            // TODO : For debug, delete later
            // GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            float adjustedDamage = damage * m_damageMultiplierMap[_hitboxType];
            _health?.TakeDamage( adjustedDamage );
        }
    }
}
