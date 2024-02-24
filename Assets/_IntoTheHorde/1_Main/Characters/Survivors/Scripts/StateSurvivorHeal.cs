using UnityEngine;

namespace IntoTheHorde
{
    public class StateSurvivorHeal : SurvivorState
    {
        SurvivorController m_survivor;

        public StateSurvivorHeal(SurvivorFSM.STATE  stateID, 
                                 SurvivorController survivorController) 
            : 
            base( stateID ) 
        {
            m_survivor = survivorController;
        }

        public override void OnStateEnter() {}

        public override void OnStateExit() {}

        public override void OnStateUpdate() {}

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
