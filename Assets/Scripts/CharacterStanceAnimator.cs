using UnityEngine;

public class CharacterStanceAnimator : MonoBehaviour, IInitializable
{
	public enum StanceState
	{
		Standing,
		Crouching,
		Prone
	}

	private StanceState m_stanceState;

	[SerializeField]
	private Animator m_animator;

	[SerializeField]
	private CharacterWeaponAnimator m_weaponAnimator;

	[SerializeField]
	private RelativeAimController m_aimController;

	public Animator Animator
	{
		set
		{
			m_animator = value;
		}
	}

	public StanceState CurrentStance => m_stanceState;

	public void Initialize(GameObject character)
	{
		if (m_animator == null)
		{
			m_animator = character.GetComponent<Animator>();
		}
		if (m_weaponAnimator == null)
		{
			m_weaponAnimator = GetComponent<CharacterWeaponAnimator>();
		}
		if (m_aimController == null)
		{
			m_aimController = GetComponent<RelativeAimController>();
		}
	}

	public void Stand()
	{
		m_stanceState = StanceState.Standing;
		m_animator.SetInteger("MoveState", (int)m_stanceState);
	}

	public void Crouch()
	{
		m_stanceState = StanceState.Crouching;
		m_animator.SetInteger("MoveState", (int)m_stanceState);
	}

	public void Prone()
	{
		m_stanceState = StanceState.Prone;
		m_animator.SetInteger("MoveState", (int)m_stanceState);
	}

	public void Jump()
	{
		m_animator.SetTrigger("Jump");
	}

	private void Awake()
	{
		Initialize(base.gameObject);
	}
}
