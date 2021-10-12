using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingCharacter : MonoBehaviour
{
	private enum CountingState
	{
		One,
		Two,
		Three,
		Sun
	}

	private enum CountingSpeed
	{
		Slow,
		Normal,
		Fast
	}

	private bool canMove;

	private CountingState currentCountingState;

	private CountingSpeed currentCountingSpeed;

	private Animator ownAnimator;

	private GameManager gameManager;

	[SerializeField]
	private BoxCollider fieldOfVision;

	[Header("Light")]
	[SerializeField]
	private GameObject spotLight;

	private float xScaleToApply = 60f;

	private float yScaleToApply = 50f;

	[Header("Actors")]
	[SerializeField]
	private Transform wallTransform;

	[SerializeField]
	private Transform frontBlockTransform;

	[Header("Bubble")]
	[SerializeField]
	private GameObject bubble;

	[SerializeField]
	private Animator bubbleAnimator;

	[SerializeField]
	private Image bubbleImage;

	[SerializeField]
	private Color redLightColor = Color.red;

	[SerializeField]
	private Color greenLightColor = Color.green;

	[SerializeField]
	private Text bubbleText;

	[Header("Parameters")]
	[SerializeField]
	private float slowCountingTime = 2.5f;

	[SerializeField]
	private float normalCountingTime = 1f;

	[SerializeField]
	private float fastCountingTime = 0.75f;

	[SerializeField]
	private float timeSpentOnEachPlayer = 0.5f;

	private float timeToTurnToWall = 1.5f;

	private float currentTimeSpentOnPlayer;

	private float currentCountingTime;

	private List<Character> currentlySeenCharacters = new List<Character>();

	protected EventManager eventManager;

	public GameObject redlight;

	public GameObject greenlight;

	//public GameObject sound;

	public void InitCountingCharacter(Transform wallTransform, Transform frontBlockTransform, GameObject bubble, Animator bubbleAnimator, Image bubbleImage, Text bubbleText)
	{
		this.wallTransform = wallTransform;
		this.frontBlockTransform = frontBlockTransform;
		this.bubble = bubble;
		this.bubbleAnimator = bubbleAnimator;
		this.bubbleImage = bubbleImage;
		this.bubbleText = bubbleText;
	}

	private void Start()
	{
		if (VoodooSauce.GetPlayerCohort() == "No_red")
		{
			base.gameObject.SetActive(value: false);
			bubble.SetActive(value: false);
			return;
		}
		fieldOfVision.enabled = false;
		canMove = false;
		ownAnimator = GetComponent<Animator>();
		bubbleAnimator.Play("Idle", 0, 0f);
		spotLight.gameObject.SetActive(value: false);
		eventManager = EventManager.Instance;
		gameManager = GameManager.Instance;
		eventManager.AddListener<FirstStartCountingEvent>(OnFirstTimeCounting);
		eventManager.AddListener<GameStartedEvent>(OnGameStarting);
		eventManager.AddListener<GameFinishedEvent>(OnGameEnding);
		eventManager.AddListener<EndScreenAppearingEvent>(OnEndScreenAppearing);
		ownAnimator.SetBool("Grounded", value: true);
		ownAnimator.SetTrigger("Land");
		bubbleText.fontSize = 2;
	}

	private void Update()
	{
		int num = 0;
		while (num < currentlySeenCharacters.Count)
		{
			Character character = currentlySeenCharacters[num];
			if (CheckIfCharacterIsMoving(character))
			{
				if (currentlySeenCharacters.Contains(character))
				{
					currentlySeenCharacters.Remove(character);
				}
			}
			else
			{
				num++;
			}
		}
	}

	private void PostSunInit()
	{
		if (canMove)
		{
			bubbleImage.color = greenLightColor;
			bubbleAnimator.Play("Scream", 0, 0f);
			bubbleText.text = LanguageScript.get_string(13);
			greenlight.GetComponent<AudioSource>().enabled = true;
			redlight.GetComponent<AudioSource>().enabled = false;
			currentCountingState = CountingState.One;
			SelectNewSpeed();
			StartCoroutine(CountDown());
		}
	}

	private void SelectNewSpeed()
	{
		int num = UnityEngine.Random.Range(0, 10);
		if (num <= 2)
		{
			currentCountingSpeed = CountingSpeed.Slow;
		}
		else if (num <= 7)
		{
			currentCountingSpeed = CountingSpeed.Normal;
		}
		else
		{
			currentCountingSpeed = CountingSpeed.Fast;
		}
	}

	private IEnumerator CountDown()
	{
		yield return new WaitForSeconds(1f);
		bubbleText.fontSize = 3;
		while (currentCountingState != CountingState.Sun && canMove)
		{
		
			bubbleText.text = ((int)(3 - currentCountingState)).ToString();
			switch (currentCountingSpeed)
			{
			case CountingSpeed.Slow:
				bubbleAnimator.Play("Slow", 0, 0f);
				yield return new WaitForSeconds(slowCountingTime);
				break;
			case CountingSpeed.Normal:
				bubbleAnimator.Play("Normal", 0, 0f);
				yield return new WaitForSeconds(normalCountingTime);
				break;
			case CountingSpeed.Fast:
				bubbleAnimator.Play("Fast", 0, 0f);
				yield return new WaitForSeconds(fastCountingTime);
				break;
			}
			currentCountingState++;
			SelectNewSpeed();
		}
		if (canMove && currentCountingState == CountingState.Sun)
		{
			
			StartCoroutine(RotateTowardsFrontBlock());
		}
	}

	private IEnumerator ScaleAndRepositionSpotlight()
	{
		Vector3 newPosition = spotLight.transform.localPosition;
		yScaleToApply = 50f;
		while (canMove)
		{
			yield return new WaitForSeconds(0.02f);
			newPosition.z = (yScaleToApply = 50f - gameManager.GetDistanceRunByPlayer() / 2f);
			spotLight.transform.localPosition = newPosition;
			spotLight.transform.localScale = new Vector3(xScaleToApply, yScaleToApply, 1f);
		}
	}

	private IEnumerator RotateTowardsFrontBlock()
	{
		if (!canMove)
		{
			yield break;
		}
		eventManager.QueueEvent(new CounterIsTurningEvent(turningTowardPlayers: true));
		bubbleImage.color = redLightColor;
		bubbleText.fontSize = 2;
		bubbleText.text = LanguageScript.get_string(14);
		redlight.GetComponent<AudioSource>().enabled = true;
		greenlight.GetComponent<AudioSource>().enabled = false;
		bubbleAnimator.Play("Scream", 0, 0f);
		fieldOfVision.enabled = true;
		xScaleToApply = 65f;
		currentTimeSpentOnPlayer = 0f;
		Vector3 normalized = (frontBlockTransform.position - base.transform.position).normalized;
		normalized.y = 0f;
		Quaternion lookingRot = Quaternion.LookRotation(normalized);
		fieldOfVision.size = new Vector3(2f, fieldOfVision.size.y, fieldOfVision.size.z);
		while (canMove && currentTimeSpentOnPlayer <= timeToTurnToWall)
		{
			currentTimeSpentOnPlayer += Time.deltaTime;
			if (currentTimeSpentOnPlayer > 0.1f && !spotLight.activeSelf)
			{
				spotLight.SetActive(value: true);
			}
			if (currentTimeSpentOnPlayer > 1f && fieldOfVision.size.x != 15f)
			{
				fieldOfVision.size = new Vector3(15f, fieldOfVision.size.y, fieldOfVision.size.z);
			}
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, lookingRot, Time.deltaTime * 2f);
			yield return new WaitForEndOfFrame();
		}
		if (VoodooSauce.GetPlayerCohort() == "Large_cone")
		{
			yield return new WaitForSeconds(1f);
			StartCoroutine(RotateTowardsWall(shouldInit: true));
		}
		else
		{
			StartCoroutine(CheckPlayers());
		}
	}

	private IEnumerator CheckPlayers()
	{
		if (!canMove)
		{
			yield break;
		}
		StartCoroutine(ReduceTheLight());
		Character firstPlayer = gameManager.GetCurrentlyFirstPlayer();
		Character characterToCheck = firstPlayer;
		while (characterToCheck != null)
		{
			Transform characterTransform = characterToCheck.transform;
			currentTimeSpentOnPlayer = 0f;
			while (canMove && currentTimeSpentOnPlayer <= timeSpentOnEachPlayer && !characterToCheck.HasWon())
			{
				Vector3 normalized = (characterTransform.position - base.transform.position).normalized;
				normalized.y = 0f;
				Quaternion b = Quaternion.LookRotation(normalized);
				currentTimeSpentOnPlayer += Time.deltaTime;
				if (timeSpentOnEachPlayer - currentTimeSpentOnPlayer < 0.25f)
				{
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, 1f);
				}
				else
				{
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 5f);
				}
				yield return new WaitForEndOfFrame();
			}
			if (characterToCheck == firstPlayer)
			{
				Character character = gameManager.GetCurrentSecondPlayer();
				if (character == firstPlayer)
				{
					character = gameManager.GetCurrentlyFirstPlayer();
				}
				characterToCheck = character;
			}
			else
			{
				characterToCheck = null;
			}
		}
		eventManager.QueueEvent(new CounterIsTurningEvent(turningTowardPlayers: false));
		StartCoroutine(RotateTowardsWall(shouldInit: true));
	}

	private IEnumerator RotateTowardsWall(bool shouldInit)
	{
		if (!canMove)
		{
			yield break;
		}
		StartCoroutine(TurnLightOff());
		currentlySeenCharacters.Clear();
		fieldOfVision.enabled = false;
		currentTimeSpentOnPlayer = 0f;
		Vector3 normalized = (wallTransform.position - base.transform.position).normalized;
		normalized.y = 0f;
		Quaternion lookingRot = Quaternion.LookRotation(normalized);
		while (canMove && currentTimeSpentOnPlayer <= timeToTurnToWall)
		{
			currentTimeSpentOnPlayer += Time.deltaTime;
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, lookingRot, Time.deltaTime * 3f);
			if (currentTimeSpentOnPlayer > 0.75f && spotLight.activeSelf)
			{
				spotLight.SetActive(value: false);
			}
			yield return new WaitForEndOfFrame();
		}
		if (shouldInit)
		{
			PostSunInit();
		}
	}

	protected void OnFirstTimeCounting(FirstStartCountingEvent e)
	{
		canMove = true;
		StartCoroutine(RotateTowardsWall(shouldInit: false));
		StartCoroutine(ScaleAndRepositionSpotlight());
	}

	protected void OnGameStarting(GameStartedEvent e)
	{
		PostSunInit();
	}

	protected void OnGameEnding(GameFinishedEvent e)
	{
		canMove = false;
		bubble.SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	protected void OnEndScreenAppearing(EndScreenAppearingEvent e)
	{
		bubble.SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Character component = other.GetComponent<Character>();
			if (!currentlySeenCharacters.Contains(component))
			{
				currentlySeenCharacters.Add(component);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Character component = other.GetComponent<Character>();
			if (currentlySeenCharacters.Contains(component))
			{
				currentlySeenCharacters.Remove(component);
			}
		}
	}

	private bool CheckIfCharacterIsMoving(Character character)
	{
		if (canMove && character.IsMoving())
		{
			character.OnBeingSeen();
			bubbleText.text = LanguageScript.get_string(15);
			return true;
		}
		return false;
	}

	private IEnumerator ReduceTheLight()
	{
		float maxValue = 65f;
		float fieldOfVisionSize2 = fieldOfVision.size.x;
		while (canMove && (xScaleToApply > 0.4f || fieldOfVisionSize2 > 0.1f))
		{
			xScaleToApply -= 1.5f;
			fieldOfVisionSize2 -= 0.33f;
			xScaleToApply = Mathf.Clamp(xScaleToApply, 0.4f, maxValue);
			fieldOfVisionSize2 = Mathf.Clamp(fieldOfVisionSize2, 0.1f, 15f);
			fieldOfVision.size = new Vector3(fieldOfVisionSize2, fieldOfVision.size.y, fieldOfVision.size.z);
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator TurnLightOff()
	{
		float currentAngle2 = spotLight.transform.localScale.x;
		while (canMove && currentAngle2 > 0f)
		{
			currentAngle2 -= 0.1f;
			currentAngle2 = Mathf.Clamp(currentAngle2, 0f, 1f);
			spotLight.transform.localScale = new Vector3(currentAngle2, yScaleToApply, 1f);
			yield return new WaitForEndOfFrame();
		}
	}
}
