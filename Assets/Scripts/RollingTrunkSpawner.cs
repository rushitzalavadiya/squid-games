using UnityEngine;

public class RollingTrunkSpawner : MonoBehaviour
{
	private Pool pool;

	public float fireRate;

	private float currentT;

	private bool counterIsWatching;

	private void Start()
	{
		pool = Pool.Instance;
		currentT = 0f;
		EventManager.Instance.AddListener<CounterIsTurningEvent>(OnCounterTurning);
		SpawnTrunk();
	}

	private void Update()
	{
		if (!counterIsWatching)
		{
			currentT += Time.deltaTime;
			if (currentT > fireRate)
			{
				SpawnTrunk();
			}
		}
	}

	private void SpawnTrunk()
	{
		GameObject pooledObject = pool.GetPooledObject("RollingTrunk");
		pooledObject.transform.position = base.transform.position;
		pooledObject.SetActive(value: true);
		currentT = 0f;
	}

	private void OnCounterTurning(CounterIsTurningEvent e)
	{
		counterIsWatching = e.isTurningTowardPlayers;
	}
}
