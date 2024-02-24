using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class AI_Director : Singleton<AI_Director>
    {
        [SerializeField] GameActor _player;
        [SerializeField] LayerMask _spawnZoneLayer;

        [Header("Relaxed Phase")]
        [SerializeField] Vector2 _relaxTimeRange = new Vector2( 5f, 15f );

        [Header("Build-Up Phase")]
        [SerializeField] float _buildUpSpawnRate = 0.8f;

        [Header("Peak Phase")]
        [SerializeField] float _peakSpawnRate = 0.4f;

        AI_DirectorFSM  m_directorFSM   = null;

        public Vector2 RelaxTimeRange   => _relaxTimeRange;
        public float   BuildUpSpawnRate => _buildUpSpawnRate;
        public float   PeakSpawnRate    => _peakSpawnRate;

        void Awake()
        {
            if (_player == null || !_player.CompareTag("Player"))
                Debug.LogError("AI Director needs a reference to the player");
        }

        void Start()
        {
            m_directorFSM = new AI_DirectorFSM();
            m_directorFSM.AddState(new PhaseDirectorRelaxed( AI_DirectorFSM.PHASE.RELAXED, this ));
            m_directorFSM.AddState(new PhaseDirectorBuildUp( AI_DirectorFSM.PHASE.BUILD_UP, this ));
            m_directorFSM.AddState(new PhaseDirectorPeak( AI_DirectorFSM.PHASE.PEAK, this ));
            m_directorFSM.ChangeState( AI_DirectorFSM.PHASE.BUILD_UP );
        }

        void Update() => m_directorFSM.OnTick();

        public List<SpawnZone> GetSpawnZonesInRangeOfPlayer(float range)
        {
            var spawnZonesNearPlayer = new List<SpawnZone>();

            Collider[] spawnZoneColliders = Physics.OverlapSphere(_player.Position, range, _spawnZoneLayer);
            foreach (Collider collider in spawnZoneColliders)
            {
                SpawnZone spawnZone = collider.GetComponent<SpawnZone>();

                if (spawnZone != null && spawnZone.IsOutOfSight)
                    spawnZonesNearPlayer.Add( spawnZone );
            }

            return spawnZonesNearPlayer;
        }

        public void ChangePhase(AI_DirectorFSM.PHASE nextPhase) => m_directorFSM.ChangeState( nextPhase );
    }
}
