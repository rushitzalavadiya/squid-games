using UnityEngine;

public class Jumper : Hazard
{
	[SerializeField]
	private float distance;

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (other.CompareTag("Player"))
		{
			other.GetComponent<Character>().JumperJump(distance);
		}
	}
}
