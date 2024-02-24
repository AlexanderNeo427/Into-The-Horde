using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( Collider ))]
    public class Key : MonoBehaviour
    {
        void Awake() => GetComponent<Collider>().isTrigger = true;
    }
}
