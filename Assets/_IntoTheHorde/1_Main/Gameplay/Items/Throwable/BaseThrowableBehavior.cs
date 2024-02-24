using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( Collider ), typeof( Rigidbody ))]
    public class BaseThrowableBehavior : MonoBehaviour
    {
        protected virtual void Awake() => GetComponent<Collider>().isTrigger = true;

        public virtual void InitThrowForce(Vector3 throwForce) => GetComponent<Rigidbody>().AddForce( throwForce );
    }
}
