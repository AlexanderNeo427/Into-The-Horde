using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IntoTheHorde
{
    public class SurvivorScript : MonoBehaviour
    {
        Camera       m_mainCam;
        Animator     m_animator;
        NavMeshAgent m_navAgent;
        InputReader  m_inputReader;

        float m_navAgentDesiredSpeed;

        void Awake()
        {
            m_mainCam     = Camera.main;
            m_animator    = GetComponent<Animator>();
            m_navAgent    = GetComponent<NavMeshAgent>();
            m_inputReader = GetComponent<InputReader>();
        }

        void Update()
        {
            Vector3 localDesiredVelocity = transform.InverseTransformVector( m_navAgent.desiredVelocity );

            float speed     = localDesiredVelocity.magnitude;
            float velocityX = localDesiredVelocity.x;
            float velocityZ = localDesiredVelocity.z;

            m_animator.SetFloat("f_speed"    , speed    , 0.1f, Time.deltaTime);
            m_animator.SetFloat("f_velocityX", velocityX, 0.1f, Time.deltaTime);
            m_animator.SetFloat("f_velocityZ", velocityZ, 0.1f, Time.deltaTime);
        }

        void OnAnimatorMove()
        {
            m_navAgentDesiredSpeed = m_animator.deltaPosition.magnitude / Time.deltaTime;
        }
    }
}
