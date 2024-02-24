using UnityEngine;

namespace IntoTheHorde
{
    public class LeaderboardPage : MonoBehaviour
    {
        [SerializeField] LeaderboardEntryView _entryPrefab;
        [SerializeField] Transform            _entryParent;

        void OnEnable()
        {
            WipeLeaderboard();
            LoadLeaderboard();
        }

        void WipeLeaderboard()
        {
            var entries = _entryParent.GetComponentsInChildren<LeaderboardEntryView>();

            foreach (var entry in entries)
                Destroy( entry.gameObject );
        }

        void LoadLeaderboard()
        {
            LeaderboardData data = SaveSystem.LoadLeaderboard();

            int size = Mathf.Min( data.Kills.Length, 10 );

            for (int i = 0; i < size; ++i)
            {
                var entryView = Instantiate(_entryPrefab, _entryParent);
                entryView.SetInfo(data.Kills[i], data.Durations[i]);
            }
        }
    }
}
