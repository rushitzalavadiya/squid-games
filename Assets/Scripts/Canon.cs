using UnityEngine;

public class Canon : Hazard
{
	private Pool pool;

	public Animator animator;

	public float fireRate;

	public Transform firePoint;

	public float startingT;

	private float currentT;

	protected override void Start()
	{
		base.Start();
		pool = Pool.Instance;
		currentT = startingT;
	}

	protected override void Update()
	{
		base.Update();
		currentT += Time.deltaTime;
		if (currentT > fireRate)
		{
			currentT = 0f;
			animator.Play("Shoot", 0, 0f);
			FireNewBall();
		}
	}

	private void FireNewBall()
	{
		GameObject pooledObject = pool.GetPooledObject("CanonBall");
		pooledObject.transform.position = firePoint.position;
		pooledObject.SetActive(value: true);
		pooledObject.GetComponent<CanonBall>().Init((base.transform.eulerAngles.y == 0f) ? Vector3.left : Vector3.right);
	}
}
