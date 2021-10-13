using System.Collections;
using System.Collections.Generic;
// using TapticPlugin;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Character
{
    public static PlayerController instance;
    [SerializeField] private List<ParticleSystem> confettis;

    [SerializeField] private float timeBetweenTwoBonusUpdate;


    private CameraManager cameraManager;

    protected float currentH;

    private int currentSneakyPointBonus;

    private float currentTimeBetweenTwoBonusUpdate;

    protected float currentV;

    private GameManager gameManager;

    private float hapticCurrentInterval;
    private bool screenIsTouched;

    private Animator sneakyBonusAnimator;

    private Text sneakyBonusText;

    private GameObject sneakyBonusTextGO;

    public GameObject deathsound;

    public GameObject FireSound;

    [Header("Sneaky")] private int sneakyPoints;

    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    protected override void Start()
    {
        base.Start();
        screenIsTouched = true;
        hapticCurrentInterval = 0f;
        sneakyPoints = 0;
        currentSneakyPointBonus = 0;
        sneakyBonusTextGO = sneakyBonusText.transform.parent.gameObject;
        sneakyBonusTextGO.SetActive(false);
        SetPlayerName(PlayerPrefs.GetString("Name", "Player"));
        cameraManager = CameraManager.Instance;
        gameManager = GameManager.Instance;
        sneakyBonusTextGO.GetComponent<FollowTarget>().SetTarget(transform);
        eventManager.AddListener<ScreenTouchedEvent>(OnScreenTouched);
    }


    protected override void Update()
    {
        base.Update();
        if (onEndScreen) return;
        if (canMove)
        {
            // if (playerCohort == "Hold_to_move")
            // {
            //     if (!screenIsTouched)
            //     {
            //         if (!isStopping)
            //         {
            //             isStopping = true;
            //             isStartingToStop = true;
            //         }
            //     }
            //     else
            //     {
            //         isStartingToStop = false;
            //         isStopping = false;
            //     }
            // }
            // else
            if (screenIsTouched)
            {
                isStopping = true;
                isStartingToStop = true;
            }
            else if (isStopping)
            {
                isStartingToStop = false;
                isStopping = false;
            }

            if (!isJumping && !isFlying)
            {
                float num;
                // if (playerCohort == "Hold_to_move")
                // {
                //     if (!screenIsTouched)
                //     {
                //         num = timeSinceStartedMoving + Time.deltaTime * -5f;
                //     }
                //     else
                //     {
                //         num = timeSinceStartedMoving + Time.deltaTime * (timeSinceStartedMoving < 0.5f ? 2.5f : 1.2f);
                //     }
                // }
                // else
                if (screenIsTouched)
                {
                    num = timeSinceStartedMoving + Time.deltaTime * -5f;
                }
                else if (playerCohort != "Instant_sprint" && playerCohort != "Faster_instant_sprint")
                {
                    num = timeSinceStartedMoving + Time.deltaTime * (timeSinceStartedMoving < 0.5f ? 2.5f : 1.2f);
                }
                else
                {
                    num = timeToReachMaxSpeed;
                }

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

            var counterIsCheckingPlayer = counterIsCheckingPlayers;
        }

        wasGrounded = isGrounded;
        // if (playerCohort == "Hold_to_move")
        // {
        //     if (isMoving && (!screenIsTouched || isJumping))
        //     {
        //         isMoving = false;
        //     }
        //     else if (!isMoving && !isJumping &&
        //              (screenIsTouched || Vector3.Distance(lastPosition, transform.position) > 0.01f))
        //     {
        //         isMoving = true;
        //     }
        // }
        // else 
        if (isMoving && (screenIsTouched || isJumping))
        {
            isMoving = false;
        }
        else if (!isMoving && !isJumping &&
                 (!screenIsTouched || Vector3.Distance(lastPosition, transform.position) > 0.01f))
        {
            isMoving = true;
        }

        lastPosition = transform.position;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.CompareTag("End") && !hasLost) screenIsTouched = true;
    }

    public void InitPlayerController(GameObject nameText, Text sneakyBonusText, Animator sneakyBonusAnimator)
    {
        this.nameText = nameText;
        this.sneakyBonusText = sneakyBonusText;
        this.sneakyBonusAnimator = sneakyBonusAnimator;
    }

    protected override void MovementUpdate()
    {
        base.MovementUpdate();
        if (gameManager.IsUsingScreenshakes()) HapticUpdate();
    }

    private void HapticUpdate()
    {
        if ((playerCohort == "Hold_to_move" && screenIsTouched || playerCohort != "Hold_to_move" && !screenIsTouched) &&
            !gameIsOver)
        {
            hapticCurrentInterval += Time.deltaTime;
            if (currentMoveSpeed > 5f && hapticCurrentInterval > 0.2f ||
                currentMoveSpeed > 3f && hapticCurrentInterval > 0.35f || hapticCurrentInterval > 0.5f)
            {
                //TapticManager.Impact(ImpactFeedback.Light);
                Vibration.Vibrate(5);
                hapticCurrentInterval = 0f;
            }
        }
        else
        {
            hapticCurrentInterval = 0f;
        }
    }

    private void SneakyUpdate()
    {
        if (!gameIsOver)
        {
            if (!sneakyBonusTextGO.activeSelf)
            {
                sneakyBonusTextGO.SetActive(true);
                currentSneakyPointBonus = 0;
                AddSneakyPoint();
            }

            currentTimeBetweenTwoBonusUpdate += Time.deltaTime;
            if (currentTimeBetweenTwoBonusUpdate >= timeBetweenTwoBonusUpdate) AddSneakyPoint();
        }
    }

    private void AddSneakyPoint()
    {
        sneakyPoints++;
        currentSneakyPointBonus++;
        sneakyBonusText.text = currentSneakyPointBonus.ToString();
        sneakyBonusAnimator.Play("Grow", 0, 0f);
        currentTimeBetweenTwoBonusUpdate = 0f;
    }

    public override void OnBeingSeen()
    {
        base.OnBeingSeen();
    }

    public override void OnDying()
    {
        base.OnDying();
    }

    public override void Losing(bool fromDeath)
    {
// play death audio play
        if (fromDeath)
        {
            deathsound.SetActive(true);
        }
        else
        {
            FireSound.SetActive(true);
            deathsound.SetActive(true);
        }

        if (!isCurrentlyInvincible && !hasLost)
        {
            cameraManager.StopFollowingPlayer();
            sneakyBonusTextGO.SetActive(false);
            currentSneakyPointBonus = 0;
            base.Losing(fromDeath);

            //FireSound.SetActive(false);
            //deathsound.SetActive(false);
        }
    }

    private void OnScreenTouched(ScreenTouchedEvent e)
    {
        if (!gameIsOver) screenIsTouched = !e.isTouched;
    }

    protected override void ActivateCharacter()
    {
        if ((firstInit || hasLost) && !gameIsOver)
        {
            cameraManager.StartFollowingPlayer();
            base.ActivateCharacter();
        }
    }

    protected override void OnCounterTurning(CounterIsTurningEvent e)
    {
        base.OnCounterTurning(e);
        if (sneakyBonusTextGO.activeSelf) StartCoroutine(DeactivateSneakyText());
    }

    protected override void OnGameEnding(GameFinishedEvent e)
    {
        base.OnGameEnding(e);
        sneakyBonusTextGO.SetActive(false);
    }

    private IEnumerator DeactivateSneakyText()
    {
        yield return new WaitForSeconds(1f);
        sneakyBonusTextGO.SetActive(false);
    }

    protected override void Won()
    {
        base.Won();
        for (var i = 0; i < confettis.Count; i++) confettis[i].Play();
    }

    public override void PlaceOnPodium(int place, Vector3 podiumPosition)
    {
        base.PlaceOnPodium(place, podiumPosition);
        for (var i = 0; i < confettis.Count; i++) confettis[i].gameObject.SetActive(false);
    }

    public int GetSneakyPoints()
    {
        return sneakyPoints;
    }
}