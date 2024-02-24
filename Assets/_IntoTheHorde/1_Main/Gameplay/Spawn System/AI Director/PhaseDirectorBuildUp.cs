using UnityEngine;

namespace IntoTheHorde
{
    public class PhaseDirectorBuildUp : AI_DirectorPhase
    {
        AI_Director m_director;
        float       m_timeElapsed;

        public PhaseDirectorBuildUp(AI_DirectorFSM.PHASE stateID,
                                    AI_Director          aiDirector)
            : base( stateID )
        {
            m_director = aiDirector;
        }

        public override void OnStateEnter() => m_timeElapsed = 0f;

        public override void OnStateUpdate() 
        {
            m_timeElapsed += Time.deltaTime;
            if (m_timeElapsed >= m_director.BuildUpSpawnRate)
            {
                m_timeElapsed = 0f;

                var spawnZones = m_director.GetSpawnZonesInRangeOfPlayer( 50f );
                foreach (SpawnZone spawnZone in spawnZones)
                {
                    spawnZone.Spawn( 1, ZOMBIE_TYPE.REGULAR );
                }
            }
        }

        public override void OnStateExit() {}
    }
}
