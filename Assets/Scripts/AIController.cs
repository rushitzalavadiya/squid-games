using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Character
{
	private enum AIState
	{
		Moving,
		Stopping
	}

	private AIState currentState;

	[SerializeField]
	private float timeBeforeStateSwitching = 5f;

	private float currentTimeBeforeStateSwitching;

	[Header("Debug")]
	[SerializeField]
	private bool usingAI = true;

	private bool shouldRunToEscapeDanger;

	private bool shouldStopToEscapeDanger;

	private List<Hazard> hazardsToRunFrom = new List<Hazard>();

	private List<Hazard> hazardsToStopFrom = new List<Hazard>();

	public void InitAIController(GameObject nameText)
	{
		base.nameText = nameText;
	}

	protected override void Awake()
	{
		base.Awake();
		currentState = AIState.Stopping;
	}

	protected override void Start()
	{
		base.Start();
		if (playerCohort == "Instant_sprint" || playerCohort == "Faster_instant_sprint")
		{
			maxMoveSpeed += 1f;
		}
	}

	protected override void Update()
	{
		if (!usingAI)
		{
			return;
		}
		base.Update();
		if (hasLost)
		{
			return;
		}
		if (canMove)
		{
			if (currentState == AIState.Moving)
			{
				isStopping = false;
				isStartingToStop = false;
			}
			else if (!isStopping)
			{
				isStartingToStop = true;
				isStopping = true;
			}
			if (!isJumping && !isFlying)
			{
				float num = 0f;
				num = ((currentState != 0) ? (timeSinceStartedMoving + Time.deltaTime * -5f) : ((!(playerCohort == "Instant_sprint") && !(playerCohort == "Faster_instant_sprint")) ? (timeSinceStartedMoving + Time.deltaTime * ((timeSinceStartedMoving < 0.5f) ? 2.5f : 1.2f)) : timeToReachMaxSpeed));
				timeSinceStartedMoving = Mathf.Clamp(num, 0f, timeToReachMaxSpeed);
				MovementUpdate();
			}
			else
			{
				if (isStopping && timeSinceStartedMoving > 0f)
				{
					timeSinceStartedMoving = 0f;
					isStartingToStop = false;
					animator.SetFloat("MoveSpeed", 0f);
				}
				JumpUpdate();
			}
		}
		wasGrounded = isGrounded;
		if (isMoving && (Vector3.Distance(lastPosition, base.transform.position) <= 0.01f || isJumping))
		{
			isMoving = false;
		}
		else if (!isMoving && Vector3.Distance(lastPosition, base.transform.position) > 0.01f && !isJumping)
		{
			isMoving = true;
		}
		currentTimeBeforeStateSwitching += Time.deltaTime;
		if (currentTimeBeforeStateSwitching > timeBeforeStateSwitching)
		{
			SelectNewState();
		}
		lastPosition = base.transform.position;
	}

	protected override void ActivateCharacter()
	{
		base.ActivateCharacter();
		nameText.SetActive(value: true);
		hazardsToStopFrom.Clear();
		hazardsToRunFrom.Clear();
	}

	protected override void MovementUpdate()
	{
		base.MovementUpdate();
	}

	private void SelectNewState()
	{
		if (!hasLost && !hasWon && !onEndScreen && canMove)
		{
			if (counterIsCheckingPlayers)
			{
				if (hazardsToStopFrom.Count > 0)
				{
					StopMoving();
				}
				else if (hazardsToRunFrom.Count > 0)
				{
					if (Random.Range(0, 20) <= 9)
					{
						StartMoving();
					}
					else
					{
						StopMoving();
					}
				}
				else if (Random.Range(0, 20) <= 3)
				{
					StartMoving();
				}
				else
				{
					StopMoving();
				}
			}
			else if (hazardsToStopFrom.Count > 0)
			{
				if (Random.Range(0, 20) <= 7)
				{
					StartMoving();
				}
				else
				{
					StopMoving();
				}
			}
			else if (hazardsToRunFrom.Count > 0)
			{
				if (Random.Range(0, 20) <= 18)
				{
					StartMoving();
				}
				else
				{
					StopMoving();
				}
			}
			else
			{
				StartMoving();
			}
		}
		currentTimeBeforeStateSwitching = 0f;
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (other.gameObject.CompareTag("End") && !hasLost)
		{
			StartMoving();
		}
	}

	private void StartMoving()
	{
		currentState = AIState.Moving;
	}

	private void StopMoving()
	{
		currentState = AIState.Stopping;
	}

	protected override void OnCounterTurning(CounterIsTurningEvent e)
	{
		base.OnCounterTurning(e);
		if (counterIsCheckingPlayers)
		{
			StartCoroutine(WaitBeforeStopping());
		}
		else
		{
			StartCoroutine(WaitBeforeMoving());
		}
	}

	private IEnumerator WaitBeforeMoving()
	{
		float seconds = Random.Range(0.1f, 1f);
		yield return new WaitForSeconds(seconds);
		SelectNewState();
	}

	private IEnumerator WaitBeforeStopping()
	{
		float seconds = Random.Range(0f, 1f);
		yield return new WaitForSeconds(seconds);
		StopMoving();
	}

	public void AddHazardToRunFrom(Hazard hazard)
	{
		hazardsToRunFrom.Add(hazard);
		SelectNewState();
	}

	public void AddHazardToStopFrom(Hazard hazard)
	{
		hazardsToStopFrom.Add(hazard);
		SelectNewState();
	}

	public void RevoveHazardToRunFrom(Hazard hazard)
	{
		hazardsToRunFrom.Remove(hazard);
		SelectNewState();
	}

	public void RevoveHazardToStopFrom(Hazard hazard)
	{
		hazardsToStopFrom.Remove(hazard);
		SelectNewState();
	}

	protected override void OnGameStarting(GameStartedEvent e)
	{
		base.OnGameStarting(e);
		StartCoroutine(WaitBeforeMoving());
	}

	protected override void OnGameEnding(GameFinishedEvent e)
	{
		base.OnGameEnding(e);
		counterIsCheckingPlayers = false;
	}
}
