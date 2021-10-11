using UnityEngine;

public class FlameWall : Hazard
{
	[SerializeField]
	private float timeBetweenTwoFlameWalls = 3f;

	private float currentTimeBetweenTwoFlameWalls;

	[SerializeField]
	private ParticleSystem flameWall;

	[SerializeField]
	private BoxCollider flameCollider;

	private bool fireIsOn;

	protected override void Start()
	{
		base.Start();
		LightOffWall();
	}

	protected override void Update()
	{
		base.Update();
		if (fireIsOn)
		{
			if (!flameWall.isPlaying)
			{
				LightOffWall();
			}
			return;
		}
		currentTimeBetweenTwoFlameWalls += Time.deltaTime;
		if (!playerShouldStop && currentTimeBetweenTwoFlameWalls > timeBetweenTwoFlameWalls - 1f)
		{
			playerShouldStop = true;
			for (int i = 0; i < charactersInHazard.Count; i++)
			{
				charactersInHazard[i].AddHazardToRunFrom(this);
			}
		}
		if (currentTimeBetweenTwoFlameWalls >= timeBetweenTwoFlameWalls)
		{
			LightUpWall();
		}
	}

	private void LightUpWall()
	{
		flameWall.Play();
		flameCollider.enabled = true;
		fireIsOn = true;
	}

	private void LightOffWall()
	{
		flameCollider.enabled = false;
		playerShouldStop = false;
		for (int i = 0; i < charactersInHazard.Count; i++)
		{
			charactersInHazard[i].RevoveHazardToStopFrom(this);
		}
		currentTimeBetweenTwoFlameWalls = 0f;
		fireIsOn = false;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<Character>().OnDying();
		}
	}
}
