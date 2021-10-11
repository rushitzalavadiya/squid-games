using System.Collections;
using UnityEngine;

public class FallingRock : Hazard
{
	public Animator animator;

	public float startingTime;

	public ParticleSystem hitParticle;

	public SpriteRenderer hitLocationSprite;

	public Color WhiteColor;

	public Color redColor;

	private float fallingRockFireRate = 3f;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(FallingTime());
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Player"))
		{
			Character component = other.transform.GetComponent<Character>();
			if (!component.HasLost() && !component.IsInvincible())
			{
				component.PreventMoving();
				component.OnDying();
			}
		}
	}

	private IEnumerator FallingTime()
	{
		float currentT = startingTime * fallingRockFireRate;
		animator.Play("Fall", 0, startingTime);
		while (true)
		{
			if (currentT < 0.33f)
			{
				hitLocationSprite.color = redColor;
			}
			while (currentT < 0.33f)
			{
				currentT += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			if (currentT < 0.5f)
			{
				hitParticle.Play();
			}
			while (currentT < 0.5f)
			{
				currentT += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			hitLocationSprite.color = WhiteColor;
			while (currentT < 2.4f)
			{
				currentT += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			hitLocationSprite.color = redColor;
			while (currentT < 3f)
			{
				currentT += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			currentT = 0f;
			animator.Play("Fall", 0, 0f);
		}
	}
}
