using UnityEngine;
using System.Collections;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputReader))]
    public class CameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform _player;

        [Header("Customisations")]
        [SerializeField] float _lookSensitivity = 50f;

        [SerializeField] [Range(0.0f, 0.8f)]
        float _smoothTime = 0.01f;

        InputReader m_inputReader;
        Vector2     m_currDelta;
        Vector2     m_currDeltaVelocity;
        float       m_rotationX;

        public float LookSensitivity
        {
            get => _lookSensitivity;
            set => _lookSensitivity = value;
        }

        void Awake()
        {
            m_inputReader = GetComponent<InputReader>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }

        void Update()
        {
            m_currDelta = Vector2.SmoothDamp(m_currDelta, m_inputReader.LookInput, ref m_currDeltaVelocity, _smoothTime);

            m_rotationX -= m_currDelta.y * _lookSensitivity;
            m_rotationX = Mathf.Clamp(m_rotationX, -87f, 87f);

            transform.localEulerAngles = Vector3.right * m_rotationX;
            _player.Rotate(Vector3.up * m_currDelta.x * _lookSensitivity);
        }

        public void AddRecoil(Vector2 recoilAmount)
        {
            m_rotationX -= recoilAmount.y;
            _player.Rotate( Vector3.up * recoilAmount.x );
        }

        IEnumerator RecoilCoroutine(Vector2 recoilAmount)
        {
            float timeElapsed = 0f;
            float recoilTime = 0.1f;

            float rotationX = m_rotationX;
            float rotationY = _player.transform.rotation.y;

            float targetX = m_rotationX - recoilAmount.y;

            Transform tmp = _player.transform;
            tmp.Rotate( Vector3.up * recoilAmount.x );
            float targetY = tmp.rotation.y;

            while (timeElapsed <= recoilTime)
            {
                timeElapsed += Time.deltaTime;

                float ratio = timeElapsed / recoilTime;
                float newX = Mathf.Lerp( rotationX, targetX, ratio );
                float newY = Mathf.Lerp( rotationY, targetY, ratio );

                m_rotationX = newX;
                _player.Rotate( Vector3.up * newY );

                yield return null;
            }

            yield return null;
        }
    }
}
