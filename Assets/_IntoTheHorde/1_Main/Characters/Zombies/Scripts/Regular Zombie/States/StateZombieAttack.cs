// TODO Delete
//      Attack is now done within chase state

/*using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    public class StateZombieAttack : RegularZombieState
    {
        static readonly int UpperBodyLayerIndex = Constants.Zombie.Regular.AnimLayer.UpperBodyLayer;
        static readonly int BaseLayerIndex      = Constants.Zombie.Regular.AnimLayer.BaseLayer;

        RegularZombieController m_zombie;
        Animator                m_animator;
        NavMeshAgent            m_navAgent;
        float                   m_setDestinationTimer;

        public StateZombieAttack(RegularZombieFSM.STATE  stateID, 
                                 RegularZombieController zombieController)
            : base( stateID )
        {
            m_zombie   = zombieController;
            m_animator = zombieController.Anim;
            m_navAgent = zombieController.NavAgent;
        }

        public override void OnStateEnter()
        {
            m_navAgent.enabled        = true;
            m_navAgent.updatePosition = true;
            m_navAgent.updateRotation = false;
            m_navAgent.speed          = m_zombie.SprintSpeed;

            m_animator.SetBool( m_zombie.Attack_1_Hash, true );
            m_animator.SetLayerWeight( UpperBodyLayerIndex, 1f );

            m_setDestinationTimer = 0f;
        }

        public override void OnStateUpdate()
        {
            if (m_zombie.Target == null)
                m_zombie.SetToIdleOrWander();

            m_setDestinationTimer += Time.deltaTime;
            if (m_setDestinationTimer >= m_zombie.SetDestinationBuffer)
            {
                m_setDestinationTimer = 0f;
                m_navAgent.SetDestination(m_zombie.Target.Position);
            }
        }

        public override void OnStateExit()
        {
            m_animator.SetBool( m_zombie.Attack_1_Hash, false );
            m_animator.SetLayerWeight( UpperBodyLayerIndex, 0f );
        }

        public override void OnStateTriggerEnter(Collider other) {}

        public override void OnStateTriggerExit(Collider other) {}

        public override void OnStateAnimatorMove()
        {
            m_navAgent.velocity = m_animator.deltaPosition / Time.deltaTime;
        }

        public override void OnStateAttackAnimComplete()
        {
            m_zombie.FSM.ChangeState( RegularZombieFSM.STATE.CHASE );
        }

        float DistFromTarget() => Vector3.Distance( m_zombie.transform.position, m_zombie.Target.transform.position );
    }
}
*/