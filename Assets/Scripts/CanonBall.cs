using UnityEngine;

public class CanonBall : Hazard
{
	private Vector3 direction;

	private Rigidbody ownRigidbody;

	public float duration = 5f;

	private float currentDuration;

	private void Awake()
	{
		ownRigidbody = GetComponent<Rigidbody>();
	}

	public void Init(Vector3 direct)
	{
		direction = direct;
		ownRigidbody.velocity = direction * 5f;
		currentDuration = 0f;
	}

	protected override void Update()
	{
		base.Update();
		currentDuration += Time.deltaTime;
		if (currentDuration >= duration)
		{
			base.gameObject.SetActive(value: false);
		}
		ownRigidbody.velocity = direction * 5f;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Player"))
		{
			Character component = other.transform.GetComponent<Character>();
			if (!component.HasLost() && !component.IsInvincible())
			{
				direction.Normalize();
				component.PreventMoving();
				component.AddForce(direction * 250f, shouldResetVelocity: false);
				component.OnDying();
			}
		}
	}
}
