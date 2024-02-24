using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( Collider ))]
    public class GuidancePoint : MonoBehaviour
    {
        GuidancePointsManager m_manager;

        void Awake() => GetComponent<Collider>().isTrigger = true;

        public void SetOwner(GuidancePointsManager manager) => m_manager = manager;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                m_manager.NextPoint();
                gameObject.SetActive( false );
            }
        }
    }
}
