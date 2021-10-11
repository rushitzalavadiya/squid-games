using UnityEngine;

public class StarGlitter : MonoBehaviour
{
	public ParticleSystem particles;

	private int starIndex;

	private Vector3 startingPosition;

	private Vector3 destination;

	private float timeToReachDestination = 0.75f;

	private float currentT;

	private void OnEnable()
	{
		particles.Play();
	}

	private void Update()
	{
		currentT += Time.deltaTime / timeToReachDestination;
		Vector3 position = Vector3.Lerp(startingPosition, destination, currentT);
		base.transform.position = position;
		if (currentT > timeToReachDestination)
		{
			StopGlittering();
		}
	}

	public void Init(Vector3 position, int index)
	{
		starIndex = index;
		startingPosition = base.transform.position;
		destination = position;
		currentT = 0f;
	}

	private void StopGlittering()
	{
		particles.Stop();
		EventManager.Instance.QueueEvent(new StarTouchedUIEvent(starIndex));
		base.enabled = false;
	}
}
