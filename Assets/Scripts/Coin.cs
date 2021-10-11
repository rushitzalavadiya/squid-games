using UnityEngine;

public class Coin : MonoBehaviour
{
	private GameManager gameManager;

	private ParticleSystem coinPickUp;

	private void Start()
	{
		gameManager = GameManager.Instance;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (other.GetComponent<Character>().GetPlayerId() == 0)
			{
				gameManager.AddCoinToPlayer();
			}
			GameObject pooledObject = Pool.Instance.GetPooledObject("CoinPickUp");
			pooledObject.transform.position = base.transform.position;
			pooledObject.SetActive(value: true);
			pooledObject.GetComponent<ParticleSystem>().Play();
			base.gameObject.SetActive(value: false);
		}
	}
}
