using UnityEngine;

public class MainMenuPlayer : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		animator.SetBool("Grounded", value: true);
	}
}
