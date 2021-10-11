using System.Collections;
using UnityEngine;

public class HeliceRotate : MonoBehaviour
{
	public float rotateSpeed;

	public float startingT;

	private bool canRotate;

	private void Start()
	{
		canRotate = true;
		EventManager.Instance.AddListener<CounterIsTurningEvent>(OnCounterTurning);
	}

	private void Update()
	{
		if (canRotate)
		{
			base.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
		}
	}

	private void OnCounterTurning(CounterIsTurningEvent e)
	{
		if (e.isTurningTowardPlayers)
		{
			canRotate = false;
		}
		else
		{
			StartCoroutine(StartRotatingAgain());
		}
	}

	private IEnumerator StartRotatingAgain()
	{
		yield return new WaitForSeconds(1f);
		canRotate = true;
	}
}
