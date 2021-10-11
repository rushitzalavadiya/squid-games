using UnityEngine;
using UnityEngine.UI;

public class InformationText : MonoBehaviour
{
	private Animator ownAnimator;

	private Text text;

	private bool shoulDisplay;

	private void Awake()
	{
		ownAnimator = GetComponent<Animator>();
		text = GetComponent<Text>();
		shoulDisplay = true;
	}

	public void DisplayInformation(string informationText)
	{
		if (shoulDisplay)
		{
			text.text = informationText;
			ownAnimator.Play("ShowUp", 0, 0f);
		}
	}

	private void OnGameEnding(GameFinishedEvent e)
	{
		shoulDisplay = false;
	}
}
