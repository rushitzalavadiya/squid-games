using UnityEngine;
using UnityEngine.UI;

public class LanguageReferenceScript : MonoBehaviour
{
	public int reference;

	public bool lower;

	private Text label;

	private void Start()
	{
		label = GetComponent<Text>();
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
