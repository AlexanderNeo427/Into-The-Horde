using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    public class StateZombieIdle : RegularZombieState
    {
        RegularZombieController m_zombie;
        NavMeshAgent            m_navAgent;
        Animator                m_animator;
        int                     m_speedHash;

        float                   m_minIdleTime, m_maxIdleTime;
        float                   m_timeToIdle;
        float                   m_idleTimer;

        float                   m_halfFOV;
        float                   m_LOS_Check_Timer;
        Collider[]              m_survivorColliders;

        public StateZombieIdle(RegularZombieFSM.STATE  stateID,
                               RegularZombieController zombieController) 
            : base( stateID )
        {
            m_zombie            = zombieController;
            m_navAgent          = zombieController.NavAgent;
            m_animator          = zombieController.Anim;
            m_minIdleTime       = zombieController.IdleTimeMin;
            m_maxIdleTime       = zombieController.IdleTimeMax;
            m_speedHash         = zombieController.SpeedHash;
            m_idleTimer         = 0f;
                                
            m_halfFOV           = m_zombie.FieldOfView / 2f;
            m_LOS_Check_Timer   = 0f;
            m_survivorColliders = null;
        }

        public override void OnStateEnter()
        {
            m_navAgent.enabled = true;
            m_navAgent.ResetPath();

            m_timeToIdle = Random.Range( m_minIdleTime, m_maxIdleTime );
            m_idleTimer  = 0f;

            m_animator.SetFloat( m_speedHash, 0f );
        }

        public override void OnStateUpdate()
        {
            m_idleTimer += Time.deltaTime;

            if (m_idleTimer > m_timeToIdle)
                m_zombie.FSM.ChangeState( RegularZombieFSM.STATE.WANDER );

            //  Check for player within line-of-sight
            m_LOS_Check_Timer += Time.deltaTime;
            if (m_LOS_Check_Timer >= m_zombie.LineOfSightBufferTime)
            {
                m_LOS_Check_Timer = 0f;

                m_survivorColliders = Physics.OverlapSphere(m_zombie.transform.position, m_zombie.SightRange, m_zombie.SurvivorLayer);

                // No survivors found
                if (m_survivorColliders.Length == 0) return;

                foreach (Collider surviorCollider in m_survivorColliders)
                {
                    // Check if out of range
                    Vector3 dirForward = m_zombie.transform.forward;
                    Vector3 dirTowardsSurvivor = surviorCollider.transform.position - m_zombie.transform.position;

                    float angleDiff = Vector3.Angle( dirForward, dirTowardsSurvivor );
                    if (angleDiff > m_halfFOV) continue;

                    // Check if can see player
                    Vector3 rayDir = surviorCollider.transform.position - m_zombie.transform.position;
                    if (Physics.Raycast( m_zombie.transform.position, rayDir, out RaycastHit m_hitInfo, m_zombie.SightRange, m_zombie.LineOfSightLayer ))
                    {
                        bool foundSurvivor = m_hitInfo.collider.Equals( surviorCollider );
                        if (foundSurvivor)
                        {
                            GameActor survivor = m_hitInfo.collider.GetComponent<GameActor>();

                            if (survivor.Team == GameActor.TEAM.ZOMBIE)
                                Debug.LogError("StateZombieWander.cs : Survivor is on Zombie team!!");
                            else
                            {
                                // Caught player within line-of-sight
                                m_zombie.SetTarget( survivor );
                                m_zombie.FSM.ChangeState( RegularZombieFSM.STATE.CHASE );
                            }
                        }
                    }
                }
            }
        }

        // If attacked, transition to alert state
        public override void OnStateShot() => m_zombie.FSM.ChangeState( RegularZombieFSM.STATE.ALERT );

        public override void OnStateAnimatorMove()
        {
            m_zombie.transform.position = m_animator.rootPosition;
            m_zombie.transform.rotation = m_animator.rootRotation;
        }

        public override void OnStateExit() {}
    }
}
