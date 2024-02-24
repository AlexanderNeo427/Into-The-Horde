using UnityEngine;
using TMPro;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class ScrollingText : MonoBehaviour
    {
        public static readonly float DESPAWN_Y = -600f;

        public float ScrollSpeed = 5f;
        RectTransform m_transform;

        void Awake() => m_transform = GetComponent<RectTransform>();

        void Update()
        {
            m_transform.position -= Vector3.up * ScrollSpeed * Time.deltaTime;

            if (m_transform.position.y < DESPAWN_Y)
                Destroy( this.gameObject );
        }
    }
}
