using UnityEngine;

public class BoxingGlove : MonoBehaviour
{
	public Animator animator;

	public float startingTime;

	private Vector3 direction;

	private void Awake()
	{
		animator.Play("Punch", 0, startingTime);
		if (base.transform.parent.eulerAngles.y == 0f)
		{
			direction = Vector3.right * 500f;
		}
		else
		{
			direction = Vector3.left * 500f;
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Player"))
		{
			Character component = other.transform.GetComponent<Character>();
			if (!component.HasLost() && !component.IsInvincible())
			{
				component.PreventMoving();
				component.AddForce(direction, shouldResetVelocity: false);
				component.OnDying();
			}
		}
	}
}
