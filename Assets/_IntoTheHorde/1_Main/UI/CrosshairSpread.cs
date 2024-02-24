using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( RectTransform ))]
    public class CrosshairSpread : MonoBehaviour
    {
        [SerializeField] float _defaultSize = 15f;

        Vector3       m_screenCenter; // Cache
        Camera        m_mainCam;
        RectTransform m_rect;

        // Set this variable internally, the crosshair will
        // continually update toward this target size
        float m_targetSize;

        public float Spread => m_rect.rect.width;

        void Awake()
        {
            m_screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
            m_mainCam      = Camera.main;
            m_rect         = GetComponent<RectTransform>();
        }

        void Start() => m_rect.sizeDelta = new Vector2(_defaultSize, _defaultSize);

        public void SetCrosshairSpread(Vector3 bulletPos, Vector3 bulletDir, float gunDist)
        {
            Vector3 bulletEndPos = bulletPos + bulletDir * gunDist;
            Vector3 screenPoint = m_mainCam.WorldToScreenPoint( bulletEndPos );
            screenPoint.z = 0.0f;

            float deviation = Vector3.Distance( m_screenCenter, screenPoint );
            m_rect.sizeDelta = new Vector2( deviation, deviation );
        }

        public float GetCrosshairSpread(Vector3 bulletPos, Vector3 bulletDir, float gunDist)
        {
            Vector3 bulletEndPos = bulletPos + bulletDir * gunDist;
            Vector3 screenPoint = m_mainCam.WorldToScreenPoint( bulletEndPos );
            screenPoint.z = 0.0f;

            return Vector3.Distance( m_screenCenter, screenPoint );
        }

        public void SetCrosshairSpread(float spread) => m_rect.sizeDelta = new Vector2( spread, spread );

        public void SetToDefaultSpread() => m_rect.sizeDelta = new Vector2(_defaultSize, _defaultSize);
    }
}
