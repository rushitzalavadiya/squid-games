using System.Collections;
using UnityEngine;

public class WindyZone : Hazard
{
	[SerializeField]
	private float calmWindTime;

	[SerializeField]
	private float strongWindTime;

	[Header("Particles")]
	[SerializeField]
	private ParticleSystem calmWindParticle;

	[SerializeField]
	private ParticleSystem strongWindParticle;

	private bool isStrongWind;

	protected override void Start()
	{
		base.Start();
		calmWindParticle.Play();
		StartCoroutine(CalmWind());
	}

	private IEnumerator CalmWind()
	{
		strongWindParticle.Stop();
		isStrongWind = false;
		yield return new WaitForSeconds(calmWindTime);
		StartCoroutine(StrongWind());
	}

	private IEnumerator StrongWind()
	{
		strongWindParticle.Play();
		yield return new WaitForSeconds(1f);
		calmWindParticle.gameObject.SetActive(value: false);
		calmWindParticle.gameObject.SetActive(value: true);
		calmWindParticle.Play();
		isStrongWind = true;
		yield return new WaitForSeconds(strongWindTime);
		StartCoroutine(CalmWind());
	}

	protected override void OnTriggerEnter(Collider other)
	{
		if (isStrongWind)
		{
			other.CompareTag("Player");
		}
	}
}
