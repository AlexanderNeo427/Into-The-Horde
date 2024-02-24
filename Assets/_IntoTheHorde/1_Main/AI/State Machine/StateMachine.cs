using System;
using System.Collections.Generic;
using UnityEngine;

/*
 *  How to use:
 *  
 *  - Define a concrete stateMachine, derived from this base StateMachine class
 *  - Add an enum for all the states that you want
 *  
 *  public EnemyStateMachine : StateMachine
 *  {
 *      public enum STATE { IDLE, PATROL, CHASE, ATTACK }
 *  }
 */
namespace IntoTheHorde
{
    public abstract class StateMachine
    {  
        protected State                   m_currentState;
        protected Dictionary<Enum, State> m_stateMap;

        public StateMachine()
        {
            m_currentState = null;
            m_stateMap     = new Dictionary<Enum, State>();
        }

        public void AddState( State newState )
        {
            if (newState == null) return;
            if (m_stateMap.ContainsKey( newState.GetStateID() )) return;

            newState.SetOwner( this );
            m_stateMap.Add( newState.GetStateID(), newState );

            if (m_currentState == null)
            {
                m_currentState = newState;
                m_currentState.OnStateEnter();
            }
        }

        public void ChangeState( Enum stateID )
        {
            if (m_stateMap.TryGetValue( stateID, out State nextState ))
            {
                if (m_currentState.GetStateID().Equals( stateID ))
                    return;

                m_currentState?.OnStateExit();
                m_currentState = nextState;
                m_currentState?.OnStateEnter();
                return;
            }

            Debug.LogError( "StateMachine.cs : ChangeState(), StateID -> " + stateID.ToString() + "is not mapped to a state" );
        }

        public virtual void OnTick() { m_currentState.OnStateUpdate(); }

        public Enum CurrentStateID()
        {
            if (m_currentState == null)
                Debug.LogError( "StateMachine.cs : CurrentStateID(), m_currentState is NULL" );

            return m_currentState.GetStateID();
        }

        /*
         * ----------------- (Optional) MonoBehavior callbacks -------------------
         * 
         *  Just make sure that in the MonoBehavior containing the stateMachine, 
         *  you call the overriden method in the respective callback
         *  
         *  Example:
         *  
         *  public class NewBehaviorScript : MonoBehavior
         *  {
         *      ConcreteStateMachine stateMachine = new ConcreteStateMachine();
         *      
         *      void OnTriggerEnter(Collider other)
         *      {
         *          stateMachine.FSM_OnTriggerEnter( other );
         *      }
         *  }
         */
        public void FSM_OnApplicationFocus() => m_currentState?.OnStateApplicationFocus();
        public void FSM_OnApplicationPause() => m_currentState?.OnStateApplicationPause();
        public void FSM_OnApplicationQuit()  => m_currentState?.OnStateApplicationQuit(); 

        public void FSM_OnAwake()       => m_currentState?.OnStateAwake(); 
        public void FSM_OnEnable()      => m_currentState?.OnStateEnable(); 
        public void FSM_OnStart()       => m_currentState?.OnStateStart(); 
        public void FSM_OnFixedUpdate() => m_currentState.OnStateFixedUpdate(); 
        public void FSM_OnLateUpdate()  => m_currentState.OnStateLateUpdate(); 
        public void FSM_OnDisable()     => m_currentState?.OnStateDisable(); 
        public void FSM_OnDestroy()     => m_currentState?.OnStateDestroy(); 

        public void FSM_OnCollisionEnter(Collision collision)              => m_currentState.OnStateCollisionEnter( collision ); 
        public void FSM_OnCollisionStay(Collision collision)               => m_currentState.OnStateCollisionStay( collision );
        public void FSM_OnCollisionExit(Collision collision)               => m_currentState.OnStateCollisionExit( collision ); 
        public void FSM_OnTriggerEnter(Collider other)                     => m_currentState.OnStateTriggerEnter( other ); 
        public void FSM_OnTriggerStay(Collider other)                      => m_currentState.OnStateTriggerStay( other ); 
        public void FSM_OnTriggerExit(Collider other)                      => m_currentState.OnStateTriggerExit( other ); 
        public void FSM_OnControllerColliderHit(ControllerColliderHit hit) => m_currentState.OnStateControllerColliderHit( hit ); 

        public void FSM_OnMouseEnter()      => m_currentState.OnStateMouseEnter(); 
        public void FSM_OnMouseOver()       => m_currentState.OnStateMouseOver(); 
        public void FSM_OnMouseExit()       => m_currentState.OnStateMouseExit(); 
        public void FSM_OnMouseUp()         => m_currentState.OnStateMouseUp(); 
        public void FSM_OnMouseUpAsButton() => m_currentState.OnStateMouseUpAsButton(); 
        public void FSM_OnMouseDown()       => m_currentState.OnStateMouseDown(); 
        public void FSM_OnMouseDrag()       => m_currentState.OnStateMouseDrag();

        public void FSM_OnDrawGizmos()         => m_currentState.OnStateDrawGizmos();
        public void FSM_OnDrawGizmosSelected() => m_currentState.OnStateDrawGizmosSelected();

        public void FSM_OnBecameVisible()   => m_currentState.OnStateBecameVisible(); 
        public void FSM_OnBecameInvisible() => m_currentState.OnStateBecameInvisible(); 

        public void FSM_OnAnimatorIK(int layerIndex) => m_currentState.OnStateAnimatorIK( layerIndex ); 
        public void FSM_OnAnimatorMove()             => m_currentState.OnStateAnimatorMove(); 
    }
}