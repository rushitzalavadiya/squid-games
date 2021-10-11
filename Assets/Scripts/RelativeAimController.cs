using UnityEngine;

public class RelativeAimController : MonoBehaviour, IInitializable
{
	public enum InputButtonType
	{
		Mouse,
		Key,
		Button
	}

	[Header("Aiming")]
	[SerializeField]
	private float m_aimSpeed = 100f;

	[SerializeField]
	private string m_horizontalAxis = "Mouse X";

	[SerializeField]
	private string m_verticalAxis = "Mouse Y";

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

	private CharacterWeaponAnimator m_animator;

	private Transform m_aimPoint;

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
					m_animator.SetGunInHand(value: false, m_animator.GunType);
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
		m_animator.SetAimDirection(base.transform.forward);
	}

	private void LateUpdate()
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
		float axis = UnityEngine.Input.GetAxis(m_horizontalAxis);
		float num = 0f - UnityEngine.Input.GetAxis(m_verticalAxis);
		Vector3 aimDirection = m_animator.AimDirection;
		aimDirection = Quaternion.AngleAxis(axis * m_aimSpeed * Time.fixedDeltaTime, Vector3.up) * aimDirection;
		float num2 = Vector3.Dot(aimDirection, Vector3.up);
		bool flag = true;
		if (num2 > 0.95f && num < 0f)
		{
			flag = false;
		}
		if (num2 < -0.95f && num > 0f)
		{
			flag = false;
		}
		if (flag)
		{
			aimDirection = Quaternion.AngleAxis(num * m_aimSpeed * Time.fixedDeltaTime, Vector3.Cross(Vector3.up, aimDirection)) * aimDirection;
		}
		m_animator.SetAimDirection(aimDirection);
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
