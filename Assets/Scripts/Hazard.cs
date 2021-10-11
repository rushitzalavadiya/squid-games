using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
	[SerializeField]
	protected bool playerShouldRun;

	[SerializeField]
	protected bool playerShouldStop;

	protected List<AIController> charactersInHazard = new List<AIController>();

	[SerializeField]
	protected bool canBeReversed;

	[SerializeField]
	protected MeshRenderer objectRenderer;

	[SerializeField]
	protected bool shouldBeHiddenIfCameraInFront;

	[SerializeField]
	private float distanceToDeactivate = 5f;

	private bool isHidden;

	private Transform mainCameraPosition;

	protected virtual void Start()
	{
		if (shouldBeHiddenIfCameraInFront)
		{
			mainCameraPosition = Camera.main.transform;
			EventManager.Instance.AddListener<GameFinishedEvent>(OnGameEnding);
		}
	}

	protected virtual void Update()
	{
		if (shouldBeHiddenIfCameraInFront)
		{
			float num = base.transform.position.z - mainCameraPosition.position.z;
			if (!isHidden && num <= distanceToDeactivate)
			{
				Hide();
			}
			else if (isHidden && num > distanceToDeactivate)
			{
				Unhide();
			}
		}
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if ((!playerShouldRun && !playerShouldStop) || !other.CompareTag("Player"))
		{
			return;
		}
		AIController component = other.GetComponent<AIController>();
		if (component != null)
		{
			charactersInHazard.Add(component);
			if (playerShouldRun)
			{
				component.AddHazardToRunFrom(this);
			}
			if (playerShouldStop)
			{
				component.AddHazardToStopFrom(this);
			}
		}
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Player"))
		{
			return;
		}
		AIController component = other.GetComponent<AIController>();
		if (component != null)
		{
			charactersInHazard.Remove(component);
			if (playerShouldRun)
			{
				component.RevoveHazardToRunFrom(this);
			}
			if (playerShouldStop)
			{
				component.RevoveHazardToStopFrom(this);
			}
		}
	}

	public void ReverseObject()
	{
		if (canBeReversed)
		{
			base.transform.position = new Vector3(0f - base.transform.position.x, base.transform.position.y, base.transform.position.z);
			Vector3 eulerAngles = base.transform.eulerAngles;
			eulerAngles.y *= -1f;
			base.transform.eulerAngles = eulerAngles;
		}
	}

	public bool CanBeReversed()
	{
		return canBeReversed;
	}

	private void OnGameEnding(GameFinishedEvent e)
	{
		if (shouldBeHiddenIfCameraInFront)
		{
			shouldBeHiddenIfCameraInFront = false;
			Unhide();
		}
	}

	private void Hide()
	{
		objectRenderer.enabled = false;
		isHidden = true;
	}

	private void Unhide()
	{
		objectRenderer.enabled = true;
		isHidden = false;
	}
}
