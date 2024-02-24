using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    public class StateZombieChase : RegularZombieState
    {
        static readonly int UpperBodyLayerIndex = Constants.Zombie.Regular.AnimLayer.UpperBodyLayer;

        RegularZombieController m_zombie;
        NavMeshAgent            m_navAgent;
        Animator                m_animator;
        float                   m_setDestinationTimer;

        public StateZombieChase(RegularZombieFSM.STATE  stateID, 
                                RegularZombieController zombieController) 
            : base( stateID )
        {
            m_zombie   = zombieController;
            m_navAgent = zombieController.NavAgent;
            m_animator = zombieController.Anim;
        }

        public override void OnStateEnter()
        {
            if (m_zombie.Target == null)
            {
                m_zombie.SetToIdleOrWander();
                return;
            }

            m_navAgent.enabled        = true;
            m_navAgent.isStopped      = false;
            m_navAgent.updatePosition = true;
            m_navAgent.updateRotation = false;
            m_navAgent.speed          = m_zombie.SprintSpeed;
            m_navAgent.SetDestination( m_zombie.Target.transform.position );
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
                m_navAgent.SetDestination( m_zombie.Target.Position );
            }

            // Rotate towards target 
            if ( m_navAgent.isStopped )
            {
                Vector3 targetDir = m_zombie.Target.Position - m_zombie.transform.position;

                float angle = Vector3.SignedAngle(targetDir, m_zombie.transform.forward, Vector3.up);
                if (Mathf.Abs( angle ) > 7f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation( targetDir, Vector3.up );
                    m_zombie.transform.rotation = Quaternion.Slerp(m_zombie.transform.rotation, targetRotation, 5f * Time.deltaTime);
                }
            }
        }

        public override void OnStateExit() {}

        public override void OnStateTriggerEnter(Collider other)
        {
            GameActor actorComp = other.GetComponent<GameActor>();

            if (actorComp != null && actorComp.Team == GameActor.TEAM.SURVIVOR)
            {
                m_animator.SetBool( m_zombie.Attack_1_Hash, true );
                m_animator.SetLayerWeight( UpperBodyLayerIndex, 1f );
                m_navAgent.isStopped = true;
            }
        }

        public override void OnStateTriggerExit(Collider other)
        {
            GameActor actorComp = other.GetComponent<GameActor>();

            if (actorComp != null && actorComp.Team == GameActor.TEAM.SURVIVOR)
            {
                m_navAgent.isStopped = false;
            }
        }

        public override void OnStateAnimatorMove() => m_navAgent.velocity = m_animator.deltaPosition / Time.deltaTime;

        public override void OnStateAnimatorIK(int layerIndex)
        {
            m_animator.SetLookAtPosition( m_zombie.Target.transform.position );
            m_animator.SetLookAtWeight( 1f );
        }

        float DistFromTarget() => Vector3.Distance( m_zombie.transform.position, m_zombie.Target.transform.position );
    }
}
