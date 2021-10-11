using UnityEngine;

public class RollingTrunk : Hazard
{
	private Rigidbody ownRigidbody;

	public Vector3 velocity;

	private bool counterIsWatching;

	private void Awake()
	{
		ownRigidbody = GetComponent<Rigidbody>();
		counterIsWatching = false;
	}

	protected override void Start()
	{
		base.Start();
		EventManager.Instance.AddListener<CounterIsTurningEvent>(OnCounterTurning);
	}

	private void FixedUpdate()
	{
		if (counterIsWatching)
		{
			ownRigidbody.velocity = Vector3.zero;
		}
		else if (base.transform.position.y <= 1.2f)
		{
			Vector3 vector = ownRigidbody.velocity;
			vector.x = 0f;
			vector.z = velocity.z;
			ownRigidbody.velocity = vector;
		}
		else if (base.transform.position.y <= -2f)
		{
			base.gameObject.SetActive(value: false);
		}
		base.transform.eulerAngles = new Vector3(0f, 0f, 90f);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Player"))
		{
			Vector3 a = velocity;
			a.Normalize();
			Character component = other.transform.GetComponent<Character>();
			if (!component.HasLost() && !component.IsInvincible())
			{
				component.PreventMoving();
				component.AddForce(a * 250f, shouldResetVelocity: false);
				component.OnDying();
			}
		}
	}

	private void OnCounterTurning(CounterIsTurningEvent e)
	{
		counterIsWatching = e.isTurningTowardPlayers;
		ownRigidbody.useGravity = !counterIsWatching;
	}
}
