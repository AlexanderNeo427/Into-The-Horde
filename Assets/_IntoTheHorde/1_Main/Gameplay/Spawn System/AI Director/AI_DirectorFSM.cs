namespace IntoTheHorde
{
    public class AI_DirectorFSM : StateMachine
    {
        public enum PHASE { RELAXED, BUILD_UP, PEAK }
    }

    public abstract class AI_DirectorPhase : State
    {
        protected AI_DirectorPhase(AI_DirectorFSM.PHASE stateID) : base( stateID ) {}
    }
}
