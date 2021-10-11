using UnityEngine;

public class Croucher : MonoBehaviour, IInitializable
{
	private enum MoveState
	{
		Standing,
		Crouching,
		Prone
	}

	private MoveState m_moveState;

	[SerializeField]
	private Animator m_animator;

	public bool IsStanding => m_moveState == MoveState.Standing;

	public Animator Animator
	{
		set
		{
			m_animator = value;
		}
	}

	public void Initialize(GameObject character)
	{
		m_animator = character.GetComponent<Animator>();
	}

	public void Stand()
	{
		m_moveState = MoveState.Standing;
		m_animator.SetInteger("MoveState", 0);
	}

	public void Crouch()
	{
		m_moveState = MoveState.Crouching;
		m_animator.SetInteger("MoveState", 1);
	}

	public void Prone()
	{
		m_moveState = MoveState.Prone;
		m_animator.SetInteger("MoveState", 2);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftControl))
		{
			switch (m_moveState)
			{
			case MoveState.Standing:
			case MoveState.Prone:
				Crouch();
				break;
			case MoveState.Crouching:
				Stand();
				break;
			}
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
		{
			switch (m_moveState)
			{
			case MoveState.Prone:
				Stand();
				break;
			case MoveState.Standing:
			case MoveState.Crouching:
				Prone();
				break;
			}
		}
	}
}
