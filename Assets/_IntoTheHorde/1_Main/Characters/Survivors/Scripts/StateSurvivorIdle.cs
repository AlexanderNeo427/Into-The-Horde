using UnityEngine;

namespace IntoTheHorde
{
    public class StateSurvivorIdle : SurvivorState
    {
        SurvivorController m_survivor;

        public StateSurvivorIdle(SurvivorFSM.STATE  stateID, 
                                 SurvivorController survivorController) 
            :
            base( stateID ) 
        {
            m_survivor = survivorController;
        }

        public override void OnStateEnter() 
        {
        }

        public override void OnStateUpdate() 
        {
            // Look for weapon if don't have one
        }

        public override void OnStateExit() 
        {
        }

        public override void OnStateSensorEnter(Collider other)
        {
        }

        public override void OnStateSensorStay(Collider other)
        {
        }

        public override void OnStateSensorExit(Collider other)
        {
        }
    }
}
