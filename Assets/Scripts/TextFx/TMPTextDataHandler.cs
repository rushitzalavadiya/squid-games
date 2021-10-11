using UnityEngine;

namespace TextFx
{
	public class TMPTextDataHandler : TextFxAnimationManager.GuiTextDataHandler
	{
		private Vector3[] m_posData;

		private Color32[] m_colData;

		private int m_numBaseLetters;

		private int m_extraVertsPerLetter;

		private int m_totalVertsPerLetter;

		private int m_numExtraQuads;

		private static int[] baseColVertIndexes = new int[4]
		{
			2,
			3,
			0,
			1
		};

		public int NumVerts => m_posData.Length;

		public int ExtraVertsPerLetter => m_extraVertsPerLetter;

		public int NumVertsPerLetter => m_totalVertsPerLetter;

		public TMPTextDataHandler(Vector3[] meshVerts, Color32[] meshCols, int numBaseLetterMeshes)
		{
			m_posData = meshVerts;
			m_colData = meshCols;
			m_numBaseLetters = numBaseLetterMeshes;
			m_totalVertsPerLetter = 4;
		}

		public Vector3[] GetLetterBaseVerts(int letterIndex)
		{
			Vector3[] array = new Vector3[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = m_posData[letterIndex * 4 + i];
			}
			return array;
		}

		public Color[] GetLetterBaseCols(int letterIndex)
		{
			Color[] array = new Color[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = m_colData[letterIndex * 4 + baseColVertIndexes[i]];
			}
			return array;
		}

		public Vector2[] GetLetterBaseUVs(int letterIndex)
		{
			return null;
		}

		public Vector3[] GetLetterExtraVerts(int letterIndex)
		{
			Vector3[] array = null;
			if (m_extraVertsPerLetter > 0)
			{
				array = new Vector3[m_extraVertsPerLetter];
				for (int i = 0; i < m_numExtraQuads; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						array[i * 4 + j] = m_posData[i * m_numBaseLetters * 4 + letterIndex * 4 + j];
					}
				}
			}
			return array;
		}

		public Color[] GetLetterExtraCols(int letterIndex)
		{
			Color[] array = null;
			if (m_extraVertsPerLetter > 0)
			{
				array = new Color[m_extraVertsPerLetter];
				for (int i = 0; i < m_numExtraQuads; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						array[i * 4 + j] = m_colData[i * m_numBaseLetters * 4 + letterIndex * 4 + j];
					}
				}
			}
			return array;
		}

		public Vector2[] GetLetterExtraUVs(int letterIndex)
		{
			return null;
		}
	}
}
