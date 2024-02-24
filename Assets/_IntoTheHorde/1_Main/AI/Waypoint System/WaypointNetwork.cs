using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    public class WaypointNetwork : Singleton<WaypointNetwork>, IRegisterable<Waypoint>
    {
        List<Waypoint> m_waypoints = new List<Waypoint>();

        public Waypoint GetRandomWaypoint() => m_waypoints[Random.Range(0, m_waypoints.Count)];

        public Waypoint GetNextWaypoint( Waypoint currWaypoint )
        {
            for (int i = 0; i < m_waypoints.Count; ++i)
            {
                if (currWaypoint.Equals( m_waypoints[i] ))
                {
                    int nextIdx = (i + 1 >= m_waypoints.Count) ? 0 : i + 1;
                    return m_waypoints[nextIdx];
                }
            }

            Debug.LogError( "WaypointNetwork.cs : GetNextWayPoint(), should not be reaching here!" );
            return null;
        }

        public void Register(Waypoint waypoint)
        {
            if (!m_waypoints.Contains( waypoint ))
                m_waypoints.Add( waypoint );
        }

        public void Unregister(Waypoint waypoint)
        {
            if (m_waypoints.Contains( waypoint ))
                m_waypoints.Remove( waypoint );
        }
    }
}
