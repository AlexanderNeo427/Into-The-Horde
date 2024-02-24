using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    [RequireComponent(typeof( InputReader ))]
    public class ClickToMove : MonoBehaviour
    {
        [SerializeField] NavMeshAgent _navAgent;

        Vector3     m_lastPos;
        Camera      m_mainCam;
        InputReader m_inputReader;

        void Awake()
        {
            m_mainCam = Camera.main;
            m_inputReader = GetComponent<InputReader>();
        }

        void Update()
        {
            if (m_inputReader.Fire1Pressed)
            {
                Ray ray = m_mainCam.ScreenPointToRay( Input.mousePosition );

                if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
                {
                    m_lastPos = hitInfo.point;
                    _navAgent.SetDestination( hitInfo.point );
                }
            }
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere( m_lastPos, 0.8f );
        }
    }
}
