using UnityEngine;

public class DirectAimController : MonoBehaviour, IInitializable
{
	public enum InputButtonType
	{
		Mouse,
		Key,
		Button
	}

	private CharacterWeaponAnimator m_animator;

	private Transform m_aimPoint;

	[Header("Shooting")]
	[SerializeField]
	private InputButtonType m_shootButtonType;

	[SerializeField]
	private int m_shootMouseButton;

	[SerializeField]
	private string m_shootButton;

	[SerializeField]
	private KeyCode m_shootKey;

	[Header("Reloading")]
	[SerializeField]
	private InputButtonType m_reloadButtonType = InputButtonType.Key;

	[SerializeField]
	private int m_reloadMouseButton;

	[SerializeField]
	private string m_reloadButton;

	[SerializeField]
	private KeyCode m_reloadKey = KeyCode.R;

	private bool m_weaponInHand;

	private bool m_isDead;

	public bool IsDead
	{
		set
		{
			m_isDead = value;
			if (m_animator != null)
			{
				if (m_isDead)
				{
					m_animator.SetGunInHand(value: false, -1);
				}
				else
				{
					m_animator.SetGunInHand(value: true, m_animator.GunType);
				}
			}
		}
	}

	public void Initialize(GameObject character)
	{
		InitializeAnimator(character);
		InitializeAimPoint(character);
	}

	private void Awake()
	{
		Initialize(base.gameObject);
	}

	private void Update()
	{
		if (!m_isDead)
		{
			ShootUpdate();
			ReloadUpdate();
			AimUpdate();
		}
	}

	private void ShootUpdate()
	{
		bool flag = false;
		switch (m_shootButtonType)
		{
		case InputButtonType.Mouse:
			flag = Input.GetMouseButton(m_shootMouseButton);
			break;
		case InputButtonType.Button:
			flag = Input.GetButton(m_shootButton);
			break;
		case InputButtonType.Key:
			flag = UnityEngine.Input.GetKey(m_shootKey);
			break;
		}
		if (flag)
		{
			m_animator.Shoot();
		}
	}

	private void ReloadUpdate()
	{
		bool flag = false;
		switch (m_reloadButtonType)
		{
		case InputButtonType.Mouse:
			flag = Input.GetMouseButtonDown(m_reloadMouseButton);
			break;
		case InputButtonType.Button:
			flag = Input.GetButtonDown(m_reloadButton);
			break;
		case InputButtonType.Key:
			flag = UnityEngine.Input.GetKeyDown(m_reloadKey);
			break;
		}
		if (flag)
		{
			m_animator.Reload();
		}
	}

	private void AimUpdate()
	{
		Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
		Plane plane = new Plane(Camera.main.transform.forward, base.transform.position + Camera.main.transform.forward * 20f);
		Vector3? vector = null;
		if (plane.Raycast(ray, out float enter))
		{
			vector = ray.GetPoint(enter);
		}
		if (vector.HasValue)
		{
			m_animator.SetAimDirection(Vector3.Normalize(vector.Value - m_aimPoint.position));
		}
	}

	private void InitializeAnimator(GameObject character)
	{
		if (!(m_animator != null))
		{
			m_animator = character.GetComponent<CharacterWeaponAnimator>();
		}
	}

	private void InitializeAimPoint(GameObject character)
	{
		if (!(m_aimPoint != null))
		{
			AimPoint aimPoint = character.GetComponentInChildren<AimPoint>();
			if (!aimPoint)
			{
				GameObject gameObject = new GameObject("AimPoint");
				aimPoint = gameObject.AddComponent<AimPoint>();
				gameObject.transform.parent = character.transform;
				gameObject.transform.localPosition = new Vector3(0f, 0.5f, 0f);
			}
			m_aimPoint = aimPoint.transform;
		}
	}
}
