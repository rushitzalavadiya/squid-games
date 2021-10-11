using System;
using UnityEngine;

[Serializable]
public class ObjectPoolItem
{
	public GameObject objectToPool;

	public int amountToPool;

	public bool shouldExpand;
}
