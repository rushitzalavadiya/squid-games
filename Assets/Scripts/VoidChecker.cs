using UnityEngine;

public class VoidChecker : MonoBehaviour
{
	private Pool pool;

	private void Start()
	{
		pool = Pool.Instance;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Character component = other.GetComponent<Character>();
			GameObject pooledObject = pool.GetPooledObject("WaterSplash");
			Vector3 position = component.transform.position;
			pooledObject.transform.position = new Vector3(position.x, pooledObject.transform.position.y, position.z);
			pooledObject.SetActive(value: true);
			pooledObject.GetComponent<ParticleSystem>().Play();
			component.OnDying();
		}
	}
}
