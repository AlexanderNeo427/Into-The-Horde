using UnityEngine;

namespace IntoTheHorde
{
    public class StateSurvivorShoot : SurvivorState
    {
        SurvivorController m_survivor;

        public StateSurvivorShoot(SurvivorFSM.STATE  stateID,
                                  SurvivorController survivorController) 
            : 
            base( stateID ) 
        {
            m_survivor = survivorController;
        }

        public override void OnStateEnter() 
        {
            // If player not holding any thing, go back to the idle state
            Item currItem = m_survivor.AI_Inventory.CurrentItem;
            if (currItem == null)
            {
                m_survivor.ChangeState( SurvivorFSM.STATE.IDLE );
                return;
            }
        }

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
