using UnityEngine;

public class CharacterMovementAnimator : MonoBehaviour, IInitializable
{
	[SerializeField]
	private Animator m_animator;

	public Animator Animator
	{
		set
		{
			m_animator = value;
		}
	}

	public void Initialize(GameObject character)
	{
		if (m_animator == null)
		{
			m_animator = character.GetComponent<Animator>();
		}
	}

	private void Awake()
	{
		Initialize(base.gameObject);
	}

	public void SetMovement(Vector3 movement)
	{
		Vector3 vector = Quaternion.Inverse(base.transform.rotation) * movement;
		m_animator.SetFloat("MoveHorizontal", vector.x);
		m_animator.SetFloat("MoveVertical", vector.z);
	}

	public void SetGrounded(bool value)
	{
		m_animator.SetBool("Grounded", value);
	}
}
