using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
	private string playerName;

	private int currentLevel;

	private int coins;

	[SerializeField]
	private InputField nameField;

	[SerializeField]
	private SkinnedMeshRenderer playerRenderer;

	[SerializeField]
	private List<GameObject> characters;

	private int characterIndex;

	private bool isUsingScreenshake;

	[SerializeField]
	private Button screenShakeButton;

	[SerializeField]
	private List<Sprite> screenShakeSprites = new List<Sprite>();

	[SerializeField]
	private Text goldText;

	[SerializeField]
	private Slider levelProgressionSlider;

	[SerializeField]
	private TextMeshProUGUI currentGlobalLevelText;

	[SerializeField]
	private TextMeshProUGUI nextGlobalLevelText;

	[SerializeField]
	private GameObject configurationPanel;

	[SerializeField]
	private GameObject configurationButton;

	[SerializeField]
	private GameObject restorePurchaseButton;

	[SerializeField]
	private GameObject gdprButton;

	[SerializeField]
	private GameObject confirmMenu;

	[SerializeField]
	private Text confirmMessage;

	private bool restorePurchaseButtonHasBeenPushed;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		//VoodooSauce.RegisterPurchaseDelegate(this);
	}

	private void Start()
	{
		LoadStats();
		SetCharacterModel();
		//VoodooSauce.ShowCrossPromo();
		//VoodooSauce.ShowBanner();
		//VoodooSauce.RequestGdprApplicability(delegate(bool is_applicable)
		// {
		// 	if (is_applicable)
		// 	{
		// 		VoodooSauce.ShowGDPRBanner();
		// 		gdprButton.SetActive(value: true);
		// 	}
		// 	else
		// 	{
		// 		
		// 	}
		// });
		
		gdprButton.SetActive(value: true);
	}

	public void OnStartButtonClicked()
	{
		SceneManager.LoadScene(1);
	}

	public void OnSecondStartButtonClicked()
	{
		SceneManager.LoadScene(2);
	}

	private void LoadStats()
	{
		if (!PlayerPrefs.HasKey("Name"))
		{
			playerName = LanguageScript.get_string(2);
			currentLevel = 1;
			characterIndex = 0;
			isUsingScreenshake = true;
			coins = 0;
			PlayerPrefs.SetString("Name", "Player");
			PlayerPrefs.SetInt("Level", 1);
			PlayerPrefs.SetInt("Model", 0);
			PlayerPrefs.SetInt("ScreenShake", 1);
			PlayerPrefs.SetInt("Coins", 0);
			PlayerPrefs.Save();
		}
		else
		{
			playerName = PlayerPrefs.GetString("Name", "Player");
			currentLevel = PlayerPrefs.GetInt("Level", 1);
			characterIndex = PlayerPrefs.GetInt("Model", 0);
			isUsingScreenshake = (PlayerPrefs.GetInt("ScreenShake", 1) == 1);
			coins = PlayerPrefs.GetInt("Coins", 0);
		}
		nameField.text = playerName;
		new StringBuilder().Append("Level ").Append(currentLevel);
		goldText.text = coins.ToString();
		screenShakeButton.image.sprite = (isUsingScreenshake ? screenShakeSprites[1] : screenShakeSprites[0]);
		PrepareProgressionSlider();
	}

	public void OnPlayerNameUpdate(InputField input)
	{
		if (input.text == string.Empty)
		{
			input.text = LanguageScript.get_string(2);
			nameField.text = LanguageScript.get_string(2);
		}
		playerName = input.text;
		PlayerPrefs.SetString("Name", playerName);
		PlayerPrefs.Save();
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

	public void SwitchCharacterModel(bool isRight)
	{
		if (isRight)
		{
			characterIndex++;
			characterIndex = ((characterIndex < characters.Count) ? characterIndex : 0);
		}
		else
		{
			characterIndex--;
			characterIndex = ((characterIndex < 0) ? (characters.Count - 1) : characterIndex);
		}
		PlayerPrefs.SetInt("Model", characterIndex);
		PlayerPrefs.Save();
		SetCharacterModel();
	}

	private void SetCharacterModel()
	{
		for (int i = 0; i < characters.Count; i++)
		{
			if (i != characterIndex)
			{
				characters[i].SetActive(value: false);
			}
		}
		characters[characterIndex].SetActive(value: true);
	}

	public void ChangeScreenShakePreference()
	{
		isUsingScreenshake = !isUsingScreenshake;
		PlayerPrefs.SetInt("ScreenShake", isUsingScreenshake ? 1 : 0);
		PlayerPrefs.Save();
		screenShakeButton.image.sprite = (isUsingScreenshake ? screenShakeSprites[1] : screenShakeSprites[0]);
	}

	public void OpenConfigurationMenu(bool shouldBeOpened)
	{
		if (shouldBeOpened)
		{
			//VoodooSauce.HideCrossPromo();
		}
		else
		{
			//VoodooSauce.ShowCrossPromo();
		}
		configurationPanel.SetActive(shouldBeOpened);
	}

	public void OpenConfirmMenu(bool shouldBeOpened)
	{
		confirmMenu.SetActive(shouldBeOpened);
	}

	public void OnGDPRButtonPushed()
	{
		//VoodooSauce.ShowGDPRSettings();
	}

	public void OnRestorePurchasesPushed()
	{
		restorePurchaseButtonHasBeenPushed = true;
		//VoodooSauce.RestorePurchases();
	}

	public void OnPurchaseComplete(string productId)
	{
		UnityEngine.Debug.Log("OnPurchaseComplete " + productId);
		if (restorePurchaseButtonHasBeenPushed)
		{
			restorePurchaseButtonHasBeenPushed = false;
			confirmMessage.text = LanguageScript.get_string(17);
			OpenConfirmMenu(shouldBeOpened: true);
		}
		else
		{
			confirmMessage.text = LanguageScript.get_string(18);
			OpenConfirmMenu(shouldBeOpened: true);
		}
	}

	public void OnInitializeSuccess()
	{
		UnityEngine.Debug.Log("OnInitializeSuccess");
	}

	// public void OnInitializeFailure(InitializationFailureReason reason)
	// {
	// 	UnityEngine.Debug.Log("OnInitializeFailure");
	// }

	// public void OnPurchaseFailure(string productId, PurchaseFailureReason reason)
	// {
	// 	UnityEngine.Debug.Log("OnPurchaseFailure " + productId + " " + reason);
	// 	if (restorePurchaseButtonHasBeenPushed)
	// 	{
	// 		restorePurchaseButtonHasBeenPushed = false;
	// 		confirmMessage.text = LanguageScript.get_string(19);
	// 		OpenConfirmMenu(shouldBeOpened: true);
	// 	}
	// 	else
	// 	{
	// 		confirmMessage.text = LanguageScript.get_string(20);
	// 		OpenConfirmMenu(shouldBeOpened: true);
	// 	}
	// }
}
