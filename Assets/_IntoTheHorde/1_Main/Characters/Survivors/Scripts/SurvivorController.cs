using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( GameActor ), typeof( NavMeshAgent ))]
    [RequireComponent(typeof( Animator ), typeof( Health ), typeof( AISurvivorInventory ))]
    public class SurvivorController : MonoBehaviour
    {
        public enum FOLLOW_STATE { IDLE, FOLLOW }

        [SerializeField] LayerMask _floorLayer;

        // ----- Constants -----
        public readonly int   BASE_LAYER       = 0;
        public readonly int   UPPER_BODY_LAYER = 1;

        public readonly int   SPEED_HASH       = Animator.StringToHash("f_Speed");
        public readonly int   ITEM_TYPE_HASH   = Animator.StringToHash("i_ItemType");
        public readonly int   IS_AIMING_HASH   = Animator.StringToHash("b_IsAiming");

        public readonly float SPRINT_SPEED     = 5.437279f;  // Got this speed from the animator tab (hardcoded yes I know)

        public readonly int   DEFAULT_POSE     = 0;
        public readonly int   PISTOL_POSE      = 1;
        public readonly int   RIFLE_POSE       = 2;

        // ----- Component cache -----
        public GameActor           Actor        { get; private set; }
        public NavMeshAgent        NavAgent     { get; private set; }
        public Animator            Anim         { get; private set; }
        public Health              HP           { get; private set; }
        public AISurvivorInventory AI_Inventory { get; private set; }

        // ----- Member vars ----- 
        SurvivorFSM      m_survivorFSM;
        GameActor        m_player;
        FOLLOW_STATE     m_followState = FOLLOW_STATE.IDLE;
        AISurvivorTarget m_target;


        void Awake()
        {
            Actor        = GetComponent<GameActor>();
            Anim         = GetComponent<Animator>();
            HP           = GetComponent<Health>();
            NavAgent     = GetComponent<NavMeshAgent>();
            AI_Inventory = GetComponent<AISurvivorInventory>();

            // Make sure the scene has a player
            m_player = GameObject.FindGameObjectWithTag("Player")
                                 .GetComponent<GameActor>();

            if (m_player == null)
                Debug.LogError("SurvivorController.cs : No objects tagged 'Player' in scene");
        }

        void Start()
        {
            m_target = new AISurvivorTarget();

            Anim.SetLayerWeight( UPPER_BODY_LAYER, 1f );

            NavAgent.ResetPath();
            NavAgent.isStopped      = false;
            NavAgent.updatePosition = true;
            NavAgent.updateRotation = false;

            SurvivorState idleState  = new StateSurvivorIdle( SurvivorFSM.STATE.IDLE, this );
            SurvivorState shootState = new StateSurvivorShoot( SurvivorFSM.STATE.SHOOT, this );
            SurvivorState healState  = new StateSurvivorHeal( SurvivorFSM.STATE.HEAL, this );

            m_survivorFSM = new SurvivorFSM();
            m_survivorFSM.AddState( idleState );
            m_survivorFSM.AddState( shootState );
            m_survivorFSM.AddState( healState );
            m_survivorFSM.ChangeState( SurvivorFSM.STATE.IDLE );
        }

        void Update()
        {
            m_survivorFSM.OnTick();

            // Update movement animation
            Anim.SetFloat( SPEED_HASH, NavAgent.desiredVelocity.magnitude, 0.1f, Time.deltaTime );

            /*
             *  Update upper body animation
             *  depending on what the survivor is holding
             *  
             *  0 - Nothing
             *  1 - Pistol pose
             *  2 - Rifle pose
             */
            int targetPoseIndex = Anim.GetInteger( ITEM_TYPE_HASH );
            Item currItem = AI_Inventory.CurrentItem;

            if (currItem != null)
            {
                switch ( currItem.SlotType )
                {
                    case Inventory.SLOT_TYPE.PRIMARY:   targetPoseIndex = RIFLE_POSE;  break;
                    case Inventory.SLOT_TYPE.SECONDARY: targetPoseIndex = PISTOL_POSE; break;
                }
            }

            int currPoseIndex = Anim.GetInteger( ITEM_TYPE_HASH );
            if (currPoseIndex != targetPoseIndex)
                Anim.SetInteger( ITEM_TYPE_HASH, targetPoseIndex );

            // Manual rotation update
            if ( !NavAgent.updateRotation )
            {
                float angle = Vector3.SignedAngle( NavAgent.desiredVelocity, transform.forward, Vector3.up );
                if (Mathf.Abs( angle ) > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation( NavAgent.desiredVelocity, Vector3.up );
                    transform.rotation = Quaternion.Slerp( transform.rotation, targetRotation, 7f * Time.deltaTime );
                }
            }

            if (m_target != null)
            {
                switch ( m_target.Type )
                {
                    case AISurvivorTarget.TYPE.NONE:
                        break;
                }
            }

            // AI follow behavior
            switch ( m_followState )
            {
                case FOLLOW_STATE.IDLE:
                    float range = NavAgent.stoppingDistance * 3f;   // Edit range here
                    bool outOfRange = NavAgent.remainingDistance >= range;
                    if ( outOfRange )
                    {
                        m_followState = FOLLOW_STATE.FOLLOW;
                        NavAgent.ResetPath();
                        NavAgent.isStopped = false;

                        if (m_target.Type != AISurvivorTarget.TYPE.NONE)
                            NavAgent.SetDestination( m_target.Position );
                    }
                    break;
                case FOLLOW_STATE.FOLLOW:
                    bool isWithinRange = NavAgent.remainingDistance < NavAgent.stoppingDistance;
                    if (isWithinRange)
                    {
                        NavAgent.ResetPath();
                        NavAgent.isStopped = true;
                        m_followState = FOLLOW_STATE.IDLE;
                    }
                    break;
            }

            NavAgent.SetDestination( m_player.Position );
        }

        void OnAnimatorMove() => NavAgent.velocity = Anim.deltaPosition / Time.deltaTime;

        public void ChangeState(SurvivorFSM.STATE nextState) => m_survivorFSM.ChangeState( nextState );

        public void OnSensorEnter(Collider other)
        {
            m_survivorFSM.OnFSMSensorEnter( other );
            // If detects item
            /*PickupInteraction pickupable = other.GetComponent<PickupInteraction>();
            if (pickupable != null)
            {
                Item itemPickup = pickupable.GetItem();
                Item currItem = AI_Inventory.GetItem( itemPickup.SlotType );

                // And said item slot is empty
                // pickup item
                if (currItem == null)
                {
                    AI_Inventory.AddItem( itemPickup );
                    Destroy( pickupable.gameObject );
                }
            }*/
        }

        public void OnSensorStay(Collider other) => m_survivorFSM.OnFSMSensorStay( other );

        public void OnSensorExit(Collider other) => m_survivorFSM.OnFSMSensorExit( other );

        public void SetTarget(AISurvivorTarget.TYPE targetType, Vector3 targetPosition)
        {
            m_target.Type     = targetType;
            m_target.Position = targetPosition;
        }
    }
}
