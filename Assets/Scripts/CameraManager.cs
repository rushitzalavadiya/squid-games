using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public static CameraManager Instance;

	[SerializeField]
	private Transform playerTransform;

	[SerializeField]
	private Vector3 offsetCounter = Vector3.zero;

	[SerializeField]
	private Vector3 offsetGame = Vector3.zero;

	[SerializeField]
	private Vector3 offsetFinishLine = Vector3.zero;

	[SerializeField]
	private Quaternion rotationFinishLine = Quaternion.identity;

	[SerializeField]
	private float moveSpeed = 0.15f;

	private Vector3 startingPositionWinner;

	private Vector3 endingPositionWinner;

	private bool isFollowingPlayer;

	private bool isFollowingWinner;

	private bool isEndGameScreen;

	private Vector3 velocity;

	private void Awake()
	{
		isFollowingPlayer = false;
		isEndGameScreen = false;
		Instance = this;
	}

	private void Start()
	{
		base.transform.position = playerTransform.position + offsetCounter;
		EventManager instance = EventManager.Instance;
		instance.AddListener<FirstStartCountingEvent>(OnGameStarting);
		instance.AddListener<GameFinishedEvent>(OnGameEnding);
	}

	private void LateUpdate()
	{
		if ((bool)playerTransform)
		{
			if (isFollowingPlayer)
			{
				Vector3 target = playerTransform.position + offsetGame;
				Vector3 position = Vector3.SmoothDamp(base.transform.position, target, ref velocity, moveSpeed);
				base.transform.position = position;
			}
			else if (isFollowingWinner)
			{
				Vector3 position2 = Vector3.SmoothDamp(base.transform.position, endingPositionWinner, ref velocity, moveSpeed + 1f);
				base.transform.position = position2;
				base.transform.LookAt(playerTransform);
			}
		}
	}

	private IEnumerator GetBackToPlayer()
	{
		yield return new WaitForSeconds(1f);
		isFollowingPlayer = true;
		Vector3 desiredPosition = playerTransform.position + offsetGame;
		while (Vector3.Distance(base.transform.position, desiredPosition) >= 0.2f)
		{
			yield return new WaitForEndOfFrame();
		}
		EventManager.Instance.QueueEvent(new GameStartedEvent());
	}

	private void OnGameStarting(FirstStartCountingEvent e)
	{
		StartCoroutine(GetBackToPlayer());
	}

	private void OnGameEnding(GameFinishedEvent e)
	{
		if (isFollowingPlayer)
		{
			isFollowingPlayer = false;
			isFollowingWinner = true;
			startingPositionWinner = base.transform.position;
			endingPositionWinner = playerTransform.position + offsetFinishLine;
		}
	}

	public void StopFollowingPlayer()
	{
		isFollowingPlayer = false;
	}

	public void StartFollowingPlayer()
	{
		if (!isFollowingWinner && !isEndGameScreen)
		{
			isFollowingPlayer = true;
		}
	}

	public void SetTarget(Transform playerTransform)
	{
		this.playerTransform = playerTransform;
	}
}
