using System;
using UnityEngine;
using System.Collections.Generic;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class ScoreManager : MonoBehaviour
    {
#region ++++++++++++++ PERSISTANT SINGLETON ++++++++++++++
        static ScoreManager m_instance = null;

        public static ScoreManager Instance 
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<ScoreManager>();

                    if (m_instance == null)
                        Debug.LogError("We need an instance of this in the scene");
                }
                return m_instance;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad( this );

            if (m_instance == null)
                m_instance = this;
            else
                Destroy( gameObject );
        }
#endregion

        List<LeaderboardEntry> m_leaderboardEntries = new List<LeaderboardEntry>();
        LeaderboardData        m_data               = null;
        float                  m_duration           = 0f;
        int                    m_kills              = 0;
        bool                   m_gameIsActive       = false;

        void Start()
        {
            m_data               = SaveSystem.LoadLeaderboard();
            m_leaderboardEntries = m_data?.ToList().GetRange( 0, 10 );
            m_duration           = 0f;
            m_kills              = 0;
        }

        void OnEnable()
        {
            EventManager.AddListener(GameEvent.OnGameBegin,     GameBeginHandler);
            EventManager.AddListener(GameEvent.OnGameEnd,       GameEndHandler);
            EventManager.AddListener(GameEvent.OnActorDeath,    ActorDeathHandler);
            EventManager.AddListener(GameEvent.OnEnterMainMenu, EnterMainMenuHandler);
        }

        void OnDisable()
        {
            EventManager.RemoveListener(GameEvent.OnGameBegin,     GameBeginHandler);
            EventManager.RemoveListener(GameEvent.OnGameEnd,       GameEndHandler);
            EventManager.RemoveListener(GameEvent.OnActorDeath,    ActorDeathHandler);
            EventManager.RemoveListener(GameEvent.OnEnterMainMenu, EnterMainMenuHandler);
        }

        void Update()
        {
            if (m_gameIsActive)
                m_duration += Time.deltaTime;
        }

        void GameBeginHandler(EventArgs args)
        {
            m_gameIsActive = true;
            m_duration     = 0f;
            m_kills        = 0;
        }

        void GameEndHandler(EventArgs args)
        {
            m_gameIsActive = false;

            LeaderboardEntry entry = new LeaderboardEntry( m_kills, m_duration );

            if (m_leaderboardEntries == null)
                m_leaderboardEntries = new List<LeaderboardEntry>();

            m_leaderboardEntries.Add( entry );
            m_leaderboardEntries.Sort();

            LeaderboardData data = new LeaderboardData( m_leaderboardEntries );
            SaveSystem.SaveLeaderboard( data );
        }

        void ActorDeathHandler(EventArgs eventArgs)
        {
            ActorDeathEventArgs args = eventArgs as ActorDeathEventArgs;
            if (args == null)    return;
            if (!m_gameIsActive) return;

            if (args.Actor.Team == GameActor.TEAM.ZOMBIE)
                ++m_kills;

            if (args.Actor.Team == GameActor.TEAM.SURVIVOR &&
                args.Actor.CompareTag("Player"))
            {
                m_kills    = 0;
                m_duration = 0f;
            }
        }

        void EnterMainMenuHandler(EventArgs args) => m_gameIsActive = false;
    }
}
