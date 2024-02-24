using UnityEngine;

namespace IntoTheHorde
{
    public class PhaseDirectorRelaxed : AI_DirectorPhase
    {
        AI_Director m_director;
        float       m_timeToTransition;
        float       m_timeElapsed;

        public PhaseDirectorRelaxed(AI_DirectorFSM.PHASE stateID, 
                                    AI_Director          aiDirector) 
            : base( stateID )
        {
            m_director = aiDirector;
        }

        public override void OnStateEnter() 
        {
            float minRelaxTime = m_director.RelaxTimeRange.x;
            float maxRelaxTime = m_director.RelaxTimeRange.y;
            m_timeToTransition = Random.Range( minRelaxTime, maxRelaxTime );
            m_timeElapsed      = 0f;
        }

        public override void OnStateUpdate() 
        {
            m_timeElapsed += Time.deltaTime;

            if (m_timeElapsed >= m_timeToTransition)
                m_director.ChangePhase( AI_DirectorFSM.PHASE.BUILD_UP );
        }

        public override void OnStateExit() {}
    }
}
