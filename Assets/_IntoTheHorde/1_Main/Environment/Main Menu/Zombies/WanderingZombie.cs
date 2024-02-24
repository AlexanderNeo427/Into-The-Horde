using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class WanderingZombie : MonoBehaviour
    {
        static readonly int   SpeedHash = Animator.StringToHash( Constants.Zombie.Regular.AnimParam.Speed );
        static readonly float MinSpeed  = 0.8f;
        static readonly float MaxSpeed  = 2.38f;

        [SerializeField] WaypointNetwork _waypointNetwork;

        NavMeshAgent m_navAgent;
        Animator     m_animator;
        Waypoint     m_currWaypoint;
        float        m_timer = 0f;
        float        m_timeToTryNextWP = 0f;

        void Awake()
        {
            m_navAgent = GetComponent<NavMeshAgent>();
            m_animator = GetComponent<Animator>();
        }

        void Start()
        {
            m_currWaypoint = _waypointNetwork.GetRandomWaypoint();
            m_navAgent.SetDestination( m_currWaypoint.Position );

            m_animator.applyRootMotion = false;
            m_navAgent.enabled         = true;
            m_navAgent.updatePosition  = true;
            m_navAgent.updateRotation  = false;
            m_navAgent.speed           = Random.Range( MinSpeed, MaxSpeed );

            m_timer           = 0f;
            m_timeToTryNextWP = Random.Range( 6f, 18f );

            AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo( 0 );
            m_animator.Play(state.fullPathHash, -1, Random.Range( 0f, 1f ));
        }

        void Update()
        {
            m_timer += Time.deltaTime;

            if (m_timer >= m_timeToTryNextWP)
            {
                m_timer = 0f;
                m_timeToTryNextWP = Random.Range( 6f, 18f );

                m_navAgent.ResetPath();
                m_currWaypoint = _waypointNetwork.GetRandomWaypoint();
                m_navAgent.SetDestination( m_currWaypoint.Position );
            }

            // Pathfinding
            if (!m_navAgent.hasPath || m_navAgent.isPathStale ||
                m_navAgent.pathStatus == NavMeshPathStatus.PathPartial ||
                m_navAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                m_navAgent.ResetPath();
                m_currWaypoint = _waypointNetwork.GetRandomWaypoint();
                m_navAgent.SetDestination(m_currWaypoint.Position);
                m_timer = 0f;
            }

            if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
            {
                m_currWaypoint = _waypointNetwork.GetRandomWaypoint();
                m_navAgent.SetDestination(m_currWaypoint.Position);
                m_timer = 0f;
            }

            if (m_navAgent.desiredVelocity.magnitude > 0f)
            {
                Vector3 localTargetDir = transform.InverseTransformVector(m_navAgent.desiredVelocity);

                float angle = Vector3.SignedAngle(m_navAgent.desiredVelocity, transform.forward, Vector3.up);
                if (Mathf.Abs( angle ) > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(m_navAgent.desiredVelocity, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1.5f * Time.deltaTime);
                }
                m_animator.SetFloat(SpeedHash, localTargetDir.z, 0.2f, Time.deltaTime);
            }
        }

        void OnAnimatorMove() => m_navAgent.velocity = m_animator.deltaPosition / Time.deltaTime;
    }
}
