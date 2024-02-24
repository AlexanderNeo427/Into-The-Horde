using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    public class LeaderboardEntry : System.IComparable<LeaderboardEntry>
    {
        public int   Kills;
        public float Duration;

        public LeaderboardEntry(int kills, float duration)
        {
            Kills    = kills;
            Duration = duration;
        }

        public int CompareTo(LeaderboardEntry other) => other.Kills.CompareTo( Kills );
    }

    [System.Serializable]
    public class LeaderboardData
    {
        public float[] Durations;
        public int[]   Kills;

        public LeaderboardData(List<LeaderboardEntry> entries)
        {
            int itr = Mathf.Min( 10, entries.Count );

            Durations = new float[itr];
            Kills     = new int[itr];

            for (int i = 0; i < itr; ++i)
            {
                Kills[i]     = entries[i].Kills;
                Durations[i] = entries[i].Duration;
            }
        }

        public List<LeaderboardEntry> ToList()
        {
            var list = new List<LeaderboardEntry>();
            int n = Kills.Length;

            for (int i = 0; i < n; ++i)
            {
                list.Add(new LeaderboardEntry( Kills[i], Durations[i] ));
            }

            return list;
        }
    }
}
