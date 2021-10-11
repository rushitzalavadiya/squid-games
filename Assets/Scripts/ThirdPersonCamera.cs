using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	[SerializeField]
	private GameObject m_character;

	[SerializeField]
	private float m_lookDistance = 10f;

	[SerializeField]
	private float m_depthOffset = -3f;

	[SerializeField]
	private float m_verticalOffset = 2f;

	[SerializeField]
	private float m_horizontalOffset = -2f;

	[SerializeField]
	private float m_rotationSpeed = 90f;

	private CharacterWeaponAnimator m_weaponAnimator;

	private Quaternion m_currentRotation;

	private Quaternion m_targetRotation;

	private Vector3 m_lookPosition;

	private Vector3 m_cameraPosition;

	public void ChangeCharacter(GameObject character)
	{
		m_character = character;
		m_weaponAnimator = character.GetComponent<CharacterWeaponAnimator>();
		UpdateCamera();
	}

	private void Awake()
	{
		m_weaponAnimator = m_character.GetComponent<CharacterWeaponAnimator>();
		UpdateCamera();
	}

	private void LateUpdate()
	{
		UpdateCamera();
	}

	private void UpdateCamera()
	{
		UpdateTargetPosition();
		UpdateCameraPosition();
		Vector3 normalized = (m_lookPosition - m_cameraPosition).normalized;
		m_targetRotation = Quaternion.LookRotation(normalized);
		m_currentRotation = Quaternion.Slerp(m_currentRotation, m_targetRotation, m_rotationSpeed * Time.deltaTime);
		base.transform.rotation = m_currentRotation;
	}

	private void UpdateTargetPosition()
	{
		m_lookPosition = m_character.transform.position + m_weaponAnimator.AimDirection * m_lookDistance;
	}

	private void UpdateCameraPosition()
	{
		Vector3 normalized = m_weaponAnimator.AimDirection.normalized;
		Vector3 a = Vector3.Cross(Vector3.up, normalized);
		Vector3 up = Vector3.up;
		m_cameraPosition = m_character.transform.position + normalized * m_depthOffset + a * m_horizontalOffset + up * m_verticalOffset;
		base.transform.position = m_cameraPosition;
	}
}
