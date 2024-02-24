using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class Waypoint : MonoBehaviour
    {
        void OnEnable() => WaypointNetwork.Instance.Register( this );

        void OnDisable()
        {
            if (WaypointNetwork.HasValidInstance())
                WaypointNetwork.Instance.Unregister( this );
        }

        public Quaternion Rotation => transform.localRotation;
        public Vector3    Position => transform.localPosition;
    }
}
