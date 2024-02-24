using System;
using UnityEngine;

namespace IntoTheHorde
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof( InputReader ))]
    public class MovementController : MonoBehaviour
    {
		[Header("Ground check")]
		[SerializeField] Transform _groundCheck;
		[SerializeField] LayerMask _groundMask;

		[Header("Movement")]
		[SerializeField] float _gravity			 = 14f;
		[SerializeField] float _moveSpeed		 = 6f;
		[SerializeField] float _sprintMultiplier = 2f;
		[SerializeField] float _jumpHeight		 = 2.2f;

		CharacterController m_controller;
		InputReader			m_inputReader;
		Vector3				m_movement;
		Vector3				m_velocity;
		Vector3				m_jumpVelocity;
		bool				m_isGrounded;

		GameActor m_actor;
		bool	  m_isEnabled = true;

		void Awake()
		{
			m_actor		  = GetComponent<GameActor>();
			m_controller  = GetComponent<CharacterController>();
			m_inputReader = GetComponent<InputReader>();
		}

		void OnEnable()
		{
			EventManager.AddListener(GameEvent.OnHealingBegin,	 HealingBeginHandler  );
			EventManager.AddListener(GameEvent.OnHealingEnd,	 HealingEndHandler	  );
			EventManager.AddListener(GameEvent.OnHealingSuccess, HealingSuccessHandler);
		}

		void OnDisable()
		{
			EventManager.RemoveListener(GameEvent.OnHealingBegin,	HealingBeginHandler	 );
			EventManager.RemoveListener(GameEvent.OnHealingEnd,		HealingEndHandler	 );
			EventManager.RemoveListener(GameEvent.OnHealingSuccess, HealingSuccessHandler);
		}

		void Start() => m_jumpVelocity.y = Mathf.Sqrt(_jumpHeight * 2f * _gravity);

		void Update()
		{
			m_isGrounded = Physics.CheckSphere(_groundCheck.position, 0.4f, _groundMask);

			if (m_isEnabled)
				m_movement = transform.right * m_inputReader.MoveInput.x + transform.forward * m_inputReader.MoveInput.y;
			else
				m_movement = Vector3.zero;

			if (m_isGrounded)
			{
				if (m_velocity.y < 0f)
					m_velocity.y = -2f;

				if (m_inputReader.SprintPressed || m_inputReader.SprintHeld)
					m_movement *= _sprintMultiplier;

				if (m_inputReader.JumpPressed)
					m_velocity = m_jumpVelocity;
			}
			else
				m_velocity.y -= _gravity * Time.deltaTime;

			m_controller.Move((m_velocity + m_movement * _moveSpeed) * Time.deltaTime);
		}

		void HealingBeginHandler(EventArgs eventArgs)
        {
			HealingBeginEventArgs args = (HealingBeginEventArgs)eventArgs;

			if (args.Actor == this.m_actor)
				m_isEnabled = false;
        }

		void HealingEndHandler(EventArgs eventArgs)
		{
			HealingEndEventArgs args = (HealingEndEventArgs)eventArgs;

			if (args.Actor == this.m_actor)
				m_isEnabled = true;
		}

		void HealingSuccessHandler(EventArgs eventArgs)
		{
			HealingSuccessEventArgs args = (HealingSuccessEventArgs)eventArgs;

			if (args.Actor == this.m_actor)
				m_isEnabled = true;
		}
	}
}
