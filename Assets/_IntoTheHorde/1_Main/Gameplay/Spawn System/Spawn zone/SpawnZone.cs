using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    public enum ZOMBIE_TYPE { REGULAR }

    [DisallowMultipleComponent]
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField] List<GameObject> _regularZombiePrefabs;

        float m_minX, m_maxX;
        float m_minZ, m_maxZ;

        bool m_isOutOfSight = false;

        public Vector3 Position     => transform.position;
        public bool    IsOutOfSight => m_isOutOfSight;

        void Start()
        {
            m_minX = transform.position.x - transform.localScale.x * 0.5f;
            m_maxX = transform.position.x + transform.localScale.x * 0.5f;
            m_minZ = transform.position.z - transform.localScale.z * 0.5f;
            m_maxZ = transform.position.z + transform.localScale.z * 0.5f;
        }

        public void Spawn(int numZombies, ZOMBIE_TYPE zombieType)
        {
            switch ( zombieType )
            {
                case ZOMBIE_TYPE.REGULAR:
                    for (int i = 0; i < numZombies; ++i)
                    {
                        int   randType  = Random.Range( 0, _regularZombiePrefabs.Count );
                        float randX     = Random.Range( m_minX, m_maxX );
                        float randZ     = Random.Range( m_minZ, m_maxZ );
                        float randAngle = Random.Range( 0f, 359.99999f );

                        Vector3    spawnPos      = new Vector3( randX, 0f, randZ );
                        Quaternion spawnRotation = Quaternion.Euler( 0f, randAngle, 0f );
                        Instantiate(_regularZombiePrefabs[randType], spawnPos, spawnRotation);
                    }
                    break;
            }
        }

        void OnBecameVisible() => m_isOutOfSight = false;

        void OnBecameInvisible() => m_isOutOfSight = true;
    }
}
