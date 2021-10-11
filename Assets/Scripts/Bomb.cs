using System.Collections;
using UnityEngine;

public class Bomb : Hazard
{
	private bool isActivated;

	[SerializeField]
	private int timeBeforeExplosion = 3;

	private Pool pool;

	protected override void Start()
	{
		base.Start();
		isActivated = false;
		pool = Pool.Instance;
	}

	protected override void Update()
	{
		base.Update();
	}

	protected void OnCollisionEnter(Collision other)
	{
		other.gameObject.CompareTag("Player");
	}

	protected override void OnTriggerEnter(Collider other)
	{
		if (!isActivated)
		{
			isActivated = true;
			StartCoroutine(ExplosionProcess());
		}
	}

	private IEnumerator ExplosionProcess()
	{
		for (int currentTimeBeforeExplosion = timeBeforeExplosion; currentTimeBeforeExplosion != 0; currentTimeBeforeExplosion--)
		{
			yield return new WaitForSeconds(1f);
		}
		Explode();
	}

	private void Explode()
	{
		GameObject pooledObject = pool.GetPooledObject("BombExplosion");
		pooledObject.transform.position = new Vector3(base.transform.position.x + 0.25f, base.transform.position.y - 0.4f, 0f);
		pooledObject.SetActive(value: true);
		pooledObject.GetComponent<ParticleSystem>().Play();
		base.gameObject.SetActive(value: false);
	}
}
