using UnityEngine;

public class DemoControls : MonoBehaviour, IInitializable
{
	[SerializeField]
	private Animator m_animator;

	[SerializeField]
	private DeadStateController m_deadState;

	[SerializeField]
	private ZombieStateController m_zombieState;

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.T) && m_zombieState != null)
		{
			m_zombieState.IsZombie = !m_zombieState.IsZombie;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Y) && m_deadState != null)
		{
			m_deadState.IsDead = !m_deadState.IsDead;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.F) && m_animator != null)
		{
			m_animator.SetTrigger("TakingHit" + UnityEngine.Random.Range(1, 3));
		}
	}

	public void Initialize(GameObject character)
	{
		if (m_animator == null)
		{
			m_animator = character.GetComponent<Animator>();
		}
		if (m_deadState == null)
		{
			m_deadState = character.GetComponent<DeadStateController>();
		}
		if (m_zombieState == null)
		{
			m_zombieState = character.GetComponent<ZombieStateController>();
		}
	}

	private void Awake()
	{
		Initialize(base.gameObject);
	}
}
