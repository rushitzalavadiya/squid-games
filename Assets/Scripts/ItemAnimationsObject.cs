using UnityEngine;

[CreateAssetMenu]
public class ItemAnimationsObject : ScriptableObject
{
	[Header("How the character will hold these items.")]
	[SerializeField]
	private AnimationClip m_holdingLeft;

	[SerializeField]
	private AnimationClip m_holdingRight;

	[Header("If Crouching poses are null, will use standing poses.")]
	[SerializeField]
	private AnimationClip m_crouchingHoldingLeft;

	[SerializeField]
	private AnimationClip m_crouchingHoldingRight;

	[Space(20f)]
	[Header("Interaction animations with these items.")]
	[Header("Only set start animation if you don't need the animation to loop.")]
	[Space(10f)]
	[Header("Standing")]
	[SerializeField]
	private AnimationClip m_interactionLeftStart;

	[SerializeField]
	private AnimationClip m_interactionLeftLoop;

	[SerializeField]
	private AnimationClip m_interactionLeftEnd;

	[SerializeField]
	private float m_interactionLeftLoopTime = 1f;

	[Space(10f)]
	[SerializeField]
	private AnimationClip m_interactionRightStart;

	[SerializeField]
	private AnimationClip m_interactionRightLoop;

	[SerializeField]
	private AnimationClip m_interactionRightEnd;

	[SerializeField]
	private float m_interactionRightLoopTime = 1f;

	[Space(20f)]
	[Header("Crouching")]
	[SerializeField]
	private bool m_useStandingInteractionAnimations;

	[SerializeField]
	private AnimationClip m_crouchingInteractionLeftStart;

	[SerializeField]
	private AnimationClip m_crouchingInteractionLeftLoop;

	[SerializeField]
	private AnimationClip m_crouchingInteractionLeftEnd;

	[SerializeField]
	private float m_crouchingInteractionLeftLoopTime = 1f;

	[Space(10f)]
	[SerializeField]
	private AnimationClip m_crouchingInteractionRightStart;

	[SerializeField]
	private AnimationClip m_crouchingInteractionRightLoop;

	[SerializeField]
	private AnimationClip m_crouchingInteractionRightEnd;

	[SerializeField]
	private float m_crouchingInteractionRightLoopTime = 1f;

	[Space(20f)]
	[Header("Equipment animations with these items.")]
	[SerializeField]
	private AnimationClip m_equipLeft;

	[SerializeField]
	private AnimationClip m_equipRight;

	[SerializeField]
	private AnimationClip m_unEquipLeft;

	[SerializeField]
	private AnimationClip m_unEquipRight;

	[Space(20f)]
	[Header("Does this item use the weapon movement animations?")]
	[SerializeField]
	private bool m_isWeapon;

	public AnimationClip HoldingLeft => m_holdingLeft;

	public AnimationClip HoldingRight => m_holdingRight;

	public AnimationClip CrouchingHoldingLeft
	{
		get
		{
			if (m_crouchingHoldingLeft != null)
			{
				return m_crouchingHoldingLeft;
			}
			return m_holdingLeft;
		}
	}

	public AnimationClip CrouchingHoldingRight
	{
		get
		{
			if (m_crouchingHoldingRight != null)
			{
				return m_crouchingHoldingRight;
			}
			return m_holdingRight;
		}
	}

	public AnimationClip InteractionLeftStart => m_interactionLeftStart;

	public AnimationClip InteractionLeftLoop => m_interactionLeftLoop;

	public AnimationClip InteractionLeftEnd => m_interactionLeftEnd;

	public float InteractionLeftLoopTime => m_interactionLeftLoopTime;

	public AnimationClip InteractionRightStart => m_interactionRightStart;

	public AnimationClip InteractionRightLoop => m_interactionRightLoop;

	public AnimationClip InteractionRightEnd => m_interactionRightEnd;

	public float InteractionRightLoopTime => m_interactionRightLoopTime;

	public AnimationClip CrouchingInteractionLeftStart
	{
		get
		{
			if (!m_useStandingInteractionAnimations)
			{
				return m_crouchingInteractionLeftStart;
			}
			return m_interactionLeftStart;
		}
	}

	public AnimationClip CrouchingInteractionLeftLoop
	{
		get
		{
			if (!m_useStandingInteractionAnimations)
			{
				return m_crouchingInteractionLeftLoop;
			}
			return m_interactionLeftLoop;
		}
	}

	public AnimationClip CrouchingInteractionLeftEnd
	{
		get
		{
			if (!m_useStandingInteractionAnimations)
			{
				return m_crouchingInteractionLeftEnd;
			}
			return m_interactionLeftEnd;
		}
	}

	public float CrouchingInteractionLeftLoopTime => m_crouchingInteractionLeftLoopTime;

	public AnimationClip CrouchingInteractionRightStart
	{
		get
		{
			if (!m_useStandingInteractionAnimations)
			{
				return m_crouchingInteractionRightStart;
			}
			return m_interactionRightStart;
		}
	}

	public AnimationClip CrouchingInteractionRightLoop
	{
		get
		{
			if (!m_useStandingInteractionAnimations)
			{
				return m_crouchingInteractionRightLoop;
			}
			return m_interactionRightLoop;
		}
	}

	public AnimationClip CrouchingInteractionRightEnd
	{
		get
		{
			if (!m_useStandingInteractionAnimations)
			{
				return m_crouchingInteractionRightEnd;
			}
			return m_interactionRightEnd;
		}
	}

	public float CrouchingInteractionRightLoopTime => m_crouchingInteractionRightLoopTime;

	public AnimationClip EquipLeft => m_equipLeft;

	public AnimationClip EquipRight => m_equipRight;

	public AnimationClip UnEquipLeft => m_unEquipLeft;

	public AnimationClip UnEquipRight => m_unEquipRight;

	public bool IsWeapon => m_isWeapon;
}
