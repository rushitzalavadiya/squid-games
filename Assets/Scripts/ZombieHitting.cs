using UnityEngine;

public class ZombieHitting : MonoBehaviour, IInitializable
{
	public enum InputButtonType
	{
		Mouse,
		Key,
		Button
	}

	[SerializeField]
	private Animator m_animator;

	[Header("Hitting")]
	[SerializeField]
	private InputButtonType m_hitButtonType;

	[SerializeField]
	private int m_hitMouseButton;

	[SerializeField]
	private string m_hitButton;

	[SerializeField]
	private KeyCode m_hitKey;

	private bool m_currentHit;

	private bool m_previousHit;

	public bool IsDead
	{
		get;
		set;
	}

	private void Update()
	{
		if (!IsDead)
		{
			HitUpdate();
		}
	}

	private void HitUpdate()
	{
		switch (m_hitButtonType)
		{
		case InputButtonType.Mouse:
			m_currentHit = Input.GetMouseButton(m_hitMouseButton);
			break;
		case InputButtonType.Button:
			m_currentHit = Input.GetButton(m_hitButton);
			break;
		case InputButtonType.Key:
			m_currentHit = UnityEngine.Input.GetKey(m_hitKey);
			break;
		}
		if (m_currentHit && !m_previousHit)
		{
			m_animator.SetTrigger("WeaponFire");
		}
		m_previousHit = m_currentHit;
		m_currentHit = false;
	}

	private void Awake()
	{
		Initialize(base.gameObject);
	}

	public void Initialize(GameObject character)
	{
		m_animator = character.GetComponent<Animator>();
	}
}
