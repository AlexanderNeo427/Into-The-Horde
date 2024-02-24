using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace IntoTheHorde
{
    public class ChangeColorOnHover : MonoBehaviour,
                                      IPointerEnterHandler,
                                      IPointerExitHandler
    {
        [SerializeField] TMP_Text _textElement;
        [SerializeField] Color    _hoverColor = Color.red;

        Color m_originalColor;

        void Awake() => m_originalColor = _textElement.color;

        void OnEnable() => _textElement.color = m_originalColor;

        public void OnPointerEnter(PointerEventData eventData) => _textElement.color = _hoverColor;

        public void OnPointerExit(PointerEventData eventData) => _textElement.color = m_originalColor;
    }
}
