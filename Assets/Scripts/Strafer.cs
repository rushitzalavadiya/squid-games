using System.Collections.Generic;
using UnityEngine;

public class Strafer : MonoBehaviour, IInitializable
{
	[SerializeField]
	private float m_moveSpeed = 2f;

	[SerializeField]
	private float m_jumpForce = 4f;

	[SerializeField]
	private Animator m_animator;

	[SerializeField]
	private Rigidbody m_rigidBody;

	private float m_currentV;

	private float m_currentH;

	private readonly float m_interpolation = 10f;

	private readonly float m_walkScale = 0.33f;

	private Vector3 m_currentDirection = Vector3.zero;

	private float m_jumpTimeStamp;

	private float m_minJumpInterval = 0.25f;

	private bool m_isGrounded;

	private List<Collider> m_collisions = new List<Collider>();

	public Animator Animator
	{
		set
		{
			m_animator = value;
		}
	}

	public Rigidbody Rigidbody
	{
		set
		{
			m_rigidBody = value;
		}
	}

	public bool IsDead
	{
		get;
		set;
	}

	public bool IsZombie
	{
		get;
		set;
	}

	public void Initialize(GameObject character)
	{
		m_animator = character.GetComponent<Animator>();
		m_rigidBody = character.GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		ContactPoint[] contacts = collision.contacts;
		for (int i = 0; i < contacts.Length; i++)
		{
			if (Vector3.Dot(contacts[i].normal, Vector3.up) > 0.5f)
			{
				if (!m_collisions.Contains(collision.collider))
				{
					m_collisions.Add(collision.collider);
				}
				m_isGrounded = true;
			}
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		ContactPoint[] contacts = collision.contacts;
		bool flag = false;
		for (int i = 0; i < contacts.Length; i++)
		{
			if (Vector3.Dot(contacts[i].normal, Vector3.up) > 0.5f)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			m_isGrounded = true;
			if (!m_collisions.Contains(collision.collider))
			{
				m_collisions.Add(collision.collider);
			}
			return;
		}
		if (m_collisions.Contains(collision.collider))
		{
			m_collisions.Remove(collision.collider);
		}
		if (m_collisions.Count == 0)
		{
			m_isGrounded = false;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (m_collisions.Contains(collision.collider))
		{
			m_collisions.Remove(collision.collider);
		}
		if (m_collisions.Count == 0)
		{
			m_isGrounded = false;
		}
	}

	private void FixedUpdate()
	{
		m_animator.SetBool("Grounded", m_isGrounded);
		if (!IsDead)
		{
			DirectUpdate();
		}
	}

	private void DirectUpdate()
	{
		float num = UnityEngine.Input.GetAxis("Vertical");
		float num2 = UnityEngine.Input.GetAxis("Horizontal");
		Transform transform = Camera.main.transform;
		if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
		{
			num *= m_walkScale;
			num2 *= m_walkScale;
		}
		m_currentV = Mathf.Lerp(m_currentV, num, Time.deltaTime * m_interpolation);
		m_currentH = Mathf.Lerp(m_currentH, num2, Time.deltaTime * m_interpolation);
		Vector3 vector = transform.right * m_currentH + transform.forward * m_currentV;
		float magnitude = vector.magnitude;
		vector.y = 0f;
		vector = vector.normalized * magnitude;
		if (vector != Vector3.zero)
		{
			m_currentDirection = Vector3.Slerp(m_currentDirection, vector, Time.deltaTime * m_interpolation);
			m_rigidBody.MovePosition(m_rigidBody.position + m_currentDirection * m_moveSpeed * Time.deltaTime);
		}
		Vector3 vector2 = Quaternion.Inverse(base.transform.rotation) * vector;
		m_animator.SetFloat("MoveHorizontal", vector2.x);
		m_animator.SetFloat("MoveVertical", vector2.z);
		if (Time.time - m_jumpTimeStamp >= m_minJumpInterval && m_isGrounded && UnityEngine.Input.GetKey(KeyCode.Space) && !IsZombie)
		{
			m_jumpTimeStamp = Time.time;
			m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
		}
	}
}
