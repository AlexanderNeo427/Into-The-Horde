using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class ActorManager : Singleton<ActorManager>
    {
        Dictionary<int, GameActor>      m_actorMap  = new Dictionary<int, GameActor>();
        Dictionary<GameActor.TEAM, int> m_teamCount = new Dictionary<GameActor.TEAM, int>();

        public Dictionary<int, GameActor> ActorMap => m_actorMap;

        public void RegisterActor(int ID, GameActor actor)
        {
            if (actor == null)
            {
                Debug.LogError("ActorManager.cs : RegisterActor() can't register NULL Actor");
                return;
            }

            if (!m_actorMap.ContainsKey( ID ))
                m_actorMap.Add( ID, actor );

            if (!m_teamCount.ContainsKey( actor.Team ))
                m_teamCount.Add( actor.Team, 0 );

            ++m_teamCount[actor.Team];
        }

        public void UnregisterActor(int ID)
        {
            if (m_actorMap.TryGetValue( ID, out GameActor actor ))
            {
                --m_teamCount[actor.Team];
                m_actorMap.Remove( ID );
            }
        }

        public int NumActorsInTeam(GameActor.TEAM team)
        {
            int numActors = 0;

            if (m_teamCount.TryGetValue( team, out numActors ))
                return numActors;

            return -1;
        }
    }
}
