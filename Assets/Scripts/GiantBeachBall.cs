using UnityEngine;

public class GiantBeachBall : Hazard
{
	[SerializeField]
	private float rotateSpeed = 2f;

	[SerializeField]
	private float startingT;

	protected override void Start()
	{
		base.Start();
		GameObject pooledObject = Pool.Instance.GetPooledObject("DangerLine");
		Vector3 position = pooledObject.transform.position;
		position.z = base.transform.position.z;
		pooledObject.transform.position = position;
		pooledObject.SetActive(value: true);
	}

	protected override void Update()
	{
		base.Update();
		Vector3 position = base.transform.position;
		position.x = -1.75f + Mathf.PingPong(startingT + Time.time * rotateSpeed, 3.5f);
		base.transform.eulerAngles = new Vector3(-150f + Mathf.PingPong(startingT * 72f + Time.time * 72f * rotateSpeed, 360f), 90f, 0f);
		base.transform.position = position;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Character component = other.gameObject.GetComponent<Character>();
			if (component.IsAbleToMove())
			{
				component.PreventMoving();
				component.OnDying();
			}
		}
	}
}
