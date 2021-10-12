using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LanguageScript : MonoBehaviour
{
	public static List<string> interface_en;

	public static List<string> common_language;

	public TextAsset fr_xml;

	public TextAsset chS_xml;

	public TextAsset chT_xml;

	public TextAsset german_xml;

	public TextAsset italian_xml;

	public TextAsset portuguese_xml;

	public TextAsset japanese_xml;

	public TextAsset spanish_xml;

	public TextAsset russian_xml;

	private static bool exist = false;

	private static string language = "";

	private void Awake()
	{
		if (!exist)
		{
			exist = true;
			Object.DontDestroyOnLoad(base.gameObject);
			language = Application.systemLanguage.ToString();
			if (language == "French")
			{
				Define_Others();
			}
			else
			{
				Define_english();
			}
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	private void Start()
	{
	}

	public void Define_english()
	{
		interface_en = new List<string>();
		interface_en.Add("Start");
		interface_en.Add("Privacy Policy");
		interface_en.Add("Player");
		interface_en.Add("Level");
		interface_en.Add("Hold to run!");
		interface_en.Add("Hold to stop running!");
		interface_en.Add("Restart");
		interface_en.Add("Retry");
		interface_en.Add("Next");
	    interface_en.Add("Victory");
		interface_en.Add("WIN!");
		interface_en.Add("OVER!");
		interface_en.Add("You need to be at least #");
		interface_en.Add("Green light!");
		interface_en.Add("Red light!");
		interface_en.Add("You move!");
		interface_en.Add("Restore Purchases");
		interface_en.Add("Purchases have been restored!");
		interface_en.Add("Ads have been removed!");
		interface_en.Add("Purchases couldn't be restored.");
		interface_en.Add("A problem has occured whern trying to buy the product.");
		interface_en.Add("Skip level?");
		common_language = interface_en;
	}

	public void Define_Others()
	{
		XmlDocument xmlDocument = new XmlDocument();
		if (language == "French")
		{
			xmlDocument.LoadXml(fr_xml.text);
		}
		else if (language == "ChineseSimplified")
		{
			xmlDocument.LoadXml(chS_xml.text);
		}
		else if (language == "ChineseTraditional")
		{
			xmlDocument.LoadXml(chT_xml.text);
		}
		else if (language == "German")
		{
			xmlDocument.LoadXml(german_xml.text);
		}
		else if (language == "Italian")
		{
			xmlDocument.LoadXml(italian_xml.text);
		}
		else if (language == "Japanese")
		{
			xmlDocument.LoadXml(japanese_xml.text);
		}
		else if (language == "Russian")
		{
			xmlDocument.LoadXml(russian_xml.text);
		}
		else if (language == "Spanish")
		{
			xmlDocument.LoadXml(spanish_xml.text);
		}
		else
		{
			if (!(language == "Portuguese"))
			{
				Define_english();
				return;
			}
			xmlDocument.LoadXml(portuguese_xml.text);
		}
		XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("string");
		common_language = new List<string>();
		for (int i = 0; elementsByTagName[i] != null; i++)
		{
			common_language.Add(elementsByTagName[i].InnerText);
		}
	}

	public static string get_string(int reference)
	{
		if (reference < common_language.Count)
		{
			return common_language[reference];
		}
		return "...";
	}

	public static string GetLowerString(int reference)
	{
		if (reference < common_language.Count)
		{
			return common_language[reference];
		}
		return "...";
	}
}
