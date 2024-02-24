namespace IntoTheHorde
{
    public class PhaseDirectorPeak : AI_DirectorPhase
    {
        AI_Director m_director;

        public PhaseDirectorPeak(AI_DirectorFSM.PHASE stateID,
                                 AI_Director          aiDirector) 
            : base( stateID )
        {
            m_director = aiDirector;
        }

        public override void OnStateEnter() {}

        public override void OnStateUpdate() {}

        public override void OnStateExit() {}
    }
}
