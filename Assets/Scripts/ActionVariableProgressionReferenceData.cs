using System;
using System.Collections.Generic;

namespace TextFx
{
	[Serializable]
	public struct ActionVariableProgressionReferenceData
	{
		public ANIMATION_DATA_TYPE m_data_type;

		public bool m_start_state;

		public int m_action_index;

		public ActionVector3Progression GetVector3Prog(List<LetterAction> letter_actions)
		{
			if (m_action_index >= letter_actions.Count)
			{
				return null;
			}
			LetterAction letterAction = letter_actions[m_action_index];
			switch (m_data_type)
			{
			case ANIMATION_DATA_TYPE.LOCAL_SCALE:
				if (!m_start_state)
				{
					return letterAction.m_end_scale;
				}
				return letterAction.m_start_scale;
			case ANIMATION_DATA_TYPE.GLOBAL_SCALE:
				if (!m_start_state)
				{
					return letterAction.m_global_end_scale;
				}
				return letterAction.m_global_start_scale;
			case ANIMATION_DATA_TYPE.LOCAL_ROTATION:
				if (!m_start_state)
				{
					return letterAction.m_end_euler_rotation;
				}
				return letterAction.m_start_euler_rotation;
			case ANIMATION_DATA_TYPE.GLOBAL_ROTATION:
				if (!m_start_state)
				{
					return letterAction.m_global_end_euler_rotation;
				}
				return letterAction.m_global_start_euler_rotation;
			case ANIMATION_DATA_TYPE.POSITION:
				if (!m_start_state)
				{
					return letterAction.m_end_pos;
				}
				return letterAction.m_start_pos;
			default:
				return null;
			}
		}

		public ActionPositionVector3Progression GetPositionVector3Prog(List<LetterAction> letter_actions)
		{
			if (m_action_index >= letter_actions.Count)
			{
				return null;
			}
			LetterAction letterAction = letter_actions[m_action_index];
			ANIMATION_DATA_TYPE data_type = m_data_type;
			if (data_type == ANIMATION_DATA_TYPE.POSITION)
			{
				if (!m_start_state)
				{
					return letterAction.m_end_pos;
				}
				return letterAction.m_start_pos;
			}
			return null;
		}

		public ActionColorProgression GetColourProg(List<LetterAction> letter_actions, ActionColorProgression defaultColourProg)
		{
			if (m_action_index < 0 && m_data_type == ANIMATION_DATA_TYPE.COLOUR)
			{
				return defaultColourProg;
			}
			if (m_action_index >= letter_actions.Count)
			{
				return null;
			}
			LetterAction letterAction = letter_actions[m_action_index];
			ANIMATION_DATA_TYPE data_type = m_data_type;
			if (data_type == ANIMATION_DATA_TYPE.COLOUR)
			{
				if (!m_start_state)
				{
					return letterAction.m_end_colour;
				}
				return letterAction.m_start_colour;
			}
			return null;
		}
	}
}
