using UnityEngine;

public class Aimer : MonoBehaviour, IInitializable
{
	private enum AimMode
	{
		Free,
		Horizontal
	}

	[SerializeField]
	private AimMode m_mode;

	[SerializeField]
	private float m_aimAngle;

	[SerializeField]
	private Vector3 m_aimDir;

	[SerializeField]
	private Vector3 m_aimPos;

	[SerializeField]
	private Vector3 m_hitNormal;

	[SerializeField]
	private Animator m_animator;

	[SerializeField]
	private Transform m_aimPoint;

	[SerializeField]
	private bool m_gunInHand;

	private int m_aimAnimLayerNum;

	private int m_weaponFireAnimLayerNum;

	private int m_gunType;

	public Animator Animator
	{
		set
		{
			m_animator = value;
		}
	}

	public Transform AimPoint
	{
		set
		{
			m_aimPoint = value;
		}
	}

	public Vector3 AimDir => m_aimDir;

	public void Initialize(GameObject character)
	{
		m_animator = character.GetComponent<Animator>();
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

	private void Awake()
	{
		m_aimAnimLayerNum = m_animator.GetLayerIndex("AimOverride");
		m_weaponFireAnimLayerNum = m_animator.GetLayerIndex("WeaponFireAdditive");
	}

	public void SetGunInHand(bool value, int gunType)
	{
		m_gunInHand = value;
		if (m_gunInHand)
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
		m_gunType = gunType;
	}

	private void Update()
	{
		switch (m_gunType)
		{
		case 0:
			if (Input.GetMouseButton(0))
			{
				m_animator.SetTrigger("WeaponFire");
			}
			break;
		case 1:
		case 2:
			if (Input.GetMouseButtonDown(0))
			{
				m_animator.SetTrigger("WeaponFire");
			}
			break;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.R))
		{
			m_animator.SetTrigger("WeaponReload");
		}
	}

	private void FixedUpdate()
	{
		Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
		if (new Plane(Camera.main.transform.forward, base.transform.position + Camera.main.transform.forward * 20f).Raycast(ray, out float enter))
		{
			m_aimPos = ray.GetPoint(enter);
		}
		if (Physics.Raycast(ray, out RaycastHit hitInfo, enter))
		{
			m_aimPos = hitInfo.point;
			m_hitNormal = hitInfo.normal;
		}
		m_aimDir = Vector3.Normalize(m_aimPos - m_aimPoint.position);
		switch (m_mode)
		{
		case AimMode.Horizontal:
			HorizontalAimUpdate(hitInfo);
			break;
		case AimMode.Free:
			FreeAimUpdate(hitInfo);
			break;
		default:
			UnityEngine.Debug.LogError("Enum not supported");
			break;
		}
		m_animator.SetFloat("AimAngle", m_aimAngle);
	}

	private void HorizontalAimUpdate(RaycastHit hitInfo)
	{
		m_aimDir.y = 0f;
		Quaternion quaternion = Quaternion.LookRotation(m_aimDir);
		base.transform.rotation = Quaternion.Euler(0f, quaternion.eulerAngles.y, 0f);
		m_aimAngle = 0.45f;
	}

	private void FreeAimUpdate(RaycastHit hitInfo)
	{
		Quaternion quaternion = Quaternion.LookRotation(m_aimDir);
		base.transform.rotation = Quaternion.Euler(0f, quaternion.eulerAngles.y, 0f);
		m_aimAngle = quaternion.eulerAngles.x;
		if (m_aimAngle > 90f)
		{
			m_aimAngle -= 360f;
		}
		m_aimAngle = 1f - (m_aimAngle + 90f) / 180f;
	}

	private void DrawPlane(Vector3 position, Vector3 normal)
	{
		Vector3 vector = (!(normal.normalized != Vector3.forward)) ? (Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude) : (Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude);
		Vector3 vector2 = position + vector;
		Vector3 vector3 = position - vector;
		vector = Quaternion.AngleAxis(90f, normal) * vector;
		Vector3 vector4 = position + vector;
		Vector3 vector5 = position - vector;
		Gizmos.color = Color.green;
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector4, vector5);
		Gizmos.DrawLine(vector2, vector4);
		Gizmos.DrawLine(vector4, vector3);
		Gizmos.DrawLine(vector3, vector5);
		Gizmos.DrawLine(vector5, vector2);
		Gizmos.color = Color.red;
		Gizmos.DrawRay(position, normal);
	}

	private void OnDrawGizmos()
	{
		DrawPlane(base.transform.position + Camera.main.transform.forward * 5f, Camera.main.transform.forward);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(m_aimPoint.position, m_aimPos);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(m_aimPos, m_aimPos + m_hitNormal);
	}
}
