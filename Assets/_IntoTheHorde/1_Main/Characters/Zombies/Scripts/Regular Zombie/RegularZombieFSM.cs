using UnityEngine;

namespace IntoTheHorde
{
    public class RegularZombieFSM : StateMachine
    {
        public enum STATE { IDLE, WANDER, ALERT, CHASE, ATTACK, DEAD }

        public void FSM_OnAttackAnimComplete()
        {
            (m_currentState as RegularZombieState).OnStateAttackAnimComplete();
        }

        public void FSM_OnShot()
        {
            (m_currentState as RegularZombieState).OnStateShot();
        }

        public void FSM_OnHearSound(AI_SOUND_TYPE soundType, Vector3 soundPos)
        {
            (m_currentState as RegularZombieState).OnStateHearSound( soundType, soundPos );
        }
    }

    public abstract class RegularZombieState : State
    {
        protected RegularZombieState(RegularZombieFSM.STATE stateID) : base( stateID ) {}

        public virtual void OnStateAttackAnimComplete() {}

        public virtual void OnStateShot() {}

        public virtual void OnStateHearSound(AI_SOUND_TYPE soundType, Vector3 soundPos) {}
    }

    public enum AI_SOUND_TYPE { PIPE_BOMB, EXPLOSION, GUN_SHOT, FOOTSTEP }
}
