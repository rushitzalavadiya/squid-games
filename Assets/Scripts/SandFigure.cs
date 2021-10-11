using UnityEngine;

public class SandFigure : MonoBehaviour
{
	private Pool pool;

	[SerializeField]
	private Material sandMaterial;

	private void Start()
	{
		pool = Pool.Instance;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Player"))
		{
			GameObject pooledObject = pool.GetPooledObject("SandHit");
			pooledObject.transform.position = base.transform.position;
			pooledObject.SetActive(value: true);
			ParticleSystem component = pooledObject.GetComponent<ParticleSystem>();
			var componentMain = component.main;
			componentMain.startColor = sandMaterial.color;
			component.Play();
			base.gameObject.SetActive(value: false);
		}
	}
}
