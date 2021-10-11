using System;
using UnityEngine;
using UnityEngine.UI;

public class TCP2_Demo_Cat : MonoBehaviour
{
	[Serializable]
	public class Ambience
	{
		public string name;

		public GameObject[] activate;

		public Material skybox;
	}

	[Serializable]
	public class ShaderStyle
	{
		[Serializable]
		public class CharacterSettings
		{
			public Material material;

			public Renderer[] renderers;
		}

		public string name;

		public CharacterSettings[] settings;
	}

	public Ambience[] ambiences;

	public int amb;

	[Space]
	public ShaderStyle[] styles;

	public int style;

	[Space]
	public GameObject shadedGroup;

	public GameObject flatGroup;

	[Space]
	public GameObject[] cats;

	public GameObject[] unityChans;

	public GameObject unityChanCopyright;

	[Space]
	public AnimationClip[] catAnimations;

	private int catAnim;

	public AnimationClip[] unityChanAnimations;

	private int uchanAnim;

	private float animTime;

	[Space]
	public Light[] dirLights;

	public Light[] otherLights;

	public Transform rotatingPointLights;

	[Space]
	public Button[] ambiencesButtons;

	public Button[] stylesButtons;

	public Button[] characterButtons;

	public Button[] textureButtons;

	public Button[] animationButtons;

	[Space]
	public Canvas canvas;

	public bool animate
	{
		get;
		set;
	}

	public bool rotateLights
	{
		get;
		set;
	}

	public bool rotatePointLights
	{
		get;
		set;
	}

	private void Awake()
	{
		rotatePointLights = true;
		rotateLights = false;
		animate = true;
		SetAmbience(0);
		SetStyle(0);
		SetCat(cat: true);
		SetFlat(flat: false);
		SetAnimation(0);
	}

	private void Update()
	{
		if (rotateLights)
		{
			Light[] array = dirLights;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].transform.Rotate(Vector3.up * Time.deltaTime * -30f, Space.World);
			}
		}
		if (rotatePointLights)
		{
			rotatingPointLights.transform.Rotate(Vector3.up * 50f * Time.deltaTime, Space.World);
		}
		if (animate)
		{
			animTime += Time.deltaTime;
			UpdateAnimation();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Tab))
		{
			if (UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift))
			{
				SetStyle(--style);
			}
			else
			{
				SetStyle(++style);
			}
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.H))
		{
			canvas.enabled = !canvas.enabled;
		}
	}

	private void UpdateAnimation()
	{
		GameObject[] array = cats;
		foreach (GameObject gameObject in array)
		{
			if (gameObject.activeInHierarchy)
			{
				catAnimations[catAnim].SampleAnimation(gameObject, animTime);
			}
		}
		array = unityChans;
		foreach (GameObject gameObject2 in array)
		{
			if (gameObject2.activeInHierarchy)
			{
				unityChanAnimations[uchanAnim].SampleAnimation(gameObject2, animTime);
			}
		}
	}

	public void SetAmbience(int index)
	{
		Ambience[] array = ambiences;
		GameObject[] activate;
		for (int i = 0; i < array.Length; i++)
		{
			activate = array[i].activate;
			for (int j = 0; j < activate.Length; j++)
			{
				activate[j].SetActive(value: false);
			}
		}
		amb = index % ambiences.Length;
		Ambience ambience = ambiences[amb];
		activate = ambience.activate;
		for (int i = 0; i < activate.Length; i++)
		{
			activate[i].SetActive(value: true);
		}
		RenderSettings.skybox = ambience.skybox;
		DynamicGI.UpdateEnvironment();
		for (int k = 0; k < ambiencesButtons.Length; k++)
		{
			ColorBlock colors = ambiencesButtons[k].colors;
			colors.colorMultiplier = ((k == index) ? 0.96f : 0.6f);
			ambiencesButtons[k].colors = colors;
		}
	}

	public void SetStyle(int index)
	{
		if (index < 0)
		{
			index = styles.Length - 1;
		}
		if (index >= styles.Length)
		{
			index = 0;
		}
		style = index;
		ShaderStyle.CharacterSettings[] settings = styles[style].settings;
		foreach (ShaderStyle.CharacterSettings characterSettings in settings)
		{
			Renderer[] renderers = characterSettings.renderers;
			for (int j = 0; j < renderers.Length; j++)
			{
				renderers[j].sharedMaterial = characterSettings.material;
			}
		}
		for (int k = 0; k < stylesButtons.Length; k++)
		{
			ColorBlock colors = stylesButtons[k].colors;
			colors.colorMultiplier = ((k == index) ? 0.96f : 0.6f);
			stylesButtons[k].colors = colors;
		}
	}

	public void SetFlat(bool flat)
	{
		shadedGroup.SetActive(!flat);
		flatGroup.SetActive(flat);
		UpdateAnimation();
		for (int i = 0; i < textureButtons.Length; i++)
		{
			ColorBlock colors = textureButtons[i].colors;
			colors.colorMultiplier = (((i == 1 && flat) || (i == 0 && !flat)) ? 0.96f : 0.6f);
			textureButtons[i].colors = colors;
		}
	}

	public void SetCat(bool cat)
	{
		GameObject[] array = cats;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(cat);
		}
		array = unityChans;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!cat);
		}
		UpdateAnimation();
		unityChanCopyright.SetActive(!cat);
		for (int j = 0; j < characterButtons.Length; j++)
		{
			ColorBlock colors = characterButtons[j].colors;
			colors.colorMultiplier = (((j == 0 && cat) || (j == 1 && !cat)) ? 0.96f : 0.6f);
			characterButtons[j].colors = colors;
		}
	}

	public void SetLightShadows(bool on)
	{
		Light[] array = dirLights;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].shadows = (on ? LightShadows.Soft : LightShadows.None);
		}
		array = otherLights;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].shadows = (on ? LightShadows.Soft : LightShadows.None);
		}
	}

	public void SetAnimation(int index)
	{
		catAnim = index;
		if (catAnim >= catAnimations.Length)
		{
			catAnim = 0;
		}
		if (catAnim < 0)
		{
			catAnim = catAnimations.Length - 1;
		}
		uchanAnim = index;
		if (uchanAnim >= unityChanAnimations.Length)
		{
			uchanAnim = 0;
		}
		if (uchanAnim < 0)
		{
			uchanAnim = unityChanAnimations.Length - 1;
		}
		for (int i = 0; i < animationButtons.Length; i++)
		{
			ColorBlock colors = animationButtons[i].colors;
			colors.colorMultiplier = ((i == index) ? 0.96f : 0.6f);
			animationButtons[i].colors = colors;
		}
	}
}
