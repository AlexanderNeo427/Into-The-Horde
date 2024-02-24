using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    public class StateZombieDead : RegularZombieState
    {
        RegularZombieController m_zombie;
        NavMeshAgent            m_navAgent;
        Animator                m_animator;
        bool                    m_despawnTimerUp;
        bool                    m_isOutOfSight;

        public StateZombieDead(RegularZombieFSM.STATE  stateID,
                               RegularZombieController zombieController)
             : base( stateID )
        {
            m_zombie         = zombieController;
            m_navAgent       = zombieController.NavAgent;
            m_animator       = zombieController.Anim;
            m_despawnTimerUp = false;
            m_isOutOfSight   = false;
        }

        public override void OnStateEnter()
        {
            if (m_navAgent.enabled)
            {
                m_navAgent.ResetPath();
                m_navAgent.enabled = false;
            }

            m_zombie.GetComponent<CapsuleCollider>().enabled = false;
            m_zombie.GetComponent<Rigidbody>().isKinematic   = true;
            m_animator.enabled = false;

            m_zombie.StartCoroutine(DespawnTimerCounter( 5f ));
            m_zombie.SetTarget( null );
        }

        public override void OnStateUpdate() 
        {
            if (m_despawnTimerUp && m_isOutOfSight)
                m_zombie.Despawn();    
        }

        public override void OnStateAnimatorMove()
        {
            m_zombie.transform.position = m_animator.rootPosition;
            m_zombie.transform.rotation = m_animator.rootRotation;
        }

        public override void OnStateExit() {}

        public override void OnStateBecameVisible() => m_isOutOfSight = false;

        public override void OnStateBecameInvisible() => m_isOutOfSight = true;

        IEnumerator DespawnTimerCounter(float despawntimer)
        {
            yield return new WaitForSeconds( despawntimer );
            m_despawnTimerUp = true;
        }
    }
}