using UnityEngine;

public class VerticalWaterGun : Hazard
{
	private void OnParticleCollision(GameObject other)
	{
		if (other.CompareTag("Player"))
		{
			Character component = other.GetComponent<Character>();
			if (component.IsAbleToMove())
			{
				component.PreventMoving();
				component.OnDying();
			}
		}
	}
}
