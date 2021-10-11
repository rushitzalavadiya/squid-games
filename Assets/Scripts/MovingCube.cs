using UnityEngine;

public class MovingCube : Hazard
{
	public Animator animator;

	private Vector3 previousPosition;

	private Vector3 currentPosition;

	public float startingT;

	protected override void Start()
	{
		base.Start();
		previousPosition = base.transform.position;
		currentPosition = base.transform.position;
		if (canBeReversed)
		{
			animator.Play("ReverseMove", 0, startingT);
		}
		else
		{
			animator.Play("Move", 0, startingT);
		}
	}

	protected override void Update()
	{
		base.Update();
		previousPosition = currentPosition;
		currentPosition = base.transform.position;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Player"))
		{
			Character component = other.transform.GetComponent<Character>();
			if (!component.HasLost() && !component.IsInvincible())
			{
				Vector3 a = currentPosition - previousPosition;
				a.Normalize();
				component.PreventMoving();
				component.AddForce(a * 250f, shouldResetVelocity: false);
				component.OnDying();
			}
		}
	}
}
