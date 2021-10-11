using UnityEngine;

public class AutoRotate : MonoBehaviour
{
	public float rotateSpeed;

	public float startingT;

	public bool isForward;

	private void Start()
	{
		base.transform.Rotate(isForward ? Vector3.back : Vector3.up, rotateSpeed * startingT);
	}

	private void Update()
	{
		base.transform.Rotate(isForward ? Vector3.back : Vector3.up, rotateSpeed * Time.deltaTime);
	}
}
