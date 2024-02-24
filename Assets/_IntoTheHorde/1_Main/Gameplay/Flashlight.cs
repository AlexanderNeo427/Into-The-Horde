using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( Light ), typeof( InputReader ))]
    public class Flashlight : MonoBehaviour
    {
        bool        m_isOn = false;
        Light       m_light;
        InputReader m_inputReader;

        void Awake()
        {
            m_light         = GetComponent<Light>();
            m_inputReader   = GetComponent<InputReader>();
            m_light.enabled = m_isOn;
        }

        void Update()
        {
            if (m_inputReader.FlashlightPressed)
            {
                m_isOn          = !m_isOn;
                m_light.enabled = m_isOn;
            }
        }
    }
}
