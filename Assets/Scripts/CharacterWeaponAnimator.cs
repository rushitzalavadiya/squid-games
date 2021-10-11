using UnityEngine;

public class CharacterWeaponAnimator : MonoBehaviour, IInitializable
{
	public enum AimMode
	{
		Free,
		Horizontal
	}

	[SerializeField]
	private AimMode m_mode;

	[SerializeField]
	private Animator m_animator;

	private int m_aimAnimLayerNum;

	private int m_weaponFireAnimLayerNum;

	private int m_gunType = -1;

	private Vector3 m_aimDirection;

	private float m_verticalAimAngle;

	private bool m_currentShoot;

	private bool m_previousShoot;

	private bool m_currentReload;

	private bool m_previousReload;

	private bool m_gunInHand;

	public int GunType => m_gunType;

	public Vector3 AimDirection => m_aimDirection;

	public Animator Animator => m_animator;

	public void Initialize(GameObject character)
	{
		if (m_animator == null)
		{
			m_animator = character.GetComponent<Animator>();
		}
	}

	public void SetAimDirection(Vector3 aimDirection)
	{
		m_aimDirection = aimDirection;
		switch (m_mode)
		{
		case AimMode.Horizontal:
			HorizontalAimUpdate(m_aimDirection);
			break;
		case AimMode.Free:
			FreeAimUpdate(m_aimDirection);
			break;
		default:
			UnityEngine.Debug.LogError("Enum not supported", base.gameObject);
			break;
		}
		m_animator.SetFloat("AimAngle", m_verticalAimAngle);
	}

	public void Shoot()
	{
		m_currentShoot = true;
	}

	public void Reload()
	{
		m_currentReload = true;
	}

	private void Awake()
	{
		Initialize(base.gameObject);
		m_aimAnimLayerNum = m_animator.GetLayerIndex("AimOverride");
		m_weaponFireAnimLayerNum = m_animator.GetLayerIndex("WeaponFireAdditive");
	}

	public void SetGunInHand(bool value, int gunType)
	{
		m_gunInHand = value;
		m_gunType = gunType;
		if (m_gunInHand && m_gunType != -1)
		{
			m_animator.SetLayerWeight(m_aimAnimLayerNum, 1f);
			m_animator.SetLayerWeight(m_weaponFireAnimLayerNum, 1f);
			m_animator.SetInteger("GunType", gunType);
		}
		else
		{
			m_animator.SetLayerWeight(m_aimAnimLayerNum, 0f);
			m_animator.SetLayerWeight(m_weaponFireAnimLayerNum, 0f);
		}
	}

	private void Update()
	{
		switch (m_gunType)
		{
		case 0:
			if (m_currentShoot)
			{
				m_animator.SetTrigger("WeaponFire");
			}
			break;
		case -1:
		case 1:
		case 2:
			if (m_currentShoot && !m_previousShoot)
			{
				m_animator.SetTrigger("WeaponFire");
			}
			break;
		}
		if (m_currentReload && !m_previousReload)
		{
			m_animator.SetTrigger("WeaponReload");
		}
		m_previousShoot = m_currentShoot;
		m_currentShoot = false;
		m_previousReload = m_currentReload;
		m_currentReload = false;
	}

	private void HorizontalAimUpdate(Vector3 aimDirection)
	{
		Vector3 forward = aimDirection;
		forward.y = 0f;
		Quaternion quaternion = Quaternion.LookRotation(forward);
		base.transform.rotation = Quaternion.Euler(0f, quaternion.eulerAngles.y, 0f);
		m_verticalAimAngle = 0.45f;
	}

	private void FreeAimUpdate(Vector3 aimDirection)
	{
		Quaternion quaternion = Quaternion.LookRotation(aimDirection);
		base.transform.rotation = Quaternion.Euler(0f, quaternion.eulerAngles.y, 0f);
		m_verticalAimAngle = quaternion.eulerAngles.x;
		if (m_verticalAimAngle > 90f)
		{
			m_verticalAimAngle -= 360f;
		}
		m_verticalAimAngle = 1f - (m_verticalAimAngle + 90f) / 180f;
	}
}
