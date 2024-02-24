using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    public class StateZombieAlert : RegularZombieState
    {
        RegularZombieController m_zombie;
        NavMeshAgent            m_navAgent;

        float                   m_alertTimer;
        float                   m_timeInAlertState;
        float                   m_timeLastSeenSurvivor;

        float                   m_reactionTime;
        float                   m_screamChance = 0.3333f; // Normalized from 0 - 1

        Collider[]              m_survivorColliders;
        float                   m_LOS_Check_Timer;
        float                   m_halfFOV;

        public StateZombieAlert(RegularZombieFSM.STATE  stateID, 
                                RegularZombieController controller) 
            : base( stateID )
        {
            m_zombie   = controller;
            m_navAgent = controller.NavAgent;
        }

        public override void OnStateEnter()
        {
            m_navAgent.enabled   = true;
            m_navAgent.isStopped = true;

            m_alertTimer       = 0f;
            m_timeInAlertState = UnityEngine.Random.Range( 3f, 8f );

            m_reactionTime = UnityEngine.Random.Range( 0.25f, 1.5f );

            m_halfFOV = m_zombie.FieldOfView * 0.5f;
        }

        public override void OnStateUpdate()
        {
            if (m_zombie.Target == null)
                m_zombie.SetToIdleOrWander();

            // Rotate towards player
            Vector3 targetDir = m_zombie.Target.Position - m_zombie.transform.position;

            float angle = Vector3.SignedAngle(targetDir, m_zombie.transform.forward, Vector3.up);
            if (Mathf.Abs( angle ) > 7f)
            {
                Quaternion targetRotation = Quaternion.LookRotation( targetDir, Vector3.up );
                m_zombie.transform.rotation = Quaternion.Slerp(m_zombie.transform.rotation, targetRotation, Time.deltaTime);
            }

            // Check if player within line of sight
            m_LOS_Check_Timer += Time.deltaTime;
            m_timeLastSeenSurvivor += Time.deltaTime;

            if (m_LOS_Check_Timer >= m_zombie.LineOfSightBufferTime)
            {
                m_LOS_Check_Timer = 0f;

                Vector3 dir = m_zombie.Target.transform.position - m_zombie.transform.position;
                Ray ray = new Ray( m_zombie.transform.position, dir );

                if (Physics.Raycast( ray, out RaycastHit hitInfo, dir.magnitude ))
                {
                    GameActor actorComp = hitInfo.collider.GetComponent<GameActor>();
                    if (actorComp != null && actorComp == m_zombie.Target)
                    {
                        m_timeLastSeenSurvivor = 0f;
                    }
                }
            }

            // Alerted for too long, it will go back to calm, wander state
            m_timeLastSeenSurvivor += Time.deltaTime;
            if (m_timeLastSeenSurvivor >= m_timeInAlertState)
            {
                m_zombie.FSM.ChangeState( RegularZombieFSM.STATE.WANDER );
            }
        }

        public override void OnStateExit() {}

        public override void OnStateAnimatorIK(int layerIndex)
        {
            // TODO : Set head IK goals
            //        Look at target
        }

        IEnumerator ScreamCoroutine()
        {
            m_zombie.FSM.ChangeState( RegularZombieFSM.STATE.CHASE );
            yield return null;
        }
    }
}
