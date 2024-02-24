using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( Collider ))]
    public class ZombieSensor : MonoBehaviour
    {
        [SerializeField] RegularZombieController _zombie;

        Collider m_collider;

        void Awake()
        {
            m_collider = GetComponent<Collider>();
            m_collider.isTrigger = true;
        }

        void OnTriggerEnter(Collider other) => _zombie.FSM.FSM_OnTriggerEnter( other );

        void OnTriggerExit(Collider other) => _zombie.FSM.FSM_OnTriggerExit( other );
    }
}
