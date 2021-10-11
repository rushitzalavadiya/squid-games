using System.Collections;
using UnityEngine;

public class Hammer : Hazard
{
	public Animator animator;

	public SpriteRenderer hitLocationSprite;

	public ParticleSystem hitParticle;

	public float startingTime;

	public Color WhiteColor;

	public Color redColor;

	private float hammerFireRate = 4.5f;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(HammerTime());
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

	private IEnumerator HammerTime()
	{
		float currentT = startingTime * hammerFireRate;
		animator.Play("Smash", 0, startingTime);
		while (true)
		{
			if (currentT < 2f)
			{
				hitLocationSprite.color = redColor;
			}
			while (currentT < 2f)
			{
				currentT += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			if (currentT < 2.16f)
			{
				hitParticle.Play();
			}
			while (currentT < 2.16f)
			{
				currentT += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			hitLocationSprite.color = WhiteColor;
			while (currentT < 4.5f)
			{
				currentT += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			currentT = 0f;
			animator.Play("Smash", 0, 0f);
		}
	}
}
