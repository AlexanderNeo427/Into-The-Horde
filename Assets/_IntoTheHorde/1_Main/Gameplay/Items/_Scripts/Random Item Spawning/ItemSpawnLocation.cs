using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class ItemSpawnLocation : MonoBehaviour
    {
        [SerializeField] bool _canSpawnPrimary    = true;
        [SerializeField] bool _canSpawnSecondary  = true;
        [SerializeField] bool _canSpawnThrowable  = true;
        [SerializeField] bool _canSpawnMedkit     = true;
        [SerializeField] bool _canSpawnConsumable = true;

        public bool CanSpawnPrimary    => _canSpawnPrimary;
        public bool CanSpawnSecondary  => _canSpawnSecondary;
        public bool CanSpawnThrowable  => _canSpawnThrowable;
        public bool CanSpawnMedkit     => _canSpawnMedkit;
        public bool CanSpawnConsumable => _canSpawnConsumable;

        public Vector3 Position => transform.position;

        void OnEnable() => RandomItemSpawnManager.Instance.Register( this );

        void OnDisable()
        {
            if (RandomItemSpawnManager.HasValidInstance())
                RandomItemSpawnManager.Instance.Unregister( this );
        }
    }
}
