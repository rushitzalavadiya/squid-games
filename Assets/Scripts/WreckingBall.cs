using UnityEngine;

public class WreckingBall : Hazard
{
	public Animator animator;

	public float startingTime;

	protected override void Start()
	{
		base.Start();
		animator.Play("Wreck", 0, startingTime);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Player"))
		{
			Character component = other.transform.GetComponent<Character>();
			if (!component.HasLost() && !component.IsInvincible())
			{
				Vector3 a = other.contacts[0].point - base.transform.position;
				a.y = 0f;
				a.Normalize();
				component.PreventMoving();
				component.AddForce(a * 350f, shouldResetVelocity: false);
				component.OnDying();
			}
		}
	}
}
