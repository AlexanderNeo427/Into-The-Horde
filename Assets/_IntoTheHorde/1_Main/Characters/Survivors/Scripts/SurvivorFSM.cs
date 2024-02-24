using UnityEngine;

namespace IntoTheHorde
{
    public class SurvivorFSM : StateMachine
    {
        public enum STATE { IDLE, SHOOT, HEAL }

        public void OnFSMSensorEnter(Collider other)
        {
            SurvivorState survivorState = m_currentState as SurvivorState;
            survivorState.OnStateSensorEnter( other );
        }

        public void OnFSMSensorStay(Collider other)
        {
            SurvivorState survivorState = m_currentState as SurvivorState;
            survivorState.OnStateSensorStay( other );
        }

        public void OnFSMSensorExit(Collider other)
        {
            SurvivorState survivorState = m_currentState as SurvivorState;
            survivorState.OnStateSensorExit( other );
        }
    }

    public abstract class SurvivorState : State
    {
        protected SurvivorState(SurvivorFSM.STATE stateID) : base( stateID ) {}

        public virtual void OnStateSensorEnter(Collider other) {}
        public virtual void OnStateSensorStay(Collider other) {}
        public virtual void OnStateSensorExit(Collider other) {}
    }

    public class AISurvivorTarget
    {
        // Each target type is mapped to its priority
        // Higher value -> Higher priority
        public enum TYPE 
        {
            NONE   = 0,
            PLAYER = 1,
            ITEM   = 2, 
            ENEMY  = 3,
        }

        public Vector3 Position;
        public TYPE    Type;

        public AISurvivorTarget()
        {
            Position = Vector3.zero;
            Type     = TYPE.NONE;
        }
    }
}
