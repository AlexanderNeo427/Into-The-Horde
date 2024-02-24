using System.Collections;
using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class DoorInteraction : Interactable
    {
        [Header("References")]
        [SerializeField] GameObject _doorHinge;

        [SerializeField] float _doorOpeningDuration = 0.8f;
        [SerializeField] float _doorOpenAngle       = 100.0f;
        [SerializeField] bool  _doorStartsOpen      = true;
        [SerializeField] bool  _opensOutward        = true;

        Quaternion m_openRotation;
        Quaternion m_closedRotation;
        bool       m_animIsActive;
        bool       m_doorIsOpen;

        public bool IsOpen => m_doorIsOpen;

        void Start()
        {
            m_closedRotation = _doorHinge.transform.rotation;

            if (_opensOutward)
                m_openRotation = _doorHinge.transform.rotation * Quaternion.Euler(0f, -_doorOpenAngle, 0f);
            else
                m_openRotation = _doorHinge.transform.rotation * Quaternion.Euler(0f, _doorOpenAngle, 0f);

            m_animIsActive = false;
            m_doorIsOpen   = false;

            if (_doorStartsOpen)
            {
                _doorHinge.transform.rotation = m_openRotation;
                m_doorIsOpen = true;
            }
        }

        public override void OnInteract(InteractionSystem interactionSystem)
        {
            if (!m_animIsActive)
                StartCoroutine(ToggleDoor());
        }

        IEnumerator ToggleDoor()
        {
            m_animIsActive = true;
            Quaternion currRotation   = _doorHinge.transform.rotation;
            Quaternion targetRotation = m_doorIsOpen ? m_closedRotation : m_openRotation;

            float timeElapsed = 0f;
            while (timeElapsed <= _doorOpeningDuration)
            {
                timeElapsed += Time.deltaTime;

                float t = timeElapsed / _doorOpeningDuration;
                Quaternion newRotation = Quaternion.Lerp( currRotation, targetRotation, t );
                _doorHinge.transform.rotation = newRotation;

                yield return null;
            }

            m_doorIsOpen = !m_doorIsOpen;
            switch ( m_doorIsOpen )
            {
                case true:  EventManager.RaiseEvent(GameEvent.OnDoorOpened, new DoorOpenedEventArgs( this )); break;
                case false: EventManager.RaiseEvent(GameEvent.OnDoorClosed, new DoorClosedEventArgs( this )); break;
            }
            m_animIsActive = false;
            yield return null;
        }
    }
}
