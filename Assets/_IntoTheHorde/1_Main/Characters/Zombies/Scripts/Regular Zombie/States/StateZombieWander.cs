using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    public class StateZombieWander : RegularZombieState
    {
        RegularZombieController m_zombie;
        NavMeshAgent            m_navAgent;
        Animator                m_animator;
        Waypoint                m_currWaypoint;

        Collider[]              m_survivorColliders;
        float                   m_LOS_Check_Timer;
        float                   m_halfFOV;

        public StateZombieWander(RegularZombieFSM.STATE  stateID,
                                 RegularZombieController zombieController) 
            : base( stateID )
        {
            m_zombie            = zombieController;
            m_navAgent          = zombieController.NavAgent;
            m_animator          = zombieController.Anim;
            m_currWaypoint      = null;

            m_survivorColliders = null;
            m_LOS_Check_Timer   = 0f;
            m_halfFOV           = m_zombie.FieldOfView / 2f;
        }

        public override void OnStateEnter()
        {
            m_navAgent.enabled         = true;
            m_navAgent.isStopped       = false;
            m_navAgent.updatePosition  = true;
            m_navAgent.updateRotation  = false;
            m_navAgent.speed           = m_zombie.WalkSpeed;

            m_currWaypoint = WaypointNetwork.Instance.GetRandomWaypoint();
            m_navAgent.SetDestination( m_currWaypoint.Position );

            m_LOS_Check_Timer = 0f;
        }

        public override void OnStateUpdate()
        {
            // Pathfinding
            if (!m_navAgent.hasPath || m_navAgent.isPathStale ||
                m_navAgent.pathStatus == NavMeshPathStatus.PathPartial || 
                m_navAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                m_navAgent.ResetPath();
                m_currWaypoint = WaypointNetwork.Instance.GetRandomWaypoint();
                m_navAgent.SetDestination( m_currWaypoint.Position );
            }
            if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
            {
                m_currWaypoint = WaypointNetwork.Instance.GetRandomWaypoint();
                m_navAgent.SetDestination( m_currWaypoint.Position );
            }

            // Find survivors within line-of-sight
            m_LOS_Check_Timer += Time.deltaTime;
            if (m_LOS_Check_Timer >= m_zombie.LineOfSightBufferTime)
            {
                m_LOS_Check_Timer = 0f;

                m_survivorColliders = Physics.OverlapSphere( m_zombie.transform.position, m_zombie.SightRange, m_zombie.SurvivorLayer );

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
                                m_zombie.SetTarget( survivor );
                                m_zombie.FSM.ChangeState( RegularZombieFSM.STATE.CHASE );
                            }
                        }
                    }
                }
            }
        }

        public override void OnStateExit() {}

        // If attacked, transition to alert state
        public override void OnStateShot() => m_zombie.FSM.ChangeState( RegularZombieFSM.STATE.ALERT );

        public override void OnStateAnimatorMove()
        {
            if (!Mathf.Approximately( Time.deltaTime, 0f ))
                m_navAgent.velocity = m_animator.deltaPosition / Time.deltaTime;
        }

/*        public override void OnStateDrawGizmosSelected()
        {
            Gizmos.color = new Color( 255, 0, 0, 50f );
            Gizmos.DrawSphere( m_zombie.transform.position, m_zombie.SightRange );
        }*/
    }
}
