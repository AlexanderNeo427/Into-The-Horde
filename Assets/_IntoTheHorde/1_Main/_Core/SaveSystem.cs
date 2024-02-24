using UnityEngine;

using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace IntoTheHorde
{
    public static class SaveSystem
    {
        public static void SaveLeaderboard(LeaderboardData data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/leaderboard.data";
            FileStream stream = new FileStream( path, FileMode.Create );

            formatter.Serialize( stream, data );
            stream.Close();
        }

        public static LeaderboardData LoadLeaderboard()
        {
            string path = Application.persistentDataPath + "/leaderboard.data";

            if (File.Exists( path ))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream( path, FileMode.Open );

                LeaderboardData data = formatter.Deserialize( stream ) as LeaderboardData;
                stream.Close();
                return data;
            }
            else
            {
                LeaderboardData data = new LeaderboardData(new List<LeaderboardEntry>());
                SaveLeaderboard( data );
                Debug.Log("File not found, creating path");
                return null;
            }
        }
    }
}
