using System;
using UnityEngine;

namespace IntoTheHorde
{
    public abstract class State
    {
        protected StateMachine m_owner   = null;
        protected Enum         m_stateID = null;

        public State( Enum stateID )
        {
            m_owner   = null;    
            m_stateID = stateID;
        }

        public Enum GetStateID() => m_stateID;
        public void SetOwner( StateMachine owner ) => m_owner = owner;

        public abstract void OnStateEnter();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();

        /*
         * ----------- Overridable MonoBehavior callbacks -------------
         */
        public virtual void OnStateApplicationFocus() {}
        public virtual void OnStateApplicationPause() {}
        public virtual void OnStateApplicationQuit() {}

        public virtual void OnStateAwake() {}
        public virtual void OnStateEnable() {}
        public virtual void OnStateStart() {}
        public virtual void OnStateFixedUpdate() {}
        public virtual void OnStateLateUpdate() {}
        public virtual void OnStateDisable() {}
        public virtual void OnStateDestroy() {}

        public virtual void OnStateCollisionEnter(Collision collision) {}
        public virtual void OnStateCollisionStay(Collision collision) {}
        public virtual void OnStateCollisionExit(Collision collision) {}
        public virtual void OnStateTriggerEnter(Collider other) {}
        public virtual void OnStateTriggerStay(Collider other) {}
        public virtual void OnStateTriggerExit(Collider other) {}
        public virtual void OnStateControllerColliderHit(ControllerColliderHit hit) {}

        public virtual void OnStateMouseEnter() {}
        public virtual void OnStateMouseOver() {}
        public virtual void OnStateMouseExit() {}
        public virtual void OnStateMouseUp() {}
        public virtual void OnStateMouseUpAsButton() {}
        public virtual void OnStateMouseDown() {}
        public virtual void OnStateMouseDrag() {}

        public virtual void OnStateDrawGizmos() {}
        public virtual void OnStateDrawGizmosSelected() {}

        public virtual void OnStateBecameVisible() {}
        public virtual void OnStateBecameInvisible() {}

        public virtual void OnStateAnimatorIK(int layerIndex) {}
        public virtual void OnStateAnimatorMove() {}
    }
}