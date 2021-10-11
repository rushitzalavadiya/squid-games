using System;
using UnityEngine;

public class TCP2_Demo : MonoBehaviour
{
	public Material[] AffectedMaterials;

	public Texture2D[] RampTextures;

	public GUISkin GuiSkin;

	public Light DirLight;

	public GameObject Robot;

	public GameObject Ethan;

	private bool mUnityShader;

	private bool mShaderSpecular = true;

	private bool mShaderBump = true;

	private bool mShaderReflection;

	private bool mShaderRim = true;

	private bool mShaderRimOutline;

	private bool mShaderOutline = true;

	private float mRimMin = 0.5f;

	private float mRimMax = 1f;

	private bool mRampTextureFlag;

	private Texture2D mRampTexture;

	private float mRampSmoothing = 0.15f;

	private float mLightRotationX = 80f;

	private float mLightRotationY = 25f;

	private bool mViewRobot;

	private bool mRobotOutlineNormals = true;

	private TCP2_Demo_View DemoView;

	private void Awake()
	{
		DemoView = GetComponent<TCP2_Demo_View>();
		mRampTexture = RampTextures[0];
		UpdateShader();
	}

	private void OnDestroy()
	{
		RestoreRimColors();
		UpdateShader();
	}

	private void OnGUI()
	{
		GUI.skin = GuiSkin;
		GUILayout.BeginArea(new Rect(new Rect(Screen.width - 310, 20f, 290f, 30f)));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Demo Character:");
		if (GUILayout.Button("Ethan", mViewRobot ? "Button" : "ButtonOn"))
		{
			mViewRobot = false;
			Robot.SetActive(value: false);
			Ethan.SetActive(value: true);
			DemoView.CharacterTransform = Ethan.transform;
		}
		if (GUILayout.Button("Robot Kyle", (!mViewRobot) ? "Button" : "ButtonOn"))
		{
			mViewRobot = true;
			Robot.SetActive(value: true);
			Ethan.SetActive(value: false);
			DemoView.CharacterTransform = Robot.transform;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(new Rect(Screen.width - 310, 55f, 290f, Screen.height - 40 - 90)));
		if (mViewRobot)
		{
			GUILayout.Label("Outline Normals");
			mRobotOutlineNormals = !GUILayout.Toggle(!mRobotOutlineNormals, "Regular Normals");
			mRobotOutlineNormals = GUILayout.Toggle(mRobotOutlineNormals, "TCP2's Encoded Smoothed Normals");
			GUILayout.Label("Toony Colors Pro 2 introduces an innovative way to fix broken outline caused by hard-edge shading.\nRead the documentation to learn more!", "SmallLabelShadow");
			GUI.Label(GUILayoutUtility.GetLastRect(), "Toony Colors Pro 2 introduces an innovative way to fix broken outline caused by hard-edge shading.\nRead the documentation to learn more!", "SmallLabel");
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(new Rect(Screen.width - 210, Screen.height - 60, 190f, 50f)));
		GUILayout.Label("Quality Settings:");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("<", GUILayout.Width(26f)))
		{
			QualitySettings.DecreaseLevel(applyExpensiveChanges: true);
		}
		GUILayout.Label(QualitySettings.names[QualitySettings.GetQualityLevel()], "LabelCenter");
		if (GUILayout.Button(">", GUILayout.Width(26f)))
		{
			QualitySettings.IncreaseLevel(applyExpensiveChanges: true);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(20f, 110f, Screen.width - 40, Screen.height - 40));
		mUnityShader = GUILayout.Toggle(mUnityShader, "View with Unity " + (mViewRobot ? "\"Diffuse Specular\"" : "\"Bumped Specular\""));
		GUILayout.Space(10f);
		GUI.enabled = !mUnityShader;
		GUILayout.Label("Toony Colors Pro 2 Settings");
		mShaderSpecular = GUILayout.Toggle(mShaderSpecular, "Specular");
		GUI.enabled = !mViewRobot;
		if (GUI.enabled)
		{
			mShaderBump = GUILayout.Toggle(mShaderBump, "Bump");
		}
		else
		{
			GUILayout.Toggle(false, "Bump");
		}
		GUI.enabled = !mUnityShader;
		mShaderReflection = GUILayout.Toggle(mShaderReflection, "Reflection");
		bool num = mShaderRim;
		mShaderRim = GUILayout.Toggle(mShaderRim, "Rim Lighting");
		bool num2 = num != mShaderRim;
		if (num2 && mShaderRim && mShaderRimOutline)
		{
			mShaderRimOutline = false;
		}
		if (num2 && mShaderRim)
		{
			RestoreRimColors();
		}
		bool num3 = mShaderRimOutline;
		mShaderRimOutline = GUILayout.Toggle(mShaderRimOutline, "Rim Outline");
		bool num4 = num3 != mShaderRimOutline;
		if (num4 && mShaderRimOutline && mShaderRim)
		{
			mShaderRim = false;
		}
		if (num4 && mShaderRimOutline)
		{
			RimOutlineColor();
		}
		GUI.enabled &= (mShaderRim || mShaderRimOutline);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Rim Min", GUILayout.Width(70f));
		mRimMin = GUILayout.HorizontalSlider(mRimMin, 0f, 1f, GUILayout.Width(130f));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Rim Max", GUILayout.Width(70f));
		mRimMax = GUILayout.HorizontalSlider(mRimMax, 0f, 1f, GUILayout.Width(130f));
		GUILayout.EndHorizontal();
		GUI.enabled = !mUnityShader;
		mShaderOutline = GUILayout.Toggle(mShaderOutline, "Outline");
		GUILayout.Space(6f);
		GUILayout.Label("Ramp Settings");
		mRampTextureFlag = GUILayout.Toggle(mRampTextureFlag, "Textured Ramp");
		GUI.enabled &= mRampTextureFlag;
		GUILayout.BeginHorizontal();
		Rect rect = GUILayoutUtility.GetRect(200f, 20f, GUILayout.ExpandWidth(expand: false));
		rect.y += 4f;
		GUI.DrawTexture(rect, mRampTexture);
		if (GUILayout.Button("<", GUILayout.Width(26f)))
		{
			PrevRamp();
		}
		if (GUILayout.Button(">", GUILayout.Width(26f)))
		{
			NextRamp();
		}
		GUILayout.EndHorizontal();
		GUI.enabled = !mUnityShader;
		GUI.enabled &= !mRampTextureFlag;
		GUILayout.BeginHorizontal();
		GUILayout.Label("Smoothing", GUILayout.Width(85f));
		mRampSmoothing = GUILayout.HorizontalSlider(mRampSmoothing, 0.01f, 1f, GUILayout.Width(115f));
		GUILayout.EndHorizontal();
		if (GUI.changed)
		{
			if (mUnityShader)
			{
				UnityDiffuseShader();
			}
			else
			{
				UpdateShader();
			}
		}
		GUI.enabled = true;
		GUILayout.Space(10f);
		GUILayout.Label("Light Rotation");
		mLightRotationX = GUILayout.HorizontalSlider(mLightRotationX, 0f, 360f, GUILayout.Width(200f));
		mLightRotationY = GUILayout.HorizontalSlider(mLightRotationY, 0f, 360f, GUILayout.Width(200f));
		GUILayout.Space(4f);
		GUILayout.Label("Hold Left mouse button to rotate character", "SmallLabelShadow");
		rect = GUILayoutUtility.GetLastRect();
		GUI.Label(rect, "Hold Left mouse button to rotate character", "SmallLabel");
		GUILayout.Label("Hold Right/Middle mouse button to scroll", "SmallLabelShadow");
		rect = GUILayoutUtility.GetLastRect();
		GUI.Label(rect, "Hold Right/Middle mouse button to scroll", "SmallLabel");
		GUILayout.Label("Use mouse scroll wheel or up/down keys to zoom", "SmallLabelShadow");
		rect = GUILayoutUtility.GetLastRect();
		GUI.Label(rect, "Use mouse scroll wheel or up/down keys to zoom", "SmallLabel");
		if (GUI.changed)
		{
			Vector3 eulerAngles = DirLight.transform.eulerAngles;
			eulerAngles.y = mLightRotationX;
			eulerAngles.x = mLightRotationY;
			DirLight.transform.eulerAngles = eulerAngles;
		}
		GUILayout.EndArea();
	}

	private void UnityDiffuseShader()
	{
		Shader shader = Shader.Find("Bumped Specular");
		Shader shader2 = Shader.Find("Specular");
		Material[] affectedMaterials = AffectedMaterials;
		foreach (Material material in affectedMaterials)
		{
			if (material.name.Contains("Robot"))
			{
				material.shader = shader2;
			}
			else
			{
				material.shader = shader;
			}
		}
	}

	private void UpdateShader()
	{
		Material[] affectedMaterials = AffectedMaterials;
		foreach (Material material in affectedMaterials)
		{
			ToggleKeyword(material, mShaderSpecular, "TCP2_SPEC");
			if (!material.name.Contains("Robot"))
			{
				ToggleKeyword(material, mShaderBump, "TCP2_BUMP");
			}
			ToggleKeyword(material, mShaderReflection, "TCP2_REFLECTION_MASKED");
			ToggleKeyword(material, mShaderRim, "TCP2_RIM");
			ToggleKeyword(material, mShaderRimOutline, "TCP2_RIMO");
			ToggleKeyword(material, mShaderOutline, "OUTLINES");
			ToggleKeyword(material, mRampTextureFlag, "TCP2_RAMPTEXT");
			material.SetFloat("_RampSmooth", mRampSmoothing);
			material.SetTexture("_Ramp", mRampTexture);
			material.SetFloat("_RimMin", mRimMin);
			material.SetFloat("_RimMax", mRimMax);
			if (material.name.Contains("Robot"))
			{
				ToggleKeyword(material, mRobotOutlineNormals, "TCP2_TANGENT_AS_NORMALS");
			}
		}
		affectedMaterials = AffectedMaterials;
		foreach (Material material2 in affectedMaterials)
		{
			Shader shaderWithKeywords = TCP2_RuntimeUtils.GetShaderWithKeywords(material2);
			if (shaderWithKeywords == null)
			{
				string text = "";
				string[] shaderKeywords = material2.shaderKeywords;
				foreach (string str in shaderKeywords)
				{
					text = text + str + ",";
				}
				text = text.TrimEnd(',');
				UnityEngine.Debug.LogError("[TCP2 Demo] Can't find shader for keywords: \"" + text + "\" in material \"" + material2.name + "\"\nThe missing shaders probably need to be unpacked. See TCP2 Documentation!");
			}
			else
			{
				material2.shader = shaderWithKeywords;
			}
		}
	}

	private void RimOutlineColor()
	{
		Material[] affectedMaterials = AffectedMaterials;
		for (int i = 0; i < affectedMaterials.Length; i++)
		{
			affectedMaterials[i].SetColor("_RimColor", Color.black);
		}
	}

	private void RestoreRimColors()
	{
		Material[] affectedMaterials = AffectedMaterials;
		foreach (Material material in affectedMaterials)
		{
			if (material.name.Contains("Robot"))
			{
				material.SetColor("_RimColor", new Color(0.2f, 0.6f, 1f, 0.5f));
			}
			else
			{
				material.SetColor("_RimColor", new Color(1f, 1f, 1f, 0.25f));
			}
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

	private void PrevRamp()
	{
		int value = Array.IndexOf(RampTextures, mRampTexture);
		value = Mathf.Clamp(value, 0, RampTextures.Length - 1);
		value--;
		if (value < 0)
		{
			value = RampTextures.Length - 1;
		}
		mRampTexture = RampTextures[value];
	}

	private void NextRamp()
	{
		int value = Array.IndexOf(RampTextures, mRampTexture);
		value = Mathf.Clamp(value, 0, RampTextures.Length - 1);
		value++;
		if (value >= RampTextures.Length)
		{
			value = 0;
		}
		mRampTexture = RampTextures[value];
	}
}
