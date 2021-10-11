using UnityEngine;

public class ItemLogic : MonoBehaviour
{
	public enum PreferredHand
	{
		Right,
		Left,
		Either
	}

	public enum ItemType
	{
		AssaultRifle,
		SniperRifle,
		Pistol,
		Other
	}

	public bool m_useBothHands;

	public PreferredHand m_PreferredHand;

	[SerializeField]
	private ItemType m_itemTypeId = ItemType.Other;

	[SerializeField]
	private Transform m_dummyPoint;

	[SerializeField]
	private ItemAnimationsObject m_itemAnimations;

	public int ItemTypeID => (int)m_itemTypeId;

	public Transform DummyPoint => m_dummyPoint;

	public ItemAnimationsObject ItemAnimations => m_itemAnimations;

	public virtual void OnPickup()
	{
	}

	public virtual void OnDrop()
	{
	}
}
