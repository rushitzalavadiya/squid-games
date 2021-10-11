using UnityEngine;

public class Helice : Hazard
{
	private Rigidbody ownRigidbody;

	private void Awake()
	{
		ownRigidbody = GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Character component = other.gameObject.GetComponent<Character>();
			if (component.IsAbleToMove())
			{
				component.PreventMoving();
				component.AddForce(ownRigidbody.velocity * 100f, shouldResetVelocity: false);
				component.OnDying();
			}
		}
	}
}
