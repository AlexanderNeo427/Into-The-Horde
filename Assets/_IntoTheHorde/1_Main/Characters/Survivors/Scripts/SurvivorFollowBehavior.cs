using System;

namespace IntoTheHorde
{
    public class MoveBehaviorFSM : StateMachine
    {
    }

    public abstract class SurvivorMoveBehavior : State
    {
        /*
         *  Survivor AI movement depends on the current target type
         *  
         *  ie. They tend to trail around the player
         *      They will run directly towards items (weapons/medkits etc)
         *      Will try to maintain a certain distance with enemies
         */
        protected SurvivorMoveBehavior(AISurvivorTarget.TYPE stateID) : base( stateID ) {}
    }
}
