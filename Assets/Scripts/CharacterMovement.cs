using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour, IInitializable
{
	public enum InputButtonType
	{
		Mouse,
		Key,
		Button
	}

	[Header("Movement")]
	[SerializeField]
	private string m_horizontalAxis = "Horizontal";

	[SerializeField]
	private string m_verticalAxis = "Vertical";

	[SerializeField]
	private Transform m_relativeTo;

	[Header("Walking")]
	[SerializeField]
	private InputButtonType m_walkButtonType = InputButtonType.Key;

	[SerializeField]
	private int m_walkMouseButton;

	[SerializeField]
	private string m_walkButton;

	[SerializeField]
	private KeyCode m_walkKey = KeyCode.LeftShift;

	[Header("Crouching")]
	[SerializeField]
	private InputButtonType m_crouchButtonType = InputButtonType.Key;

	[SerializeField]
	private int m_crouchMouseButton;

	[SerializeField]
	private string m_crouchButton;

	[SerializeField]
	private KeyCode m_crouchKey = KeyCode.LeftControl;

	[Header("Prone")]
	[SerializeField]
	private InputButtonType m_proneButtonType = InputButtonType.Key;

	[SerializeField]
	private int m_proneMouseButton;

	[SerializeField]
	private string m_proneButton;

	[SerializeField]
	private KeyCode m_proneKey = KeyCode.Z;

	[Header("Jumping")]
	[SerializeField]
	private InputButtonType m_jumpButtonType = InputButtonType.Key;

	[SerializeField]
	private int m_jumpMouseButton;

	[SerializeField]
	private string m_jumpButton;

	[SerializeField]
	private KeyCode m_jumpKey = KeyCode.Space;

	[Header("Movement Variables")]
	[SerializeField]
	private float m_moveSpeed = 2f;

	[SerializeField]
	private float m_jumpForce = 4f;

	[SerializeField]
	private float m_minJumpInterval = 0.25f;

	[Header("Components")]
	[SerializeField]
	private CharacterMovementAnimator m_movementAnimator;

	[SerializeField]
	private CharacterStanceAnimator m_stanceAnimator;

	[SerializeField]
	private Rigidbody m_rigidbody;

	private float m_currentV;

	private float m_currentH;

	private Vector3 m_currentMovement = Vector3.zero;

	private float m_jumpTimestamp;

	private readonly float m_interpolation = 10f;

	private readonly float m_walkScale = 0.33f;

	private List<Collider> m_collisions = new List<Collider>();

	private bool m_isGrounded;

	private bool m_isWalking;

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
		if (m_movementAnimator == null)
		{
			m_movementAnimator = character.GetComponent<CharacterMovementAnimator>();
		}
		if (m_stanceAnimator == null)
		{
			m_stanceAnimator = character.GetComponent<CharacterStanceAnimator>();
		}
		if (m_rigidbody == null)
		{
			m_rigidbody = character.GetComponent<Rigidbody>();
		}
		if (m_relativeTo == null)
		{
			m_relativeTo = base.transform;
		}
	}

	private void Awake()
	{
		Initialize(base.gameObject);
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
		m_movementAnimator.SetGrounded(m_isGrounded);
		if (!IsDead)
		{
			WalkUpdate();
			MovementUpdate();
			CrouchUpdate();
			ProneUpdate();
			JumpUpdate();
		}
	}

	private void MovementUpdate()
	{
		float num = UnityEngine.Input.GetAxis(m_verticalAxis);
		float num2 = UnityEngine.Input.GetAxis(m_horizontalAxis);
		if (m_isWalking)
		{
			num *= m_walkScale;
			num2 *= m_walkScale;
		}
		switch (m_stanceAnimator.CurrentStance)
		{
		case CharacterStanceAnimator.StanceState.Crouching:
			num *= 0.5f;
			num2 *= 0.5f;
			break;
		case CharacterStanceAnimator.StanceState.Prone:
			num *= 0.25f;
			num2 *= 0.25f;
			break;
		}
		m_currentV = Mathf.Lerp(m_currentV, num, Time.fixedDeltaTime * m_interpolation);
		m_currentH = Mathf.Lerp(m_currentH, num2, Time.fixedDeltaTime * m_interpolation);
		Vector3 forward = Vector3.forward;
		Vector3 right = Vector3.right;
		if (m_relativeTo != null)
		{
			forward = m_relativeTo.forward;
			forward.y = 0f;
			forward.Normalize();
			right = m_relativeTo.right;
			right.y = 0f;
			right.Normalize();
		}
		Vector3 vector = right * m_currentH + forward * m_currentV;
		if (vector != Vector3.zero)
		{
			m_currentMovement = Vector3.Slerp(m_currentMovement, vector, Time.fixedDeltaTime * m_interpolation);
			m_rigidbody.MovePosition(m_rigidbody.position + m_currentMovement * m_moveSpeed * Time.fixedDeltaTime);
		}
		m_movementAnimator.SetMovement(vector);
	}

	private void WalkUpdate()
	{
		m_isWalking = false;
		switch (m_walkButtonType)
		{
		case InputButtonType.Mouse:
			m_isWalking = Input.GetMouseButton(m_walkMouseButton);
			break;
		case InputButtonType.Button:
			m_isWalking = Input.GetButton(m_walkButton);
			break;
		case InputButtonType.Key:
			m_isWalking = UnityEngine.Input.GetKey(m_walkKey);
			break;
		}
	}

	private void CrouchUpdate()
	{
		bool flag = false;
		switch (m_crouchButtonType)
		{
		case InputButtonType.Mouse:
			flag = Input.GetMouseButtonDown(m_crouchMouseButton);
			break;
		case InputButtonType.Button:
			flag = Input.GetButtonDown(m_crouchButton);
			break;
		case InputButtonType.Key:
			flag = UnityEngine.Input.GetKeyDown(m_crouchKey);
			break;
		}
		if (flag)
		{
			switch (m_stanceAnimator.CurrentStance)
			{
			case CharacterStanceAnimator.StanceState.Standing:
			case CharacterStanceAnimator.StanceState.Prone:
				m_stanceAnimator.Crouch();
				break;
			case CharacterStanceAnimator.StanceState.Crouching:
				m_stanceAnimator.Stand();
				break;
			}
		}
	}

	private void ProneUpdate()
	{
		bool flag = false;
		switch (m_proneButtonType)
		{
		case InputButtonType.Mouse:
			flag = Input.GetMouseButtonDown(m_proneMouseButton);
			break;
		case InputButtonType.Button:
			flag = Input.GetButtonDown(m_proneButton);
			break;
		case InputButtonType.Key:
			flag = UnityEngine.Input.GetKeyDown(m_proneKey);
			break;
		}
		if (flag)
		{
			switch (m_stanceAnimator.CurrentStance)
			{
			case CharacterStanceAnimator.StanceState.Standing:
			case CharacterStanceAnimator.StanceState.Crouching:
				m_stanceAnimator.Prone();
				break;
			case CharacterStanceAnimator.StanceState.Prone:
				m_stanceAnimator.Stand();
				break;
			}
		}
	}

	private void JumpUpdate()
	{
		bool flag = false;
		switch (m_jumpButtonType)
		{
		case InputButtonType.Mouse:
			flag = Input.GetMouseButton(m_jumpMouseButton);
			break;
		case InputButtonType.Button:
			flag = Input.GetButton(m_jumpButton);
			break;
		case InputButtonType.Key:
			flag = UnityEngine.Input.GetKey(m_jumpKey);
			break;
		}
		if (flag && m_stanceAnimator.CurrentStance == CharacterStanceAnimator.StanceState.Standing && !IsZombie && Time.fixedTime - m_jumpTimestamp >= m_minJumpInterval && m_isGrounded)
		{
			m_jumpTimestamp = Time.fixedTime;
			m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
		}
	}
}
