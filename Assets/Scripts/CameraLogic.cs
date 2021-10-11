using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
	private Transform m_currentTarget;

	private float m_distance = 2f;

	private float m_height = 1f;

	private float m_lookAtAroundAngle = 180f;

	[SerializeField]
	private List<Transform> m_targets;

	private int m_currentIndex;

	private void Start()
	{
		if (m_targets.Count > 0)
		{
			m_currentIndex = 0;
			m_currentTarget = m_targets[m_currentIndex];
		}
	}

	private void SwitchTarget(int step)
	{
		if (m_targets.Count != 0)
		{
			m_currentIndex += step;
			if (m_currentIndex > m_targets.Count - 1)
			{
				m_currentIndex = 0;
			}
			if (m_currentIndex < 0)
			{
				m_currentIndex = m_targets.Count - 1;
			}
			m_currentTarget = m_targets[m_currentIndex];
		}
	}

	public void NextTarget()
	{
		SwitchTarget(1);
	}

	public void PreviousTarget()
	{
		SwitchTarget(-1);
	}

	private void Update()
	{
		int count = m_targets.Count;
	}

	private void LateUpdate()
	{
		if (!(m_currentTarget == null))
		{
			float y = m_currentTarget.position.y + m_height;
			float lookAtAroundAngle = m_lookAtAroundAngle;
			Quaternion rotation = Quaternion.Euler(0f, lookAtAroundAngle, 0f);
			Vector3 vector = m_currentTarget.position;
			vector -= rotation * Vector3.forward * m_distance;
			vector.y = y;
			base.transform.position = vector;
			base.transform.LookAt(m_currentTarget.position + new Vector3(0f, m_height, 0f));
		}
	}
}
