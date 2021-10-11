using System.Collections.Generic;
using UnityEngine;

public class MainMenuCharacter : MonoBehaviour, IInitializable
{
	private enum ControlMode
	{
		Tank,
		Direct
	}

	[SerializeField]
	private float m_moveSpeed = 2f;

	[SerializeField]
	private float m_turnSpeed = 200f;

	[SerializeField]
	private float m_jumpForce = 4f;

	[SerializeField]
	private Animator m_animator;

	[SerializeField]
	private Rigidbody m_rigidBody;

	[SerializeField]
	private ControlMode m_controlMode = ControlMode.Direct;

	private float m_currentV;

	private float m_currentH;

	private readonly float m_interpolation = 10f;

	private readonly float m_walkScale = 0.33f;

	private readonly float m_backwardsWalkScale = 0.16f;

	private readonly float m_backwardRunScale = 0.66f;

	private bool m_wasGrounded;

	private Vector3 m_currentDirection = Vector3.zero;

	private float m_jumpTimeStamp;

	private float m_minJumpInterval = 0.25f;

	private bool m_isGrounded;

	private List<Collider> m_collisions = new List<Collider>();

	public void Initialize(GameObject character)
	{
		m_animator = character.GetComponent<Animator>();
		m_rigidBody = character.GetComponent<Rigidbody>();
	}

	private void Awake()
	{
		if (!m_animator)
		{
			base.gameObject.GetComponent<Animator>();
		}
		if (!m_rigidBody)
		{
			base.gameObject.GetComponent<Animator>();
		}
		m_isGrounded = true;
	}

	private void FixedUpdate()
	{
		m_animator.SetBool("Grounded", m_isGrounded);
		DirectUpdate();
		m_wasGrounded = m_isGrounded;
	}

	private void DirectUpdate()
	{
		m_animator.SetFloat("MoveSpeed", 0f);
		JumpingAndLanding();
	}

	private void JumpingAndLanding()
	{
		if (Time.time - m_jumpTimeStamp >= m_minJumpInterval && m_isGrounded && UnityEngine.Input.GetKey(KeyCode.Space))
		{
			m_jumpTimeStamp = Time.time;
			m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
		}
		if (!m_wasGrounded && m_isGrounded)
		{
			m_animator.SetTrigger("Land");
		}
		if (!m_isGrounded && m_wasGrounded)
		{
			m_animator.SetTrigger("Jump");
		}
	}
}
