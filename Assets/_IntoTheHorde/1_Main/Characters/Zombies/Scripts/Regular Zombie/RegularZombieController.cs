using System;
using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( GameActor ), typeof( Health ))]
    [RequireComponent(typeof( Animator ), typeof( NavMeshAgent ))]
    [RequireComponent(typeof( CapsuleCollider ), typeof( Rigidbody ))]
    public class RegularZombieController : MonoBehaviour
    {
        // TODO : Debug only, delete later
        public string InspectorCurrentState;

        public readonly int SpeedHash     = Animator.StringToHash( Constants.Zombie.Regular.AnimParam.Speed );
        public readonly int Death_1_Hash  = Animator.StringToHash( Constants.Zombie.Regular.AnimParam.Death_1 );
        public readonly int Death_2_Hash  = Animator.StringToHash( Constants.Zombie.Regular.AnimParam.Death_2 );
        public readonly int Attack_1_Hash = Animator.StringToHash( Constants.Zombie.Regular.AnimParam.Attack1 );

        public readonly int IdleSpeed   = Constants.Zombie.Regular.Locomotion.Idle;
        public readonly int WalkSpeed   = Constants.Zombie.Regular.Locomotion.Walk;
        public readonly int RunSpeed    = Constants.Zombie.Regular.Locomotion.Run;
        public readonly int SprintSpeed = Constants.Zombie.Regular.Locomotion.Sprint;

        [Header("Sounds")]
        [SerializeField] Sound[] _growlingSounds;
        [SerializeField] Sound[] _alertedSounds;
        [SerializeField] Sound[] _aggresiveSounds;

        [Header("General")]
        [SerializeField] float     _sightRange            = 20f;
        [SerializeField] float     _lineOfSightBufferTime = 0.2f;
        [SerializeField] float     _fieldOfView           = 135f;
        [SerializeField] LayerMask _survivorLayer;
        [SerializeField] [Range(0.5f, 2f)] 
        float _animSpeedMultiplier = 1f;
        [SerializeField] LayerMask _lineOfSightLayer;

        [Header("Idle State")]
        [SerializeField] [Range(3f, 8f)]  float _idleTimeMin = 5f;
        [SerializeField] [Range(9f, 18f)] float _idleTimeMax = 15f;

        [Header("Chase State")]
        [SerializeField] float _setDestinationBuffer = 0.3f;

        [Header("Attack State")]
        [SerializeField] [Range(1.8f, 3.5f)] float _attackRange  = 2f;
        [SerializeField]                     float _attackDamage = 10f;

        GameActor    m_target = null;

        GameActor        m_gameActor;
        Health           m_health;
        NavMeshAgent     m_navAgent;
        Animator         m_animator;
        RegularZombieFSM m_stateMachine;
        bool             m_useRootPosition;
        bool             m_useRootRotation;
        float            m_intelligence;

        // Debug
        public string            CurrentState => m_stateMachine.CurrentStateID().ToString();
        public bool              PathPending;
        public bool              HasPath;
        public NavMeshPathStatus PathStatus;

        // General zombie stats
        public float            SightRange            => _sightRange;
        public float            LineOfSightBufferTime => _lineOfSightBufferTime;
        public float            FieldOfView           => _fieldOfView;
        public LayerMask        SurvivorLayer         => _survivorLayer;
        public float            AnimSpeedMultiplier   => _animSpeedMultiplier;
        public LayerMask        LineOfSightLayer      => _lineOfSightLayer;
                                
        // Idle state params    
        public float            IdleTimeMin => _idleTimeMin;
        public float            IdleTimeMax => _idleTimeMax;
                                
        // Chase state params   
        public float            SetDestinationBuffer => _setDestinationBuffer;

        // Accessors for private members
        public GameActor        Target   => m_target;
        public GameActor        Actor    => m_gameActor;
        public NavMeshAgent     NavAgent => m_navAgent;
        public Animator         Anim     => m_animator;
        public RegularZombieFSM FSM      => m_stateMachine;

        public bool UseRootPosition
        {
            get => m_useRootPosition;
            set => m_useRootPosition = value;
        }

        public bool UseRootRotation
        {
            get => m_useRootRotation;
            set => m_useRootRotation = value;
        }

        void Awake()
        {
            m_gameActor = GetComponent<GameActor>();
            m_health    = GetComponent<Health>();
            m_navAgent  = GetComponent<NavMeshAgent>();
            m_animator  = GetComponent<Animator>();
        }

        void Start()
        {
            m_navAgent.enabled = true;

            m_stateMachine = new RegularZombieFSM();
            m_stateMachine.AddState(new StateZombieIdle(RegularZombieFSM.STATE.IDLE, this));
            m_stateMachine.AddState(new StateZombieWander(RegularZombieFSM.STATE.WANDER, this));
            m_stateMachine.AddState(new StateZombieAlert(RegularZombieFSM.STATE.ALERT, this));
            m_stateMachine.AddState(new StateZombieChase(RegularZombieFSM.STATE.CHASE, this));
            // m_stateMachine.AddState(new StateZombieAttack(RegularZombieFSM.STATE.ATTACK, this));
            m_stateMachine.AddState(new StateZombieDead(RegularZombieFSM.STATE.DEAD, this));

            // Set initial state
            float rand = UnityEngine.Random.Range( 0f, 100f );
            RegularZombieFSM.STATE initialState;
            initialState = rand < 70f ? RegularZombieFSM.STATE.IDLE : RegularZombieFSM.STATE.WANDER;

            m_stateMachine.ChangeState( initialState );
            m_animator.speed = _animSpeedMultiplier;
        }

        void OnEnable()
        {
            EventManager.AddListener( GameEvent.OnActorDeath, GameActorDeathHandler );
            m_stateMachine?.FSM_OnEnable();
        }

        void OnDisable()
        {
            EventManager.RemoveListener( GameEvent.OnActorDeath, GameActorDeathHandler );
            m_stateMachine?.FSM_OnDisable();
        }

        void Update()
        {
            // For debugging, so I can see the current state in the inspector
            InspectorCurrentState = m_stateMachine.CurrentStateID().ToString();

            m_stateMachine.OnTick(); // Update state machine

            // Animations
            if (m_navAgent.enabled)
            {
                HasPath     = m_navAgent.hasPath;
                PathPending = m_navAgent.pathPending;
                PathStatus  = m_navAgent.pathStatus;

                if (m_navAgent.desiredVelocity.magnitude > 0f && !m_navAgent.isStopped)
                {
                    m_animator.SetFloat(SpeedHash, m_navAgent.desiredVelocity.magnitude, 0.185f, Time.deltaTime);
                }
                else
                {
                    m_animator.SetFloat(SpeedHash, 0.01f, 0.15f, Time.deltaTime);
                }

                if (!m_navAgent.updateRotation)
                {
                    float angle = Vector3.SignedAngle(m_navAgent.desiredVelocity, transform.forward, Vector3.up);
                    if (Mathf.Abs( angle ) > 0.1f)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(m_navAgent.desiredVelocity, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime);
                    }
                }
            }
        }

        void OnAnimatorMove() => m_stateMachine.FSM_OnAnimatorMove();

        void OnAnimatorIK(int layerIndex) => m_stateMachine.FSM_OnAnimatorIK( layerIndex );

        void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
                return;

            m_stateMachine.FSM_OnDrawGizmosSelected();
        }

        public void SetTarget(GameActor target) => m_target = target;

        void GameActorDeathHandler(EventArgs eventArgs)
        {
            ActorDeathEventArgs args = (ActorDeathEventArgs)eventArgs;

            if (args != null)
            {
                if (args.Actor == this.m_gameActor)
                {
                    m_stateMachine.ChangeState( RegularZombieFSM.STATE.DEAD );
                }
                else if (m_target != null && args.Actor == m_target)
                {
                    SetToIdleOrWander();
                }
            }
        }

        public void SetToIdleOrWander()
        {
            float rand = UnityEngine.Random.Range( 0f, 100f );
            RegularZombieFSM.STATE nextState;
            nextState = rand < 50f ? RegularZombieFSM.STATE.IDLE :
                                     RegularZombieFSM.STATE.WANDER;

            m_stateMachine.ChangeState( nextState );
        }

        void AttackTarget()
        {
            if (m_target != null)
            {
                float distFromTarget = Vector3.Distance( transform.position, m_target.transform.position );

                if (distFromTarget <= 5f)
                    m_target.GetComponent<Health>()?.TakeDamage(_attackDamage);
            }
            else
            {
                SetToIdleOrWander();
            }
        }

        void OnAttackAnimComplete() => m_stateMachine.FSM_OnAttackAnimComplete();

        // Called when the zombie is shot
        public void OnShot() => m_stateMachine.FSM_OnShot();

        public void OnHearSound(AI_SOUND_TYPE soundType, Vector3 soundPos) => m_stateMachine.FSM_OnHearSound( soundType, soundPos );

        public void Despawn() => Destroy( gameObject );
    }
}
