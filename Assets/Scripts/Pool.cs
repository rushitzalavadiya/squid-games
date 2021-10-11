using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
	public static Pool Instance;

	public List<ObjectPoolItem> itemsToPool;

	public List<GameObject> pooledObjects;

	private void Awake()
	{
		Instance = this;
		pooledObjects = new List<GameObject>();
		foreach (ObjectPoolItem item in itemsToPool)
		{
			for (int i = 0; i < item.amountToPool; i++)
			{
				CreateNewObject(item);
			}
		}
	}

	private void Start()
	{
	}

	public GameObject GetPooledObject(string objectToPoolName)
	{
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].name == objectToPoolName)
			{
				return pooledObjects[i];
			}
		}
		foreach (ObjectPoolItem item in itemsToPool)
		{
			if (item.objectToPool.name == objectToPoolName && item.shouldExpand)
			{
				return CreateNewObject(item);
			}
		}
		return null;
	}

	private GameObject CreateNewObject(ObjectPoolItem item)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(item.objectToPool);
		gameObject.name = gameObject.name.Replace("(Clone)", "");
		gameObject.SetActive(value: false);
		pooledObjects.Add(gameObject);
		return gameObject;
	}
}
