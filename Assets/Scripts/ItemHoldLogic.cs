using System.Collections.Generic;
using UnityEngine;

public class ItemHoldLogic : MonoBehaviour, IInitializable
{
	[SerializeField]
	private Croucher m_croucher;

	[SerializeField]
	private CharacterWeaponAnimator m_aimScript;

	[SerializeField]
	private CharacterItemAnimator m_itemScriptL;

	[SerializeField]
	private CharacterItemAnimator m_itemScriptR;

	[SerializeField]
	private Transform m_handBoneL;

	[SerializeField]
	private Transform m_handBoneR;

	private AnimatorOverrideController m_animator;

	public ItemLogic m_itemInHandL;

	public ItemLogic m_itemInHandR;

	private bool m_itemUsesBothHands;

	private bool m_isHoldingWeapon;

	private List<KeyValuePair<AnimationClip, AnimationClip>> m_overrides;

	public CharacterWeaponAnimator AimScript
	{
		set
		{
			m_aimScript = value;
		}
	}

	public Transform HandBoneR
	{
		get
		{
			return m_handBoneR;
		}
		set
		{
			m_handBoneR = value;
		}
	}

	public Transform HandBoneL
	{
		get
		{
			return m_handBoneL;
		}
		set
		{
			m_handBoneL = value;
		}
	}

	public void Initialize(GameObject character)
	{
		if (m_croucher == null)
		{
			m_croucher = character.GetComponent<Croucher>();
		}
		if (m_aimScript == null)
		{
			m_aimScript = character.GetComponent<CharacterWeaponAnimator>();
		}
		if (m_itemScriptR == null || m_itemScriptL == null)
		{
			CharacterItemAnimator[] components = character.GetComponents<CharacterItemAnimator>();
			foreach (CharacterItemAnimator characterItemAnimator in components)
			{
				if (characterItemAnimator.ThisHand == CharacterItemAnimator.Hand.Right)
				{
					m_itemScriptR = characterItemAnimator;
				}
				else if (characterItemAnimator.ThisHand == CharacterItemAnimator.Hand.Left)
				{
					m_itemScriptL = characterItemAnimator;
				}
			}
		}
		Hand[] componentsInChildren = character.GetComponentsInChildren<Hand>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			switch (componentsInChildren[j].GetHandSide)
			{
			case Hand.HandSide.Left:
				m_handBoneL = componentsInChildren[j].transform;
				break;
			case Hand.HandSide.Right:
				m_handBoneR = componentsInChildren[j].transform;
				break;
			}
		}
		Animator component = GetComponent<Animator>();
		m_animator = new AnimatorOverrideController(component.runtimeAnimatorController);
		m_animator.name = component.runtimeAnimatorController.name;
		component.runtimeAnimatorController = m_animator;
	}

	private void Awake()
	{
		Initialize(base.gameObject);
	}

	private void Start()
	{
		if (!m_handBoneR || !m_handBoneL)
		{
			UnityEngine.Debug.LogError("Handbones not set. Can't hold items.");
			return;
		}
		if (m_itemInHandR == null && m_itemInHandL == null && (bool)m_aimScript)
		{
			m_aimScript.SetGunInHand(value: false, -1);
		}
		CheckHands();
		if ((bool)m_itemInHandR)
		{
			AttachItem(m_itemInHandR, CharacterItemAnimator.Hand.Right);
		}
		if ((bool)m_itemInHandL)
		{
			AttachItem(m_itemInHandL, CharacterItemAnimator.Hand.Left);
		}
	}

	private void CheckHands()
	{
		ItemLogic itemInHandR = m_itemInHandR;
		ItemLogic itemLogic = m_itemInHandL;
		ItemLogic itemLogic2 = null;
		if ((bool)itemInHandR)
		{
			if (itemInHandR.m_useBothHands && (bool)m_itemInHandL)
			{
				Drop(CharacterItemAnimator.Hand.Left);
				itemLogic = null;
			}
			if (itemInHandR.m_PreferredHand == ItemLogic.PreferredHand.Right)
			{
				m_itemInHandR = itemInHandR;
			}
			else if (itemInHandR.m_PreferredHand == ItemLogic.PreferredHand.Left)
			{
				m_itemInHandL = itemInHandR;
				m_itemInHandR = null;
			}
			else if (itemInHandR.m_PreferredHand == ItemLogic.PreferredHand.Either)
			{
				m_itemInHandR = itemInHandR;
				itemLogic2 = itemInHandR;
			}
		}
		if (!itemLogic)
		{
			return;
		}
		if (itemLogic.m_useBothHands)
		{
			if ((bool)m_itemInHandR)
			{
				Drop(CharacterItemAnimator.Hand.Right);
			}
			if ((bool)m_itemInHandL)
			{
				Drop(CharacterItemAnimator.Hand.Left);
			}
			itemLogic2 = null;
		}
		if (itemLogic.m_PreferredHand == ItemLogic.PreferredHand.Left)
		{
			m_itemInHandL = itemLogic;
		}
		else if (itemLogic.m_PreferredHand == ItemLogic.PreferredHand.Right)
		{
			if (m_itemInHandR == null)
			{
				m_itemInHandR = itemLogic;
			}
			else
			{
				Drop(CharacterItemAnimator.Hand.Left);
			}
			if (itemLogic2 != null)
			{
				m_itemInHandR = itemLogic;
				m_itemInHandL = itemLogic2;
			}
		}
		else if (itemLogic.m_PreferredHand == ItemLogic.PreferredHand.Either)
		{
			if (m_itemInHandL != null && m_itemInHandL != itemLogic)
			{
				m_itemInHandR = itemLogic;
			}
			else
			{
				m_itemInHandL = itemLogic;
			}
		}
	}

	public void AttachItem(ItemLogic item, CharacterItemAnimator.Hand handToAttach)
	{
		if (item.m_PreferredHand == ItemLogic.PreferredHand.Left)
		{
			handToAttach = CharacterItemAnimator.Hand.Left;
		}
		else if (item.m_PreferredHand == ItemLogic.PreferredHand.Right)
		{
			handToAttach = CharacterItemAnimator.Hand.Right;
		}
		if (m_itemUsesBothHands || item.m_useBothHands)
		{
			if ((bool)m_itemInHandL && m_itemInHandL != item)
			{
				Drop(CharacterItemAnimator.Hand.Left);
			}
			if ((bool)m_itemInHandR && m_itemInHandR != item)
			{
				Drop(CharacterItemAnimator.Hand.Right);
			}
		}
		if (item == m_itemInHandL && handToAttach == CharacterItemAnimator.Hand.Left)
		{
			m_itemInHandL = item;
		}
		else if (item == m_itemInHandR && handToAttach == CharacterItemAnimator.Hand.Right)
		{
			m_itemInHandR = item;
		}
		else if (m_itemInHandL == null && handToAttach == CharacterItemAnimator.Hand.Left)
		{
			m_itemInHandL = item;
		}
		else if (m_itemInHandR == null && handToAttach == CharacterItemAnimator.Hand.Right)
		{
			m_itemInHandR = item;
		}
		else if (item.m_PreferredHand == ItemLogic.PreferredHand.Right)
		{
			if ((bool)m_itemInHandR)
			{
				Drop(CharacterItemAnimator.Hand.Right);
			}
			m_itemInHandR = item;
		}
		else if (item.m_PreferredHand == ItemLogic.PreferredHand.Left || item.m_PreferredHand == ItemLogic.PreferredHand.Either)
		{
			if ((bool)m_itemInHandL)
			{
				Drop(CharacterItemAnimator.Hand.Left);
			}
			m_itemInHandL = item;
		}
		CheckHands();
		Transform dummyPoint = null;
		if (item.DummyPoint != null)
		{
			dummyPoint = item.DummyPoint;
		}
		if ((bool)item.ItemAnimations)
		{
			bool isWeapon = item.ItemAnimations.IsWeapon;
		}
		m_itemUsesBothHands = item.m_useBothHands;
		if (m_itemScriptR != null && item == m_itemInHandR)
		{
			m_itemScriptR.UseBothHands = m_itemUsesBothHands;
		}
		else if (m_itemScriptL != null && item == m_itemInHandL)
		{
			m_itemScriptL.UseBothHands = m_itemUsesBothHands;
		}
		if (item == m_itemInHandR)
		{
			Attach(item, m_handBoneR, dummyPoint);
		}
		else if (item == m_itemInHandL)
		{
			Attach(item, m_handBoneL, dummyPoint);
		}
		if ((bool)m_aimScript)
		{
			if (item.ItemTypeID <= 2)
			{
				m_aimScript.SetGunInHand(value: true, item.ItemTypeID);
			}
			else
			{
				m_aimScript.SetGunInHand(value: false, -1);
			}
		}
		if ((bool)m_itemScriptL || (bool)m_itemScriptR)
		{
			SetCorrectAnimations();
		}
	}

	private void Attach(ItemLogic item, Transform hand, Transform dummyPoint)
	{
		item.transform.parent = hand;
		item.transform.localScale = new Vector3(Mathf.Abs(item.transform.localScale.x), Mathf.Abs(item.transform.localScale.y), Mathf.Abs(item.transform.localScale.z));
		item.transform.localPosition = Vector3.zero;
		item.transform.localRotation = Quaternion.identity;
		if (m_itemScriptL != null && hand == HandBoneL && (bool)item.ItemAnimations)
		{
			m_isHoldingWeapon = item.ItemAnimations.IsWeapon;
		}
		if (m_itemScriptR != null && hand == HandBoneR && (bool)item.ItemAnimations && !m_isHoldingWeapon)
		{
			m_isHoldingWeapon = item.ItemAnimations.IsWeapon;
		}
		if (m_itemScriptL != null && hand == HandBoneL)
		{
			m_itemScriptL.SetHolding(value: true, m_isHoldingWeapon);
		}
		if (m_itemScriptR != null && hand == HandBoneR)
		{
			m_itemScriptR.SetHolding(value: true, m_isHoldingWeapon);
		}
		if (dummyPoint != null)
		{
			item.transform.position = dummyPoint.position;
			item.transform.localRotation *= dummyPoint.localRotation;
		}
		if (hand == m_handBoneR)
		{
			m_itemInHandR = item;
		}
		else if (hand == m_handBoneL)
		{
			m_itemInHandL = item;
		}
		item.OnPickup();
	}

	public void Drop(CharacterItemAnimator.Hand hand)
	{
		switch (hand)
		{
		case CharacterItemAnimator.Hand.Right:
		{
			ItemLogic component2 = m_itemInHandR.GetComponent<ItemLogic>();
			m_itemInHandR.transform.parent = null;
			m_itemInHandR = null;
			if ((bool)component2)
			{
				component2.OnDrop();
			}
			break;
		}
		case CharacterItemAnimator.Hand.Left:
		{
			ItemLogic component = m_itemInHandL.GetComponent<ItemLogic>();
			m_itemInHandL.transform.parent = null;
			m_itemInHandL = null;
			if ((bool)component)
			{
				component.OnDrop();
			}
			break;
		}
		}
	}

	public void Toggle(CharacterItemAnimator.Hand hand)
	{
		switch (hand)
		{
		case CharacterItemAnimator.Hand.Right:
		{
			if (m_itemInHandR.gameObject.activeSelf)
			{
				m_itemInHandR.gameObject.SetActive(value: false);
				break;
			}
			ItemLogic itemInHandR = m_itemInHandR;
			itemInHandR.gameObject.SetActive(value: true);
			m_itemInHandR = null;
			AttachItem(itemInHandR, hand);
			break;
		}
		case CharacterItemAnimator.Hand.Left:
		{
			if (m_itemInHandL.gameObject.activeSelf)
			{
				m_itemInHandL.gameObject.SetActive(value: false);
				break;
			}
			ItemLogic itemInHandL = m_itemInHandL;
			itemInHandL.gameObject.SetActive(value: true);
			m_itemInHandL = null;
			AttachItem(itemInHandL, hand);
			break;
		}
		}
	}

	private void SetCorrectAnimations()
	{
		List<KeyValuePair<AnimationClip, AnimationClip>> list = new List<KeyValuePair<AnimationClip, AnimationClip>>(m_animator.overridesCount);
		m_animator.GetOverrides(list);
		m_overrides = list;
		if ((bool)m_itemInHandR)
		{
			ItemLogic component = m_itemInHandR.GetComponent<ItemLogic>();
			if ((bool)component.ItemAnimations)
			{
				if (component.ItemAnimations.InteractionRightLoop != null)
				{
					m_itemScriptR.LoopingAnimationStanding = true;
					if (component.ItemAnimations.InteractionRightStart != null)
					{
						m_itemScriptR.InteractionStandingStartDuration = component.ItemAnimations.InteractionRightStart.length;
						m_itemScriptR.LoopingDuration = component.ItemAnimations.InteractionRightLoopTime;
					}
				}
				else
				{
					m_itemScriptR.LoopingAnimationStanding = false;
				}
				if (component.ItemAnimations.CrouchingInteractionRightLoop != null)
				{
					m_itemScriptR.LoopingAnimationCrouching = true;
					if (component.ItemAnimations.CrouchingInteractionRightStart != null)
					{
						m_itemScriptR.InteractionCrouchingStartDuration = component.ItemAnimations.CrouchingInteractionRightStart.length;
						m_itemScriptR.LoopingDuration = component.ItemAnimations.CrouchingInteractionRightLoopTime;
					}
				}
				else
				{
					m_itemScriptR.LoopingAnimationCrouching = false;
				}
				ReplaceAnimation("_ItemInteractionRStart", component.ItemAnimations.InteractionRightStart);
				ReplaceAnimation("_ItemInteractionRLoop", component.ItemAnimations.InteractionRightLoop);
				ReplaceAnimation("_ItemInteractionREnd", component.ItemAnimations.InteractionRightEnd);
				ReplaceAnimation("_ItemInteractionCrouchingRStart", component.ItemAnimations.CrouchingInteractionRightStart);
				ReplaceAnimation("_ItemInteractionCrouchingRLoop", component.ItemAnimations.CrouchingInteractionRightLoop);
				ReplaceAnimation("_ItemInteractionCrouchingREnd", component.ItemAnimations.CrouchingInteractionRightEnd);
				ReplaceAnimation("_ItemHoldR", component.ItemAnimations.HoldingRight);
				ReplaceAnimation("_ItemHoldCrouchingR", component.ItemAnimations.CrouchingHoldingRight);
				ReplaceAnimation("_ItemEquipR", component.ItemAnimations.EquipRight);
				ReplaceAnimation("_ItemUnEquipR", component.ItemAnimations.UnEquipRight);
				if (component.m_useBothHands)
				{
					ReplaceAnimation("_ItemInteractionLStart", component.ItemAnimations.InteractionRightStart);
					ReplaceAnimation("_ItemInteractionLLoop", component.ItemAnimations.InteractionRightLoop);
					ReplaceAnimation("_ItemInteractionLEnd", component.ItemAnimations.InteractionRightEnd);
					ReplaceAnimation("_ItemInteractionCrouchingLStart", component.ItemAnimations.CrouchingInteractionRightStart);
					ReplaceAnimation("_ItemInteractionCrouchingLLoop", component.ItemAnimations.CrouchingInteractionRightLoop);
					ReplaceAnimation("_ItemInteractionCrouchingLEnd", component.ItemAnimations.CrouchingInteractionRightEnd);
					ReplaceAnimation("_ItemHoldB", component.ItemAnimations.HoldingRight);
					ReplaceAnimation("_ItemHoldCrouchingB", component.ItemAnimations.CrouchingHoldingRight);
					ReplaceAnimation("_ItemEquipB", component.ItemAnimations.EquipRight);
					ReplaceAnimation("_ItemUnEquipB", component.ItemAnimations.UnEquipRight);
				}
			}
			else
			{
				ReplaceAnimation("_ItemInteractionRStart", null);
				ReplaceAnimation("_ItemInteractionRLoop", null);
				ReplaceAnimation("_ItemInteractionREnd", null);
				ReplaceAnimation("_ItemInteractionCrouchingRStart", null);
				ReplaceAnimation("_ItemInteractionCrouchingRLoop", null);
				ReplaceAnimation("_ItemInteractionCrouchingREnd", null);
				ReplaceAnimation("_ItemHoldR", null);
				ReplaceAnimation("_ItemHoldCrouchingR", null);
				ReplaceAnimation("_ItemEquipR", null);
				ReplaceAnimation("_ItemUnEquipR", null);
			}
		}
		if ((bool)m_itemInHandL)
		{
			ItemLogic component2 = m_itemInHandL.GetComponent<ItemLogic>();
			if ((bool)component2.ItemAnimations)
			{
				if (component2.ItemAnimations.InteractionLeftLoop != null)
				{
					m_itemScriptL.LoopingAnimationStanding = true;
					if (component2.ItemAnimations.InteractionLeftStart != null)
					{
						m_itemScriptL.InteractionStandingStartDuration = component2.ItemAnimations.InteractionLeftStart.length;
						m_itemScriptL.LoopingDuration = component2.ItemAnimations.InteractionLeftLoopTime;
					}
				}
				else
				{
					m_itemScriptL.LoopingAnimationStanding = false;
				}
				if (component2.ItemAnimations.CrouchingInteractionLeftLoop != null)
				{
					m_itemScriptL.LoopingAnimationCrouching = true;
					if (component2.ItemAnimations.CrouchingInteractionLeftStart != null)
					{
						m_itemScriptL.InteractionCrouchingStartDuration = component2.ItemAnimations.CrouchingInteractionLeftStart.length;
						m_itemScriptL.LoopingDuration = component2.ItemAnimations.CrouchingInteractionLeftLoopTime;
					}
				}
				else
				{
					m_itemScriptL.LoopingAnimationCrouching = false;
				}
				ReplaceAnimation("_ItemInteractionLStart", component2.ItemAnimations.InteractionLeftStart);
				ReplaceAnimation("_ItemInteractionLLoop", component2.ItemAnimations.InteractionLeftLoop);
				ReplaceAnimation("_ItemInteractionLEnd", component2.ItemAnimations.InteractionLeftEnd);
				ReplaceAnimation("_ItemInteractionCrouchingLStart", component2.ItemAnimations.CrouchingInteractionLeftStart);
				ReplaceAnimation("_ItemInteractionCrouchingLLoop", component2.ItemAnimations.CrouchingInteractionLeftLoop);
				ReplaceAnimation("_ItemInteractionCrouchingLEnd", component2.ItemAnimations.CrouchingInteractionLeftEnd);
				ReplaceAnimation("_ItemHoldL", component2.ItemAnimations.HoldingLeft);
				ReplaceAnimation("_ItemHoldCrouchingL", component2.ItemAnimations.CrouchingHoldingLeft);
				ReplaceAnimation("_ItemEquipL", component2.ItemAnimations.EquipLeft);
				ReplaceAnimation("_ItemUnEquipL", component2.ItemAnimations.UnEquipLeft);
				if (component2.m_useBothHands)
				{
					ReplaceAnimation("_ItemInteractionRStart", component2.ItemAnimations.InteractionLeftStart);
					ReplaceAnimation("_ItemInteractionRLoop", component2.ItemAnimations.InteractionLeftLoop);
					ReplaceAnimation("_ItemInteractionREnd", component2.ItemAnimations.InteractionLeftEnd);
					ReplaceAnimation("_ItemInteractionCrouchingRStart", component2.ItemAnimations.CrouchingInteractionLeftStart);
					ReplaceAnimation("_ItemInteractionCrouchingRLoop", component2.ItemAnimations.CrouchingInteractionLeftLoop);
					ReplaceAnimation("_ItemInteractionCrouchingREnd", component2.ItemAnimations.CrouchingInteractionLeftEnd);
					ReplaceAnimation("_ItemHoldB", component2.ItemAnimations.HoldingLeft);
					ReplaceAnimation("_ItemHoldCrouchingB", component2.ItemAnimations.CrouchingHoldingLeft);
					ReplaceAnimation("_ItemEquipB", component2.ItemAnimations.EquipLeft);
					ReplaceAnimation("_ItemUnEquipB", component2.ItemAnimations.UnEquipLeft);
				}
			}
			else
			{
				ReplaceAnimation("_ItemInteractionLStart", null);
				ReplaceAnimation("_ItemInteractionLLoop", null);
				ReplaceAnimation("_ItemInteractionLEnd", null);
				ReplaceAnimation("_ItemInteractionCrouchingLStart", null);
				ReplaceAnimation("_ItemInteractionCrouchingLLoop", null);
				ReplaceAnimation("_ItemInteractionCrouchingLEnd", null);
				ReplaceAnimation("_ItemHoldL", null);
				ReplaceAnimation("_ItemHoldCrouchingL", null);
				ReplaceAnimation("_ItemEquipR", null);
				ReplaceAnimation("_ItemUnEquipR", null);
			}
		}
		List<KeyValuePair<AnimationClip, AnimationClip>> list2 = new List<KeyValuePair<AnimationClip, AnimationClip>>();
		for (int i = 0; i < m_overrides.Count; i++)
		{
			if (m_overrides[i].Value == list[i].Value)
			{
				list2.Add(m_overrides[i]);
			}
		}
		m_animator.ApplyOverrides(list2);
	}

	private void ReplaceAnimation(string oldAnimation, AnimationClip newAnimation)
	{
		for (int i = 0; i < m_overrides.Count; i++)
		{
			if (m_overrides[i].Key.name == oldAnimation)
			{
				m_overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(m_overrides[i].Key, newAnimation);
				return;
			}
		}
		UnityEngine.Debug.LogWarningFormat("Didn't find {0} in the animator", oldAnimation);
	}
}
