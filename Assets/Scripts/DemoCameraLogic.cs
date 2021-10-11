using System.Collections.Generic;
using UnityEngine;

public class DemoCameraLogic : MonoBehaviour
{
	private Transform m_currentTarget;

	private float m_distance = 4f;

	private float m_height = 1f;

	[SerializeField]
	private List<Transform> m_targets;

	private int m_currentIndex;

	private Vector3 m_correctPosition;

	private Quaternion m_correctRotation;

	[SerializeField]
	private ThirdPersonCamera m_cameraController;

	private void ToggleRenderers(Transform trans, bool enabled = true)
	{
		Renderer[] componentsInChildren = trans.GetComponentsInChildren<Renderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = enabled;
		}
		trans.gameObject.GetComponent<CapsuleCollider>().enabled = enabled;
		Rigidbody component = trans.gameObject.GetComponent<Rigidbody>();
		component.useGravity = enabled;
		component.isKinematic = !enabled;
	}

	private void ToggleTarget(GameObject gameObject, bool enabled = true)
	{
		gameObject.SetActive(enabled);
	}

	private void Start()
	{
		if (m_targets.Count > 0)
		{
			m_currentIndex = 0;
			for (int i = 0; i < m_targets.Count; i++)
			{
				ToggleRenderers(m_targets[i], enabled: false);
			}
			m_currentTarget = m_targets[m_currentIndex];
			ToggleRenderers(m_currentTarget);
		}
	}

	public void PreviousTarget()
	{
		m_correctPosition = m_targets[m_currentIndex].position;
		m_correctRotation = m_targets[m_currentIndex].rotation;
		ChangeTarget(-1);
	}

	public void NextTarget()
	{
		m_correctPosition = m_targets[m_currentIndex].position;
		m_correctRotation = m_targets[m_currentIndex].rotation;
		ChangeTarget(1);
	}

	private void ChangeTarget(int step)
	{
		m_currentIndex += step;
		if (m_currentIndex == m_targets.Count)
		{
			m_currentIndex = 0;
		}
		else if (m_currentIndex < 0)
		{
			m_currentIndex = m_targets.Count - 1;
		}
		for (int i = 0; i < m_targets.Count; i++)
		{
			ToggleRenderers(m_targets[i], enabled: false);
		}
		m_currentTarget = m_targets[m_currentIndex];
		m_currentTarget.position = m_correctPosition;
		m_currentTarget.rotation = m_correctRotation;
		ToggleRenderers(m_currentTarget);
		m_cameraController.ChangeCharacter(m_currentTarget.gameObject);
	}

	private void LateUpdate()
	{
		if (!(m_currentTarget == null))
		{
			float y = m_currentTarget.position.y + m_height;
			Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
			Vector3 vector = m_currentTarget.position;
			vector -= rotation * Vector3.forward * m_distance;
			vector.y = y;
			base.transform.position = vector;
			base.transform.LookAt(m_currentTarget.position + new Vector3(0f, m_height, 0f));
			base.transform.Rotate(new Vector3(0f, 0f, 0f));
		}
	}
}
