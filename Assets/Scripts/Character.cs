using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField]
	private int playerId = -2;

	[SerializeField]
	protected string playerName = "Player";

	[SerializeField]
	protected float maxMoveSpeed = 5f;

	protected float currentMoveSpeed;

	protected float temporaryInvincibility = 0.3f;

	protected bool isCurrentlyInvincible;

	protected float temporaryInvisibility = 1f;

	protected bool isCurrentlyInvisible;

	private float startingZPoint;

	[SerializeField]
	protected float timeToReachMaxSpeed = 6f;

	protected float timeSinceStartedMoving;

	[Header("References")]
	[SerializeField]
	protected GameObject characterModel;

	[SerializeField]
	protected GameObject nameText;

	[SerializeField]
	private TrailRenderer trailRenderer;

	[SerializeField]
	protected ParticleSystem detectedEmote;

	[SerializeField]
	protected ParticleSystem stopSmoke;

	[SerializeField]
	private GameObject crown;

	protected Animator animator;

	protected Rigidbody ownRigidbody;

	private Vector3 respawnPosition = Vector3.zero;

	protected List<Collider> collisions = new List<Collider>();

	protected readonly float interpolation = 10f;

	protected readonly float walkScale = 0.33f;

	protected float jumpTimeStamp;

	protected float minJumpInterval = 0.25f;

	protected Vector3 currentDirection = Vector3.zero;

	protected bool wasGrounded;

	protected bool isGrounded;

	protected bool canMove;

	protected EventManager eventManager;

	protected Pool pool;

	protected Transform mainCameraTransform;

	protected bool firstInit;

	public bool isMoving = false;

	protected bool isStartingToStop;

	protected bool isStopping;

	protected bool hasLost;

	protected bool hasWon;

	protected bool gameIsOver;

	protected bool counterIsCheckingPlayers;

	protected Vector3 lastPosition;

	protected bool isFlying;

	protected bool isJumping;

	protected float jumpStartZ;

	protected float jumpDestinationZ;

	[SerializeField]
	protected float jumpDuration;

	protected float currentJumpDuration;

	protected bool onEndScreen;

	protected string playerCohort;

	//private PlayerController Player;

	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
		ownRigidbody = GetComponent<Rigidbody>();
		crown.SetActive(value: false);
		firstInit = true;
		canMove = false;
		isMoving = false;
		hasLost = false;
		hasWon = false;
		gameIsOver = false;
		isCurrentlyInvincible = false;
		isCurrentlyInvisible = false;
		counterIsCheckingPlayers = false;
		isStartingToStop = false;
		isStopping = false;
		isGrounded = true;
		currentMoveSpeed = 0f;
		timeSinceStartedMoving = 0f;
		mainCameraTransform = Camera.main.transform;
	}

	protected virtual void Start()
	{
		playerCohort = VoodooSauce.GetPlayerCohort();
		if (playerCohort == "Faster_instant_sprint")
		{
			maxMoveSpeed += 1.5f;
		}
		respawnPosition = base.transform.position;
		eventManager = EventManager.Instance;
		pool = Pool.Instance;
		startingZPoint = base.transform.position.z;
		lastPosition = base.transform.position;
		eventManager.AddListener<GameStartedEvent>(OnGameStarting);
		eventManager.AddListener<GameFinishedEvent>(OnGameEnding);
		eventManager.AddListener<CounterIsTurningEvent>(OnCounterTurning);
		nameText.GetComponent<FollowTarget>().SetTarget(base.transform);
	}

	protected virtual void Update()
	{
		JumpingAndLanding();
		animator.SetBool("Grounded", isGrounded);
	}

	protected virtual void MovementUpdate()
	{
		Vector3 vector = base.transform.forward;
		float magnitude = vector.magnitude;
		vector.y = 0f;
		vector = vector.normalized * magnitude;
		if (!(vector != Vector3.zero))
		{
			return;
		}
		if (isStopping)
		{
			if (isStartingToStop)
			{
				isStartingToStop = false;
				animator.speed = 0f;
				if (currentMoveSpeed > 4f)
				{
					stopSmoke.Play();
				}
			}
			if ((currentMoveSpeed <= 0.1f || hasLost) && stopSmoke.isPlaying)
			{
				if (hasLost)
				{
					stopSmoke.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
				else
				{
					stopSmoke.Stop();
				}
			}
		}
		else
		{
			if (stopSmoke.isPlaying)
			{
				stopSmoke.Stop();
			}
			animator.speed = 1f;
		}
		currentMoveSpeed = Mathf.Lerp(0f, 1f, timeSinceStartedMoving / timeToReachMaxSpeed) * maxMoveSpeed;
		currentDirection = Vector3.Slerp(currentDirection, vector, Time.deltaTime * interpolation);
		base.transform.rotation = Quaternion.LookRotation(currentDirection);
		base.transform.position += currentDirection * currentMoveSpeed * Time.deltaTime;
		if (!isStopping)
		{
			animator.SetFloat("MoveSpeed", currentMoveSpeed / maxMoveSpeed);
		}
	}

	protected virtual void JumpUpdate()
	{
		if (isJumping)
		{
			currentJumpDuration += Time.deltaTime;
			Vector3 lhs = base.transform.forward;
			float magnitude = lhs.magnitude;
			lhs.y = 0f;
			lhs = lhs.normalized * magnitude;
			if (lhs != Vector3.zero)
			{
				float z = Mathf.Lerp(jumpStartZ, jumpDestinationZ, currentJumpDuration / jumpDuration);
				base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, z);
			}
			if (currentJumpDuration >= jumpDuration)
			{
				isJumping = false;
			}
			if (stopSmoke.isPlaying)
			{
				stopSmoke.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
	}

	protected void JumpingAndLanding()
	{
		if (onEndScreen || (isFlying && base.transform.position.y < 0.6f))
		{
			isGrounded = true;
			isFlying = false;
			animator.SetTrigger("Land");
			if (!onEndScreen)
			{
				GameObject pooledObject = pool.GetPooledObject("SmokePuff");
				pooledObject.transform.position = new Vector3(base.transform.position.x, pooledObject.transform.position.y, base.transform.position.z);
				pooledObject.SetActive(value: true);
				pooledObject.GetComponent<ParticleSystem>().Play();
			}
		}
		if (!onEndScreen && isJumping && !isFlying && base.transform.position.y >= 0.6f)
		{
			if (isStopping)
			{
				animator.speed = 1f;
			}
			animator.SetTrigger("Jump");
			jumpTimeStamp = Time.time;
			isFlying = true;
			isGrounded = false;
		}
	}

	protected virtual void OnGameStarting(GameStartedEvent e)
	{
		ActivateCharacter();
	}

	protected virtual void OnGameEnding(GameFinishedEvent e)
	{
		gameIsOver = true;
		animator.speed = 1f;
	}

	protected virtual void OnCounterTurning(CounterIsTurningEvent e)
	{
		counterIsCheckingPlayers = e.isTurningTowardPlayers;
	}

	public void StopCharacter()
	{
		onEndScreen = true;
		trailRenderer.enabled = false;
		canMove = false;
		ownRigidbody.velocity = Vector3.zero;
		ownRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		animator.SetFloat("MoveSpeed", 0f);
		detectedEmote.gameObject.SetActive(value: false);
	}

	public virtual void PlaceOnPodium(int place, Vector3 podiumPosition)
	{
		base.gameObject.SetActive(value: true);
		characterModel.SetActive(value: true);
		nameText.SetActive(value: true);
		stopSmoke.gameObject.SetActive(value: false);
		if (place == 1)
		{
			animator.Play("Win");
			nameText.GetComponent<FollowTarget>().ModifyOffset(new Vector3(0f, 1.4f, 0f));
		}
		else
		{
			animator.Play("Empty");
			nameText.GetComponent<FollowTarget>().ModifyOffset(new Vector3(0f, 1.2f, 0f));
		}
		base.transform.position = podiumPosition;
		base.transform.eulerAngles = new Vector3(0f, -90f, 0f);
		animator.speed = 1f;
	}

	public void DeactivateCharacter()
	{
		nameText.SetActive(value: false);
		base.gameObject.SetActive(value: false);
		if (eventManager != null)
		{
			if (eventManager.HasListener<GameStartedEvent>(OnGameStarting))
			{
				eventManager.RemoveListener<GameStartedEvent>(OnGameStarting);
			}
			if (eventManager.HasListener<GameFinishedEvent>(OnGameEnding))
			{
				eventManager.RemoveListener<GameFinishedEvent>(OnGameEnding);
			}
			if (eventManager.HasListener<CounterIsTurningEvent>(OnCounterTurning))
			{
				eventManager.RemoveListener<CounterIsTurningEvent>(OnCounterTurning);
			}
		}
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (!hasWon && other.gameObject.CompareTag("End"))
		{
			Won();
		}
	}

	protected virtual void Won()
	{
		hasWon = true;
		canMove = false;
		animator.speed = 1f;
		currentMoveSpeed = 0f;
		animator.SetFloat("MoveSpeed", 0f);
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			animator.Play("Dance");
		}
		else
		{
			animator.Play("Win");
		}
		stopSmoke.gameObject.SetActive(value: false);
		PlayerHasWonEvent evt = new PlayerHasWonEvent(playerId);
		eventManager.QueueEvent(evt);
	}

	public virtual void OnBeingSeen()
	{
		if (!isCurrentlyInvisible)
		{
			Losing(fromDeath: false);
		}
	}

	public virtual void OnDying()
	{
		Losing(fromDeath: true);
	}

	public virtual void Losing(bool fromDeath)
	{
		if (!isCurrentlyInvincible && !hasLost)
		{
			isMoving = false;
			isFlying = false;
			hasLost = true;
			canMove = false;
			isJumping = false;
			isGrounded = true;
			nameText.SetActive(value: false);
			trailRenderer.enabled = false;
			animator.speed = 1f;
			animator.SetFloat("MoveSpeed", 0f);
			isCurrentlyInvincible = true;
			isCurrentlyInvisible = true;
			if (!fromDeath)
			{
				detectedEmote.Play();
			}
			base.transform.tag = "DeadPlayer";
			stopSmoke.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
			eventManager.QueueEvent(new PlayerLostEvent(playerId, playerName, !fromDeath));
			StartCoroutine(Respawn());
		}
	}

	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds(1f);
		ActivateCharacter();
		StartCoroutine(Invincible());
		StartCoroutine(Invisible());
		yield return new WaitForSeconds(1f);
		PlayerController.instance.FireSound.SetActive(false);
		PlayerController.instance.deathsound.SetActive(false);
	}

	private IEnumerator Invincible()
	{
		yield return new WaitForSeconds(temporaryInvincibility);
		isCurrentlyInvincible = false;
	}

	private IEnumerator Invisible()
	{
		yield return new WaitForSeconds(temporaryInvisibility);
		isCurrentlyInvisible = false;
	}

	public void ChangeRespawn(float newZ)
	{
		respawnPosition.z = newZ;
	}

	protected virtual void ActivateCharacter()
	{
		if ((firstInit || hasLost) && !hasWon)
		{
			animator.enabled = true;
			base.transform.tag = "Player";
			ownRigidbody.velocity = Vector3.zero;
			ownRigidbody.constraints = (RigidbodyConstraints)114;
			base.transform.position = respawnPosition;
			currentMoveSpeed = 0f;
			timeSinceStartedMoving = 0f;
			isJumping = false;
			hasLost = false;
			canMove = true;
			trailRenderer.enabled = true;
			isMoving = false;
			base.transform.eulerAngles = Vector3.zero;
			if (firstInit)
			{
				firstInit = false;
			}
		}
	}

	public bool IsMoving()
	{
		bool result = isMoving;
		if (isCurrentlyInvisible || hasWon)
		{
			result = false;
		}
		return result;
	}

	public bool HasThePlayerLost()
	{
		return hasLost;
	}

	public void JumperJump(float distance)
	{
		AddForce(Vector3.up * 200f, shouldResetVelocity: true);
		isJumping = true;
		jumpStartZ = base.transform.position.z;
		jumpDestinationZ = jumpStartZ + distance;
		currentJumpDuration = 0f;
	}

	public void AddForce(Vector3 forceToAdd, bool shouldResetVelocity)
	{
		if (!isCurrentlyInvisible && !hasLost)
		{
			if (shouldResetVelocity)
			{
				ownRigidbody.velocity = Vector3.zero;
			}
			ownRigidbody.AddForce(forceToAdd, ForceMode.Acceleration);
		}
	}

	public float GetDistanceRun()
	{
		return Mathf.Abs(base.transform.position.z - startingZPoint) * 3.281f;
	}

	public int GetPlayerId()
	{
		return playerId;
	}

	public void SetPlayerName(string newName)
	{
		playerName = newName;
		nameText.GetComponent<Text>().text = playerName;
	}

	public void SetPlayerId(int id)
	{
		playerId = id;
	}

	public void PreventMoving()
	{
		if (!isCurrentlyInvincible && !hasLost)
		{
			canMove = false;
			ownRigidbody.constraints = RigidbodyConstraints.None;
		}
	}

	public bool IsAbleToMove()
	{
		return canMove;
	}

	public void ManageCrown(bool shouldBeActivated)
	{
		crown.SetActive(shouldBeActivated);
	}

	public bool HasLost()
	{
		return hasLost;
	}

	public bool HasWon()
	{
		return hasWon;
	}

	public bool IsInvincible()
	{
		return isCurrentlyInvincible;
	}

	public bool IsInvisible()
	{
		return isCurrentlyInvisible;
	}
}
