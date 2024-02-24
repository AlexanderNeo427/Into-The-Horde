using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class Billboard : MonoBehaviour
    {
        [SerializeField] float _xOffset = 0f;
        [SerializeField] float _yOffset = 0f;
        [SerializeField] float _zOffset = 0f;

        Transform m_mainCam;

        void Awake() => m_mainCam = Camera.main.transform;

        void LateUpdate()
        {
            transform.LookAt( m_mainCam.position );
            transform.rotation *= Quaternion.Euler(_xOffset, _yOffset, _zOffset);
        }
    }
}
