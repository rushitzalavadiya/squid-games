using UnityEngine;

public class Hand : MonoBehaviour
{
	public enum HandSide
	{
		Left,
		Right
	}

	[SerializeField]
	private HandSide m_side;

	public HandSide GetHandSide => m_side;
}
