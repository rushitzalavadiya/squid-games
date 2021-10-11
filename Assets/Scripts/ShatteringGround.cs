using UnityEngine;

public class ShatteringGround : Hazard
{
	private Pool pool;

	[SerializeField]
	private float magnitude = 0.05f;

	[SerializeField]
	private float timeBeforeExploding = 2f;

	[SerializeField]
	private float timeBeforeAscending = 1f;

	private float currentTimeBeforeExploding;

	private float currentTimeBeforeAscending;

	[SerializeField]
	private Transform shatteredTop;

	private Vector3 startingPosition;

	private Vector3 shatteredTopStartingPosition;

	private bool isShattering;

	private bool isFalling;

	private bool isAscending;

	private void Awake()
	{
		isShattering = false;
		isFalling = false;
		isAscending = false;
		startingPosition = base.transform.position;
		shatteredTopStartingPosition = shatteredTop.localPosition;
	}

	protected override void Start()
	{
		base.Start();
		pool = Pool.Instance;
	}

	protected override void Update()
	{
		base.Update();
		if (isAscending)
		{
			Vector3 vector = base.transform.position + Vector3.up * 0.5f;
			if (vector.y >= startingPosition.y)
			{
				isAscending = false;
				vector.y = startingPosition.y;
			}
			base.transform.position = vector;
		}
		else if (isFalling)
		{
			currentTimeBeforeAscending += Time.deltaTime;
			if (currentTimeBeforeAscending >= timeBeforeAscending)
			{
				isFalling = false;
				isAscending = true;
			}
			base.transform.position -= Vector3.up * 0.1f;
		}
		else if (isShattering)
		{
			currentTimeBeforeExploding += Time.deltaTime;
			if (currentTimeBeforeExploding >= timeBeforeExploding)
			{
				FallingAppart();
			}
			else
			{
				Shake();
			}
		}
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (!isFalling)
		{
			StartShattering();
		}
	}

	private void StartShattering()
	{
		isShattering = true;
		currentTimeBeforeExploding = 0f;
	}

	private void Shake()
	{
		Vector3 localPosition = new Vector3(shatteredTopStartingPosition.x + Random.Range(-1f, 1f) * magnitude, shatteredTopStartingPosition.y, shatteredTopStartingPosition.z + Random.Range(-1f, 1f) * magnitude);
		shatteredTop.localPosition = localPosition;
	}

	private void FallingAppart()
	{
		shatteredTop.localPosition = shatteredTopStartingPosition;
		isShattering = false;
		isFalling = true;
		currentTimeBeforeAscending = 0f;
	}
}
