// using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using System.Text;
// using TextFx;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	private bool gameIsOver;

	private int currentLevel;

	[Header("Actors")]
	private Transform playerTransform;

	private List<Character> characterScripts = new List<Character>();

	private PlayerController playerController;

	private Character currentBestPlayer;

	private Character currentSecondPlayer;

	[SerializeField]
	private Vector3 playerStartingPosition = Vector3.zero;

	[SerializeField]
	private Transform endLineTransform;

	[SerializeField]
	private List<ParticleSystem> fireworks;

	private List<Character> finalPlayerRanking;

	[Header("UI")]
	[SerializeField]
	private GameObject gamePanel;

	[SerializeField]
	private Slider progressionSlider;

	[SerializeField]
	private InformationText informationText;

	[SerializeField]
	private TextMeshProUGUI goText;

//	[SerializeField]
	//private Text currentLevelText;

	//[SerializeField]
	//private Text nextLevelText;

	[SerializeField]
	private TextMeshProUGUI levelText;

	[SerializeField]
	private Animator goAnimator;

	[SerializeField]
	private GameObject pauseButton;

	[SerializeField]
	private GameObject pausePanel;

	[SerializeField]
	private Button screenShakeButton;

	[SerializeField]
	private Animator redAlertPanelAnimator;

	[SerializeField]
	private GameObject skipIGPanel;

	[SerializeField]
	private List<Sprite> screenShakeSprites;

	[SerializeField]
	private TextMeshProUGUI explanationText;

	private bool isUsingScreenShake;

	private string currentLevelString;

	private string currentLevelCheckpointDeathString;

	private string currentLevelCheckpointRedSeenString;

	private int amountOfDeathTotal;

	private int amountOfDeathInCurrentSection;

	private int amountOfRedSeenInCurrentSection;

	private bool gameHasStarted;

	private bool gameIsPaused;

	private float timeSinceStart;

	private int currentCoinAmount;

	private bool skipPanelHasBeenShown;

	[Header("End Game")]
	[SerializeField]
	private GameObject endGamePanel;

	[SerializeField]
	private GameObject victoryTextFX;

	public TextMeshProUGUI victoryText;

	[SerializeField]
	private TextMeshProUGUI loseExplanationText;

	[SerializeField]
	private GameObject loseExplanationTextGO;

	[SerializeField]
	private TextMeshProUGUI rankText;

	[SerializeField]
	private GameObject nextButton;

	[SerializeField]
	private GameObject restartButton;

	[SerializeField]
	private GameObject skipButton;

	[SerializeField]
	private Slider levelProgressionSlider;

	//[SerializeField]
	//private GameObject triangleIndicator;

	[SerializeField]
	private TextMeshProUGUI currentGlobalLevelText;

	[SerializeField]
	private TextMeshProUGUI nextGlobalLevelText;

	//[SerializeField]
	//private Image nextGlobalLevelImage;

	[SerializeField]
	private Color progreessionColor = Color.black;

	private EventManager eventManager;

	private int currentLevelProgression;

	private string currentCohort;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		Instance = this;
		gamePanel.SetActive(value: true);
		pausePanel.SetActive(value: false);
		pauseButton.SetActive(value: false);
		nextButton.SetActive(value: false);
		restartButton.SetActive(value: false);
		//triangleIndicator.SetActive(value: false);
		skipIGPanel.SetActive(value: false);
		gameIsOver = false;
		gameHasStarted = false;
		gameIsPaused = false;
		finalPlayerRanking = new List<Character>();
		progressionSlider.gameObject.SetActive(value: false);
		endGamePanel.SetActive(value: false);
		currentLevel = PlayerPrefs.GetInt("Level", 1);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(LanguageScript.get_string(3)).Append(" ").Append(currentLevel);
		isUsingScreenShake = ((PlayerPrefs.GetInt("ScreenShake", 1) == 1) ? true : false);
		screenShakeButton.image.sprite = (isUsingScreenShake ? screenShakeSprites[1] : screenShakeSprites[0]);
		currentCoinAmount = PlayerPrefs.GetInt("Coins", 0);
		PrepareProgressionSlider();
		levelText.text = PlayerPrefs.GetInt("Level", 1).ToString();
		Time.timeScale = 1f;
		currentLevelProgression = 0;
	}

	private void Start()
	{
		// currentCohort = VoodooSauce.GetPlayerCohort();
		// if (currentLevel == 1)
		// {
		// 	if (currentCohort == "Hold_to_move")
		// 	{
		// 		explanationText.text = LanguageScript.get_string(4);
		// 	}
		// 	else
		// 	{
		// 		explanationText.text = LanguageScript.get_string(5);
		// 	}
		// 	//explanationText.AnimationManager.PlayAnimation();
		// }
		// else
		// {
			explanationText.gameObject.SetActive(value: false);
		// }
		currentLevelString = $"{currentLevel:D5}";
		currentLevelCheckpointDeathString = $"{currentLevel:D5}CheckpointsDeaths";
		currentLevelCheckpointRedSeenString = $"{currentLevel:D5}CheckpointsRedSeen";
		amountOfDeathTotal = 0;
		amountOfDeathInCurrentSection = 0;
		amountOfRedSeenInCurrentSection = 0;
		progressionSlider.maxValue = Mathf.Abs((endLineTransform.position - playerTransform.position).z);
		//VoodooSauce.OnGameStarted();
		//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelString);
		eventManager = EventManager.Instance;
		eventManager.AddListener<PlayerLostEvent>(OnPlayerLosing);
		eventManager.AddListener<GameStartedEvent>(OnGameStarting);
		eventManager.AddListener<PlayerHasWonEvent>(OnPlayerWinning);
		eventManager.AddListener<CounterIsTurningEvent>(OnCounterTurning);
		UpdateSlider();
		
		if (currentLevel == 1)
		{
			TutorialLevelSetup();
		}
		StartCoroutine(StartDelay());
	}

	internal void InitPlayers(List<Character> characterScripts)
	{
		this.characterScripts.AddRange(characterScripts);
		playerTransform = characterScripts[0].transform;
		playerController = playerTransform.GetComponent<PlayerController>();
		playerStartingPosition = playerController.transform.position;
	}

	private void PrepareProgressionSlider()
	{
		int num = (int)Mathf.Ceil((float)currentLevel / 3f);
		int num2 = num + 1;
		int num3 = (currentLevel - 1) % 3;
		currentGlobalLevelText.text = num.ToString();
		nextGlobalLevelText.text = num2.ToString();
		switch (num3)
		{
		case 1:
			levelProgressionSlider.value = 0.33f;
			break;
		case 2:
			levelProgressionSlider.value = 0.66f;
			break;
		}
	}

	private void Update()
	{
		if (!gameIsOver && gameHasStarted)
		{
			UpdateSlider();
			timeSinceStart += Time.deltaTime;
			if (!skipPanelHasBeenShown && amountOfDeathTotal >= 2)
			{
				skipPanelHasBeenShown = true;
				skipIGPanel.SetActive(value: true);
			}
		}
	}

	private void TutorialLevelSetup()
	{
		if (characterScripts.Count > 1)
		{
			characterScripts[3].DeactivateCharacter();
			characterScripts[4].DeactivateCharacter();
		}
	}

	private void UpdateSlider()
	{
		progressionSlider.value = progressionSlider.maxValue - Mathf.Abs((endLineTransform.position - playerTransform.position).z);
	}

	private IEnumerator StartDelay()
	{
		yield return new WaitForSeconds(0.5f);
		eventManager.QueueEvent(new FirstStartCountingEvent());
		timeSinceStart = 0f;
		gameHasStarted = true;
	}

	private IEnumerator CheckForCrownWielder()
	{
		while (!gameIsOver)
		{
			yield return new WaitForSeconds(0.1f);
			SelectCrownWielder(isEndGame: false);
		}
	}

	private void SelectCrownWielder(bool isEndGame)
	{
		float num = -0.1f;
		float num2 = -0.1f;
		currentBestPlayer = null;
		currentSecondPlayer = null;
		for (int i = 0; i < characterScripts.Count; i++)
		{
			if ((!isEndGame && characterScripts[i].HasLost()) || characterScripts[i].HasWon())
			{
				continue;
			}
			float distanceRun = characterScripts[i].GetDistanceRun();
			if (distanceRun > num)
			{
				if (currentBestPlayer != null && distanceRun > num2)
				{
					currentSecondPlayer = currentBestPlayer;
					num2 = num;
				}
				num = distanceRun;
				currentBestPlayer = characterScripts[i];
			}
			else if (distanceRun > num2)
			{
				currentSecondPlayer = characterScripts[i];
				num2 = distanceRun;
			}
		}
		if (currentBestPlayer != null)
		{
			currentBestPlayer.ManageCrown(shouldBeActivated: true);
		}
		for (int j = 0; j < characterScripts.Count; j++)
		{
			if (characterScripts[j] != currentBestPlayer)
			{
				characterScripts[j].ManageCrown(shouldBeActivated: false);
			}
		}
	}

	private List<Character> GetMissingPlayersRanking()
	{
		List<Character> list = new List<Character>();
		List<PlayerScore> list2 = new List<PlayerScore>();
		for (int i = 0; i < characterScripts.Count; i++)
		{
			Character character = characterScripts[i];
			if (!character.HasWon())
			{
				PlayerScore item = new PlayerScore(character, character.GetDistanceRun());
				list2.Add(item);
			}
		}
		list2.Sort((PlayerScore a, PlayerScore b) => -1 * a.CompareTo(b));
		for (int j = 0; j < list2.Count; j++)
		{
			list.Add(list2[j].characterScript);
		}
		return list;
	}

	private void OnGameStarting(GameStartedEvent e)
	{
		progressionSlider.gameObject.SetActive(value: true);
		pauseButton.SetActive(value: true);
		//goAnimator.Play("Go");
		StartCoroutine(CheckForCrownWielder());
		if (currentLevel == 1)
		{
			eventManager.AddListenerOnce<ScreenTouchedEvent>(OnScreenTouched);
		}
	}

	private void OnCounterTurning(CounterIsTurningEvent e)
	{
	}

	public void ShowSkipIGPanel(bool shouldbeShown)
	{
		skipIGPanel.SetActive(shouldbeShown);
	}

	protected void OnGameEnding(bool playerIsOnPodium, int playerRanking)
	{
		if (!gameIsOver)
		{
			gameIsOver = true;
			gamePanel.SetActive(value: false);
			StringBuilder stringBuilder = new StringBuilder();
			if (playerIsOnPodium)
			{
				stringBuilder.Append(LanguageScript.get_string(10));
			}
			else
			{
				stringBuilder.Append(LanguageScript.get_string(11));
			}
			stringBuilder.Append("\n #").Append(playerRanking);
			goText.text = stringBuilder.ToString();
			UpdateProgression();
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelString, playerRanking);
			//VoodooSauce.OnGameFinished(levelComplete: true, playerRanking);
			StartCoroutine(PostEnd(playerIsOnPodium, playerRanking));
		}
	}

	private void OnPlayerWinning(PlayerHasWonEvent e)
	{
		finalPlayerRanking.Add(characterScripts[e.playerId]);
		if (e.playerId == 0)
		{
			int count = finalPlayerRanking.Count;
			bool flag = (currentCohort == "Max3_first_win") ? (count == 1) : (count <= 3);
			GameFinishedEvent evt = new GameFinishedEvent(flag, base.transform);
			eventManager.QueueEvent(evt);
			OnGameEnding(flag, count);
		}
	}

	private IEnumerator PostEnd(bool playerWon, int playerRanking)
	{
		//goAnimator.Play("LongGo");
		yield return new WaitForSeconds(4f);
		List<Character> missingPlayersRanking = GetMissingPlayersRanking();
		finalPlayerRanking.AddRange(missingPlayersRanking);
		eventManager.QueueEvent(new EndScreenAppearingEvent());
		endGamePanel.SetActive(value: true);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("#").Append(playerRanking);
		rankText.text = stringBuilder.ToString();
		if (playerWon)
		{
			UpdateLevel();
			victoryTextFX.gameObject.SetActive(value: true);
			victoryText.text = LanguageScript.get_string(9);
			//victoryTextFX.AnimationManager.PlayAnimation();
			skipButton.SetActive(value: false);
		}
		else
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(LanguageScript.get_string(12));
			if (currentCohort == "Max3_first_win")
			{
				stringBuilder2.Append("1");
			}
			else
			{
				stringBuilder2.Append("3");
			}
			loseExplanationText.text = stringBuilder2.ToString();
			loseExplanationTextGO.SetActive(value: true);
			//if (VoodooSauce.IsRewardedVideoAvailable())
			//{
				skipButton.SetActive(value: true);
			//}
			// else
			// {
			// 	skipButton.SetActive(value: false);
			// }
		}
		StartCoroutine(AddLevelProgression(playerWon));
	}

	private IEnumerator AddLevelProgression(bool playerWon)
	{
		if (playerWon)
		{
			int newSubLevel = (currentLevel - 1) % 3;
			float startingSliderValue = 0f;
			float targetSliderValue = 0f;
			switch (newSubLevel)
			{
			case 1:
				//triangleIndicator.transform.localPosition = new Vector3(-34.8f, -56.26f, 0f);
				startingSliderValue = 0f;
				targetSliderValue = 0.33f;
				break;
			case 2:
				//triangleIndicator.transform.localPosition = new Vector3(34.8f, -56.26f, 0f);
				startingSliderValue = 0.33f;
				targetSliderValue = 0.66f;
				break;
			case 0:
				//triangleIndicator.transform.localPosition = new Vector3(102.5f, -56.26f, 0f);
				startingSliderValue = 0.66f;
				targetSliderValue = 1f;
				break;
			}
			float t = 1f;
			float currentT = 0f;
			while (currentT <= t)
			{
				yield return new WaitForEndOfFrame();
				currentT += Time.deltaTime;
				float value = Mathf.Lerp(startingSliderValue, targetSliderValue, currentT);
				levelProgressionSlider.value = value;
			}
			if (newSubLevel == 0)
			{
				//nextGlobalLevelImage.color = progreessionColor;
			}
			nextButton.SetActive(value: true);
			//triangleIndicator.SetActive(value: true);
		}
		else
		{
			switch ((currentLevel - 1) % 3)
			{
			case 0:
				//triangleIndicator.transform.localPosition = new Vector3(-101.5f, -56.26f, 0f);
				break;
			case 1:
				//triangleIndicator.transform.localPosition = new Vector3(-34.8f, -56.26f, 0f);
				break;
			case 2:
				//triangleIndicator.transform.localPosition = new Vector3(34.8f, -56.26f, 0f);
				break;
			}
			restartButton.SetActive(value: true);
		}
		//triangleIndicator.SetActive(value: true);
	}

	public void WatchRewardAdd()
	{
		//VoodooSauce.ShowRewardedVideo(OnRewardAddWatched);
		if (gameIsOver)
		{
			UpdateLevel();
			restartButton.SetActive(value: false);
			//triangleIndicator.SetActive(value: false);
			skipButton.SetActive(value: false);
			StartCoroutine(AddLevelProgression(playerWon: true));
		}
		else
		{
			skipIGPanel.SetActive(value: false);
			UpdateLevel();
			Restart();
		}}

	private void OnRewardAddWatched(bool watchedTillTheEnd)
	{
		if (watchedTillTheEnd)
		{
			
		}
	}

	private void UpdateLevel()
	{
		currentLevel++;
		PlayerPrefs.SetInt("Level", currentLevel);
		if (PlayerPrefs.GetInt("ExistingLevel", 0) == 1)
		{
			PlayerPrefs.DeleteKey("ExistingLevel");
		}
		PlayerPrefs.Save();
	}

	private void OnPlayerLosing(PlayerLostEvent e)
	{
		if (gameIsOver)
		{
			return;
		}
		new StringBuilder();
		if (e.playerId == 0)
		{
			if (e.isByCounter)
			{
				amountOfRedSeenInCurrentSection++;
			}
			else
			{
				amountOfDeathInCurrentSection++;
			}
			amountOfDeathTotal++;
		}
	}

	public void UpdateProgression()
	{
		currentLevelProgression++;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(currentLevelCheckpointDeathString);
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.Append(currentLevelCheckpointRedSeenString);
		if (currentLevelProgression == 1)
		{
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelCheckpointDeathString);
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelCheckpointRedSeenString);
			stringBuilder.Append("_01");
			stringBuilder2.Append("_01");
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelCheckpointDeathString, stringBuilder.ToString());
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelCheckpointRedSeenString, stringBuilder2.ToString());
		}
		else if (currentLevelProgression == 2)
		{
			stringBuilder.Append("_01");
			stringBuilder2.Append("_01");
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointDeathString, stringBuilder.ToString(), amountOfDeathInCurrentSection);
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointRedSeenString, stringBuilder2.ToString(), amountOfRedSeenInCurrentSection);
			stringBuilder = new StringBuilder();
			stringBuilder2 = new StringBuilder();
			stringBuilder.Append(currentLevelCheckpointDeathString).Append("_02");
			stringBuilder2.Append(currentLevelCheckpointRedSeenString).Append("_02");
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelCheckpointDeathString, stringBuilder.ToString());
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelCheckpointRedSeenString, stringBuilder2.ToString());
		}
		else if (currentLevelProgression == 3)
		{
			stringBuilder.Append("_02");
			stringBuilder2.Append("_02");
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointDeathString, stringBuilder.ToString(), amountOfDeathInCurrentSection);
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointRedSeenString, stringBuilder2.ToString(), amountOfRedSeenInCurrentSection);
			if (currentLevel == 1)
			{
				//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointDeathString);
				//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointRedSeenString);
			}
			else
			{
				stringBuilder = new StringBuilder();
				stringBuilder2 = new StringBuilder();
				stringBuilder.Append(currentLevelCheckpointDeathString).Append("_03");
				stringBuilder2.Append(currentLevelCheckpointRedSeenString).Append("_03");
				//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelCheckpointDeathString, stringBuilder.ToString());
				//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelCheckpointRedSeenString, stringBuilder2.ToString());
			}
		}
		else if (currentLevelProgression == 4)
		{
			stringBuilder.Append("_03");
			stringBuilder2.Append("_03");
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointDeathString, stringBuilder.ToString(), amountOfDeathInCurrentSection);
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointRedSeenString, stringBuilder2.ToString(), amountOfRedSeenInCurrentSection);
			stringBuilder = new StringBuilder();
			stringBuilder2 = new StringBuilder();
			stringBuilder.Append(currentLevelCheckpointDeathString).Append("_04");
			stringBuilder2.Append(currentLevelCheckpointRedSeenString).Append("_04");
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelCheckpointDeathString, stringBuilder.ToString());
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevelCheckpointRedSeenString, stringBuilder2.ToString());
		}
		else if (currentLevelProgression == 5)
		{
			stringBuilder.Append("_04");
			stringBuilder2.Append("_04");
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointDeathString, stringBuilder.ToString(), amountOfDeathInCurrentSection);
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointRedSeenString, stringBuilder2.ToString(), amountOfRedSeenInCurrentSection);
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointDeathString);
			//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelCheckpointRedSeenString);
		}
		amountOfDeathInCurrentSection = 0;
		amountOfRedSeenInCurrentSection = 0;
	}

	public void AddCoinToPlayer()
	{
		currentCoinAmount++;
		PlayerPrefs.SetInt("Coins", currentCoinAmount);
		PlayerPrefs.Save();
	}

	private IEnumerator PlayFireworks()
	{
		for (int i = 0; i < fireworks.Count; i++)
		{
			GameObject gameObject = fireworks[i].gameObject;
			gameObject.transform.localPosition = new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(0f, 8f), 20f);
			gameObject.SetActive(value: true);
			fireworks[i].Play();
			yield return new WaitForSeconds(0.4f);
		}
		StartCoroutine(PlayFireworks());
	}

	private void OnScreenTouched(ScreenTouchedEvent e)
	{
		explanationText.gameObject.SetActive(value: false);
	}

	public void ChangeScreenShakePreference()
	{
		isUsingScreenShake = !isUsingScreenShake;
		PlayerPrefs.SetInt("ScreenShake", isUsingScreenShake ? 1 : 0);
		PlayerPrefs.Save();
		screenShakeButton.image.sprite = (isUsingScreenShake ? screenShakeSprites[1] : screenShakeSprites[0]);
	}

	public void PauseGame(bool paused)
	{
		gameIsPaused = paused;
		if (gameIsPaused)
		{
			if (!gameIsOver)
			{
				pausePanel.SetActive(value: true);
				Time.timeScale = 0f;
			}
		}
		else
		{
			pausePanel.SetActive(value: false);
			Time.timeScale = 1f;
		}
	}

	public void GoToNextLevel()
	{
		//VoodooSauce.ShowInterstitial();
		Restart();
	}

	public void OnRestartButtonPushed()
	{
		//VoodooSauce.OnGameFinished(levelComplete: false, 0f);
		//VoodooSauce.ShowInterstitial();
		Restart();
	}

	public void RestartLevel()
	{
		//VoodooSauce.ShowInterstitial();
		Restart();
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene(0);
	}

	public Character GetCurrentlyFirstPlayer()
	{
		return currentBestPlayer;
	}

	public Character GetCurrentSecondPlayer()
	{
		return currentSecondPlayer;
	}

	public float GetDistanceRunByPlayer()
	{
		return Mathf.Abs((playerTransform.position - playerStartingPosition).z);
	}

	public bool IsUsingScreenshakes()
	{
		return isUsingScreenShake;
	}

	private void OnApplicationPause(bool paused)
	{
	}
}
