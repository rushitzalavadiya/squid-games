using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LevelInitializer : MonoBehaviour
{
	private int currentLevel;

	[SerializeField]
	private GameManager gameManager;

	[SerializeField]
	private CameraManager cameraManager;

	[SerializeField]
	private List<Material> sandMaterials;

	[SerializeField]
	private List<Color> groundColorList;

	[SerializeField]
	private Material hazardMaterial;

	[SerializeField]
	private List<Color> hazardColorList;

	[SerializeField]
	private Material subHazardMaterial;

	[SerializeField]
	private List<Color> subHazardColorList;

	[SerializeField]
	private List<GameObject> possibleSequences = new List<GameObject>();

	[SerializeField]
	private List<Transform> platforms = new List<Transform>();

	[SerializeField]
	private List<GameObject> prefabPlayers = new List<GameObject>();

	[SerializeField]
	private List<GameObject> prefabAI = new List<GameObject>();

	[SerializeField]
	private List<GameObject> prefabCounter = new List<GameObject>();

	private List<GameObject> characters = new List<GameObject>();

	private List<Character> characterScripts = new List<Character>();

	[SerializeField]
	private GameObject startingLine;

	[Header("Data for characters")]
	[SerializeField]
	private List<GameObject> nameTexts = new List<GameObject>();

	[SerializeField]
	private Text sneakyBonusText;

	[SerializeField]
	private Animator sneakyBonusAnimator;

	[SerializeField]
	private Transform wallTransform;

	[SerializeField]
	private Transform frontBlockTransform;

	[SerializeField]
	private GameObject bubble;

	[SerializeField]
	private Animator bubbleAnimator;

	[SerializeField]
	private Image bubbleImage;

	[SerializeField]
	private Text bubbleText;

	[SerializeField]
	private List<Transform> startingPositions = new List<Transform>();

	[SerializeField]
	private Vector3 counterPosition = Vector3.zero;

	private bool sequence25IsUsed;

	private bool sequence27IsUsed;

	private List<string> listPlayerNames = new List<string>(new string[211]
	{
		"Isuka",
		"Benji",
		"Guillaume",
		"Arnaud",
		"Thomas",
		"Romeo",
		"Aurelien",
		"Bahamut",
		"Isuka",
		"Diremas",
		"cannelle",
		"sorry",
		"Frite",
		"mrprof",
		"vriska",
		"d4rksky",
		"Canada",
		"ノラ",
		"JoJo",
		"Geb",
		"Reggie",
		"lol",
		"husami",
		"Ash",
		"Pikachu",
		"Nikolai",
		"Dima",
		"thelord",
		"Ken",
		"Ifrit",
		"zizou",
		"Rocket",
		"Batman",
		"zlatan",
		"Messi",
		"Rock",
		"zoz06",
		"Ronaldo",
		"Murica",
		"pasta",
		"Nikki",
		"Ezio",
		"Master",
		"dogo",
		"rarara",
		"meka",
		"cloud",
		"kokyo",
		"beba",
		"Knight",
		"tony61",
		"Kaloyan",
		"uza",
		"カズオ",
		"Алексей",
		"Mish",
		"Light",
		"Dark",
		"Sam",
		"たけ",
		"あゆ",
		"Cool",
		"Zap",
		"Lena",
		"伸夫",
		"Mantis",
		"Sponge",
		"jazz",
		"Fut",
		"didi91",
		"Mitch",
		"Xana",
		"Jack",
		"Naruto",
		"Ichigo",
		"Luffy",
		"Doflamingo",
		"Frog",
		"Ouioui",
		"Zultrin",
		"Pam",
		"Snegir",
		"Агата",
		"Надя",
		"Паша",
		"Faker",
		"Brownie",
		"cello",
		"gAmZeE",
		"Yoshi",
		"Joker",
		"Fire",
		"Maxime",
		"Zwei",
		"聡",
		"Wilhelm",
		"Eren",
		"nate",
		"billy",
		"zii",
		"harry",
		"KARKAT",
		"bart",
		"Vellemer",
		"Ninatha",
		"Lole",
		"Fthanafh",
		"happy",
		"Fukuo",
		"LILI",
		"Sora",
		"Robin",
		"Roger",
		"Kris",
		"Aaron",
		"Craig",
		"ferNANdo",
		"Blindo",
		"Spyro",
		"Ryne",
		"Roxy",
		"Bopeep",
		"granger",
		"Scott",
		"Artheon",
		"Mercy",
		"Caitlyn",
		"Jesse",
		"Mewtwo",
		"Komn",
		"Snow",
		"Ocelot",
		"Carolin",
		"Excelsior",
		"Stan",
		"Hunter",
		"Natsu",
		"Captain",
		"Ludo",
		"Jotaro",
		"Drifter",
		"Onigiri",
		"Kamui",
		"mArt",
		"scrub",
		"KORB",
		"Timo",
		"mark15",
		"666",
		"Oni",
		"congratz",
		"luna",
		"drstrange",
		"Kirby",
		"Jo",
		"Shark",
		"lordDwarf",
		"avenger",
		"atrgez",
		"momo",
		"茜",
		"勝雄",
		"美紀",
		"sayuri",
		"monika",
		"Bob",
		"Kameha",
		"dragon",
		"pom",
		"TheBoss",
		"NumberOne",
		"shrek",
		"Coco",
		"Fairy",
		"Ursa",
		"loil",
		"Spinozio",
		"Pendragon",
		"Jiminy",
		"Pop",
		"Sakura",
		"Matthew",
		"Thor",
		"Gamer",
		"Oculus",
		"SaxBoy",
		"Daenerys",
		"Loutre",
		"Bunny",
		"Anger",
		"Eevee",
		"Brent",
		"Perceval",
		"PoPPy",
		"=PyC=",
		"aaa",
		"Боксер",
		"Лост",
		"алоха",
		"индиго",
		"Olyas",
		"Candy",
		"Racoon",
		"Minou",
		"猫",
		"은서",
		"승호",
		"경미",
		"지원",
		"Lea",
		"영미"
	});

	private void Awake()
	{
		InitLevel();
		InitPlayers();
		gameManager.InitPlayers(characterScripts);
		cameraManager.SetTarget(characters[0].transform);
	}

	private void Start()
	{
		EventManager instance = EventManager.Instance;
	}

	private void InitPlayers()
	{
		int @int = PlayerPrefs.GetInt("Model", 0);
		GameObject gameObject = UnityEngine.Object.Instantiate(prefabPlayers[@int]);
		characters.Add(gameObject);
		gameObject.transform.eulerAngles = Vector3.zero;
		gameObject.GetComponent<PlayerController>().InitPlayerController(nameTexts[0], sneakyBonusText, sneakyBonusAnimator);
		for (int i = 0; i < 4; i++)
		{
			int index = UnityEngine.Random.Range(0, prefabAI.Count);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(prefabAI[index]);
			characters.Add(gameObject2);
			gameObject2.transform.eulerAngles = Vector3.zero;
			gameObject2.GetComponent<AIController>().InitAIController(nameTexts[i + 1]);
		}
		for (int j = 0; j < characters.Count; j++)
		{
			Character component = characters[j].GetComponent<Character>();
			component.SetPlayerId(j);
			characterScripts.Add(component);
		}
		int index2 = UnityEngine.Random.Range(0, prefabCounter.Count);
		GameObject gameObject3 = UnityEngine.Object.Instantiate(prefabCounter[index2]);
		gameObject3.transform.eulerAngles = new Vector3(0f, 180f, 0f);
		CountingCharacter component2 = gameObject3.GetComponent<CountingCharacter>();
		component2.InitCountingCharacter(wallTransform, frontBlockTransform, bubble, bubbleAnimator, bubbleImage, bubbleText);
		component2.transform.position = counterPosition;
		SetPlayerNames();
		RandomlyPlacePlayers();
	}

	private void InitLevel()
	{
		currentLevel = PlayerPrefs.GetInt("Level", 1);
		int index = ((int)Mathf.Ceil((float)currentLevel / 3f) - 1) % groundColorList.Count;
		for (int i = 0; i < sandMaterials.Count; i++)
		{
			sandMaterials[i].color = groundColorList[index];
		}
		hazardMaterial.color = hazardColorList[index];
		subHazardMaterial.color = subHazardColorList[index];
		if (currentLevel == 1)
		{
			GameObject platform = UnityEngine.Object.Instantiate(possibleSequences[0], platforms[2]);
			Object.Instantiate(possibleSequences[0], platforms[3]);
			startingLine.transform.localPosition = new Vector3(0f, 0f, -30f);
			RemoveCheckpoint(platform);
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Levels/level").Append(currentLevel);
		TextAsset textAsset = Resources.Load<TextAsset>(stringBuilder.ToString());
		if (textAsset != null)
		{
			string json;
			using (StreamReader streamReader = new StreamReader(new MemoryStream(textAsset.bytes)))
			{
				json = streamReader.ReadToEnd();
			}
			LevelData levelData = JsonUtility.FromJson<LevelData>(json);
			InstantiatePlatformFromResources(levelData.firstPlatformId, 0, levelData.firstPlatformShouldBeReversed);
			InstantiatePlatformFromResources(levelData.secondPlatformId, 1, levelData.secondPlatformShouldBeReversed);
			InstantiatePlatformFromResources(levelData.thirdPlatformId, 2, levelData.thirdPlatformShouldBeReversed);
			InstantiatePlatformFromResources(levelData.fourthPlatformId, 3, levelData.fourthPlatformShouldBeReversed);
			return;
		}
		if (PlayerPrefs.GetInt("ExistingLevel", 0) == 1)
		{
			for (int j = 0; j < platforms.Count; j++)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append("ExistingLevel_").Append(j);
				int @int = PlayerPrefs.GetInt(stringBuilder2.ToString(), 0);
				GameObject gameObject = possibleSequences[@int];
				GameObject platform2 = UnityEngine.Object.Instantiate(gameObject, platforms[j]);
				if (j == 0)
				{
					RemoveCheckpoint(platform2);
				}
				possibleSequences.Remove(gameObject);
			}
			return;
		}
		for (int k = 0; k < platforms.Count; k++)
		{
			int num = UnityEngine.Random.Range(0, possibleSequences.Count);
			GameObject gameObject2 = possibleSequences[num];
			GameObject platform3 = UnityEngine.Object.Instantiate(gameObject2, platforms[k]);
			if (k == 0)
			{
				RemoveCheckpoint(platform3);
			}
			possibleSequences.Remove(gameObject2);
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.Append("ExistingLevel_").Append(k);
			PlayerPrefs.SetInt(stringBuilder3.ToString(), num);
		}
		PlayerPrefs.SetInt("ExistingLevel", 1);
		PlayerPrefs.Save();
	}

	private void InstantiatePlatformFromResources(string platformId, int placingID, bool shouldBeReversed)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Platforms/Platform_").Append(platformId);
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(stringBuilder.ToString(), typeof(GameObject)), platforms[placingID]) as GameObject;
		Transform transform = gameObject.transform;
		if (shouldBeReversed)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Hazard component = transform.GetChild(i).GetComponent<Hazard>();
				if (component != null && component.CanBeReversed())
				{
					component.ReverseObject();
				}
			}
		}
		if (placingID == 0)
		{
			RemoveCheckpoint(gameObject);
		}
		if (platformId == "25")
		{
			sequence25IsUsed = true;
		}
		else if (platformId == "27")
		{
			sequence27IsUsed = true;
		}
	}

	private void RemoveCheckpoint(GameObject platform)
	{
		platform.transform.Find("Checkpoint").GetComponent<Checkpoint>().DeactivateCheckpointElements();
	}

	private void RandomlyPlacePlayers()
	{
		int i = 0;
		int num = 5;
		if (currentLevel == 1 /*|| VoodooSauce.GetPlayerCohort() == "Max3_first_win"*/)
		{
			startingPositions.RemoveAt(0);
			startingPositions.RemoveAt(startingPositions.Count - 1);
			num = 3;
			characters[3].SetActive(value: false);
			characters[4].SetActive(value: false);
		}
		for (; i < num; i++)
		{
			Transform transform = (i != 0) ? startingPositions[Random.Range(0, startingPositions.Count)] : ((sequence25IsUsed && sequence27IsUsed) ? startingPositions[2] : ((!sequence25IsUsed) ? ((!sequence27IsUsed) ? startingPositions[Random.Range(0, startingPositions.Count)] : startingPositions[Random.Range(2, startingPositions.Count)]) : startingPositions[Random.Range(0, 3)]));
			characterScripts[i].transform.position = transform.position;
			startingPositions.Remove(transform);
		}
	}

	private IEnumerator PrepareNextLevel()
	{
		yield return new WaitForSeconds(1f);
		InitLevel();
	}

	private void SetPlayerNames()
	{
		List<int> list = new List<int>();
		for (int i = 1; i < characterScripts.Count; i++)
		{
			int num = UnityEngine.Random.Range(0, listPlayerNames.Count);
			if (!list.Contains(num))
			{
				list.Add(num);
				characterScripts[i].SetPlayerName(listPlayerNames[num]);
			}
		}
	}
}
