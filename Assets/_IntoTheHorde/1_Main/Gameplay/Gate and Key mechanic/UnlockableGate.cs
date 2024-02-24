using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( BoxCollider ), typeof( Rigidbody ))]
    public class UnlockableGate : MonoBehaviour
    {
        [SerializeField] Key _key;

        void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        void OnCollisionEnter(Collision collision)
        {
            Key key = collision.gameObject.GetComponent<Key>();

            if (key != null && key == _key)
            {
                Destroy( key );
                Destroy( this );
            }
        }
    }
}
