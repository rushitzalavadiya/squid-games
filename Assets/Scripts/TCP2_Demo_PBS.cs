using System;
using UnityEngine;
using UnityEngine.UI;

public class TCP2_Demo_PBS : MonoBehaviour
{
	[Serializable]
	public class SkyboxSetting
	{
		public Material SkyMaterial;

		public Color lightColor;

		public Vector3 DirLightEuler;
	}

	public Light DirLight;

	public GameObject PointLights;

	public MeshRenderer Robot;

	public GameObject Canvas;

	public SkyboxSetting[] SkySettings;

	public bool FlipLight = true;

	public Texture2D[] RampTextures;

	public Slider SmoothnessSlider;

	public Text SmoothnessValue;

	public Slider MetallicSlider;

	public Text MetallicValue;

	public Text BumpScaleValue;

	public Text ShaderText;

	public Text SkyboxValue;

	public Text RampValue;

	public Slider RampThresholdSlider;

	public Text RampThresholdValue;

	public Slider RampSmoothSlider;

	public Text RampSmoothValue;

	public Slider RampSmoothAddSlider;

	public Text RampSmoothAddValue;

	public RawImage RampImage;

	private int currentSky;

	private int currentRamp;

	private Material robotMaterial;

	private bool mUseOutline;

	private bool mRotatePointLights = true;

	public bool ShowPointLights
	{
		set
		{
			PointLights.SetActive(value);
		}
	}

	public bool ShowDirLight
	{
		set
		{
			DirLight.enabled = value;
		}
	}

	public bool RotatePointLights
	{
		get
		{
			return mRotatePointLights;
		}
		set
		{
			mRotatePointLights = value;
		}
	}

	public bool UseOutline
	{
		get
		{
			return mUseOutline;
		}
		set
		{
			mUseOutline = value;
			if (robotMaterial.shader.name.Contains("Toony"))
			{
				ShowTCP2Shader();
			}
		}
	}

	public bool UseRampTexture
	{
		set
		{
			robotMaterial.SetFloat("_TCP2_RAMPTEXT", value ? 1f : 0f);
			if (value)
			{
				robotMaterial.EnableKeyword("TCP2_RAMPTEXT");
			}
			else
			{
				robotMaterial.DisableKeyword("TCP2_RAMPTEXT");
			}
		}
	}

	public bool UseStylizedFresnel
	{
		set
		{
			robotMaterial.SetFloat("_TCP2_STYLIZED_FRESNEL", value ? 1f : 0f);
			if (value)
			{
				robotMaterial.EnableKeyword("TCP2_STYLIZED_FRESNEL");
			}
			else
			{
				robotMaterial.DisableKeyword("TCP2_STYLIZED_FRESNEL");
			}
		}
	}

	public bool UseStylizedSpecular
	{
		set
		{
			robotMaterial.SetFloat("_TCP2_SPEC_TOON", value ? 1f : 0f);
			if (value)
			{
				robotMaterial.EnableKeyword("TCP2_SPEC_TOON");
			}
			else
			{
				robotMaterial.DisableKeyword("TCP2_SPEC_TOON");
			}
		}
	}

	private void Awake()
	{
		robotMaterial = Robot.material;
		mUseOutline = robotMaterial.shader.name.Contains("Outline");
		MetallicSlider.value = robotMaterial.GetFloat("_Metallic");
		SmoothnessSlider.value = robotMaterial.GetFloat("_Glossiness");
		RampThresholdSlider.value = robotMaterial.GetFloat("_RampThreshold");
		RampSmoothSlider.value = robotMaterial.GetFloat("_RampSmooth");
		RampSmoothAddSlider.value = robotMaterial.GetFloat("_RampSmoothAdd");
		UpdateSky();
		UpdateRamp();
	}

	private void Update()
	{
		if (mRotatePointLights)
		{
			PointLights.transform.Rotate(Vector3.up * 20f * Time.deltaTime);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.H))
		{
			Canvas.SetActive(!Canvas.activeSelf);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
		{
			NextSky();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
		{
			PrevSky();
		}
	}

	public void ToggleShader()
	{
		if (robotMaterial.shader.name.Contains("Toony"))
		{
			ShowUnityStandardShader();
			ShaderText.text = "View with TCP2 PBS shader";
		}
		else
		{
			ShowTCP2Shader();
			ShaderText.text = "View with Unity Standard shader";
		}
	}

	public void NextSky()
	{
		currentSky++;
		if (currentSky >= SkySettings.Length)
		{
			currentSky = 0;
		}
		UpdateSky();
	}

	public void PrevSky()
	{
		currentSky--;
		if (currentSky < 0)
		{
			currentSky = SkySettings.Length - 1;
		}
		UpdateSky();
	}

	public void NextRamp()
	{
		currentRamp++;
		if (currentRamp >= RampTextures.Length)
		{
			currentRamp = 0;
		}
		UpdateRamp();
	}

	public void PrevRamp()
	{
		currentRamp--;
		if (currentRamp < 0)
		{
			currentRamp = RampTextures.Length - 1;
		}
		UpdateRamp();
	}

	public void SetMetallic(float f)
	{
		robotMaterial.SetFloat("_Metallic", f);
		MetallicValue.text = f.ToString("0.00");
	}

	public void SetSmoothness(float f)
	{
		robotMaterial.SetFloat("_Glossiness", f);
		SmoothnessValue.text = f.ToString("0.00");
	}

	public void SetBumpScale(float f)
	{
		robotMaterial.SetFloat("_BumpScale", f);
		BumpScaleValue.text = f.ToString("0.00");
	}

	public void SetRampThreshold(float f)
	{
		robotMaterial.SetFloat("_RampThreshold", f);
		RampThresholdValue.text = f.ToString("0.00");
	}

	public void SetRampSmooth(float f)
	{
		robotMaterial.SetFloat("_RampSmooth", f);
		RampSmoothValue.text = f.ToString("0.00");
	}

	public void SetRampSmoothAdd(float f)
	{
		robotMaterial.SetFloat("_RampSmoothAdd", f);
		RampSmoothAddValue.text = f.ToString("0.00");
	}

	private void UpdateRamp()
	{
		robotMaterial.SetTexture("_Ramp", RampTextures[currentRamp]);
		RampValue.text = $"{currentRamp + 1}/{RampTextures.Length}";
		RampImage.texture = RampTextures[currentRamp];
	}

	private void UpdateSky()
	{
		SkyboxSetting skyboxSetting = SkySettings[currentSky];
		DirLight.transform.eulerAngles = skyboxSetting.DirLightEuler;
		if (FlipLight)
		{
			DirLight.transform.Rotate(Vector3.up, 180f, Space.Self);
		}
		DirLight.color = skyboxSetting.lightColor;
		RenderSettings.skybox = skyboxSetting.SkyMaterial;
		RenderSettings.customReflection = (skyboxSetting.SkyMaterial.GetTexture("_Tex") as Cubemap);
		DynamicGI.UpdateEnvironment();
		SkyboxValue.text = $"{currentSky + 1}/{SkySettings.Length}";
	}

	private void ShowUnityStandardShader()
	{
		robotMaterial.shader = Shader.Find("Standard");
	}

	public void ShowTCP2Shader()
	{
		Shader shader = Shader.Find(mUseOutline ? "Hidden/Toony Colors Pro 2/Standard PBS Outline" : "Toony Colors Pro 2/Standard PBS");
		if (shader != null)
		{
			robotMaterial.shader = shader;
		}
	}

	private void ToggleKeyword(Material m, bool enabled, string keyword)
	{
		if (enabled)
		{
			m.EnableKeyword(keyword);
		}
		else
		{
			m.DisableKeyword(keyword);
		}
	}
}