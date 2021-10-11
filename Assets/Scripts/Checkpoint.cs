using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	private GameManager gameManager;

	private List<Character> alreadyCheckedCharacters;

	private bool usingFlags = true;

	public List<Animator> flagAnimators;

	public List<GameObject> flags;

	public GameObject line;

	private void Awake()
	{
		alreadyCheckedCharacters = new List<Character>();
	}

	private void Start()
	{
		gameManager = GameManager.Instance;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
		{
			return;
		}
		Character component = other.GetComponent<Character>();
		if (alreadyCheckedCharacters.Contains(component))
		{
			return;
		}
		alreadyCheckedCharacters.Add(component);
		component.ChangeRespawn(base.transform.position.z);
		if (component.GetPlayerId() == 0)
		{
			gameManager.UpdateProgression();
			if (usingFlags)
			{
				flagAnimators[0].Play("LeftUp");
				flagAnimators[1].Play("RightUp");
			}
		}
	}

	public void DeactivateCheckpointElements()
	{
		flags[0].gameObject.SetActive(value: false);
		flags[1].gameObject.SetActive(value: false);
		line.SetActive(value: false);
		usingFlags = false;
	}
}
