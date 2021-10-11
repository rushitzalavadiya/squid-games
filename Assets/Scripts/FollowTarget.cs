using UnityEngine;

public class FollowTarget : MonoBehaviour
{
	public Transform target;

	public Vector3 offset;

	public bool shouldFaceCamera;

	private Transform cameraTransform;

	private void Awake()
	{
		cameraTransform = Camera.main.transform;
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		if (target != null)
		{
			base.transform.position = target.position + offset;
			if (shouldFaceCamera)
			{
				base.transform.rotation = Quaternion.LookRotation(base.transform.position - cameraTransform.position);
			}
		}
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public void ModifyOffset(Vector3 newOffset)
	{
		offset = newOffset;
	}
}
