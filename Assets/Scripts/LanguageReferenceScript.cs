using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageReferenceScript : MonoBehaviour
{
	public int reference;

	public bool lower;

	private TextMeshProUGUI label;

	private void Start()
	{
		label = GetComponent<TextMeshProUGUI>();
		if (!lower)
		{
			label.text = LanguageScript.get_string(reference);
		}
		else
		{
			label.text = LanguageScript.GetLowerString(reference);
		}
	}
}
