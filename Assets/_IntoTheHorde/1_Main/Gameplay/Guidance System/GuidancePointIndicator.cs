using UnityEngine;
using TMPro;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class GuidancePointIndicator : MonoBehaviour
    {
        [SerializeField] RectTransform _canvas;
        [SerializeField] TMP_Text      _distanceText;

        Camera        m_mainCam;
        RectTransform m_rect;
        Vector2       m_UIOffset;

        void Awake()
        {
            m_mainCam  = Camera.main;
            m_rect     = GetComponent<RectTransform>();
        }

        void Start() => m_UIOffset = new Vector2(_canvas.sizeDelta.x * 0.5f, _canvas.sizeDelta.y * 0.5f);

        /*
         *  Converts the position to a point on the canvas
         */
        public void SetInfo(Vector3 worldPos, float remainingDistance)
        {
            Vector2 viewportPos = m_mainCam.WorldToViewportPoint( worldPos );
            Vector2 proportionalPos = new Vector2(viewportPos.x * _canvas.sizeDelta.x, 
                                                  viewportPos.y * _canvas.sizeDelta.y);

            m_rect.localPosition = proportionalPos - m_UIOffset;

            _distanceText.text = Mathf.Floor(remainingDistance).ToString() + "m";
        }
    }
}
