using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class GameActor : MonoBehaviour
    {
        public enum TEAM { SURVIVOR, ZOMBIE }

        [SerializeField] TEAM _team;

        public TEAM    Team     => _team;
        public Vector3 Position => transform.position;

        void OnEnable() => ActorManager.Instance.RegisterActor( GetInstanceID(), this );

        void OnDisable()
        {
           if (ActorManager.HasValidInstance())
                ActorManager.Instance.UnregisterActor( GetInstanceID() );
        }
    }
}
