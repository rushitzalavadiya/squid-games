using System;
using TMPro;
using UnityEngine;

namespace TextFx
{
	public class TextFxTextMeshPro : TextMeshPro, TextFxAnimationInterface
	{
		[SerializeField]
		private TextFxAnimationManager m_animation_manager;

		[SerializeField]
		private GameObject m_gameobject_reference;

		[SerializeField]
		private bool m_renderToCurve;

		[SerializeField]
		private TextFxBezierCurve m_bezierCurve;

		[SerializeField]
		private Vector3[] m_currentVerts;

		[SerializeField]
		private Color32[] m_currentColours;

		private bool m_textFxUpdateGeometryCall;

		private bool m_textFxAnimDrawCall;

		private bool _forceNativeDrawCall;

		private string _strippedText;

		private string _rawMeshChars;

		private int _numRenderedLetters;

		private Vector3[] _vertsToUse;

		private Color32[] _colsToUse;

		public string AssetNameSuffix => "_TMP";

		public float MovementScale => 1f;

		public int LayerOverride => -1;

		public TEXTFX_IMPLEMENTATION TextFxImplementation => TEXTFX_IMPLEMENTATION.TMP;

		public TextAlignment TextAlignment
		{
			get
			{
				switch (base.alignment)
				{
				case TextAlignmentOptions.Top:
				case TextAlignmentOptions.Center:
				case TextAlignmentOptions.Bottom:
				case TextAlignmentOptions.Baseline:
				case TextAlignmentOptions.Midline:
					return TextAlignment.Center;
				case TextAlignmentOptions.TopRight:
				case TextAlignmentOptions.Right:
				case TextAlignmentOptions.BottomRight:
				case TextAlignmentOptions.BaselineRight:
				case TextAlignmentOptions.MidlineRight:
					return TextAlignment.Right;
				default:
					return TextAlignment.Left;
				}
			}
		}

		public bool FlippedMeshVerts => true;

		public TextFxAnimationManager AnimationManager => m_animation_manager;

		public GameObject GameObject
		{
			get
			{
				if (m_gameobject_reference == null)
				{
					m_gameobject_reference = base.gameObject;
				}
				return m_gameobject_reference;
			}
		}

		public bool CurvePositioningEnabled => true;

		public bool RenderToCurve
		{
			get
			{
				return m_renderToCurve;
			}
			set
			{
				m_renderToCurve = value;
			}
		}

		public TextFxBezierCurve BezierCurve
		{
			get
			{
				return m_bezierCurve;
			}
			set
			{
				m_bezierCurve = value;
				m_animation_manager.CheckCurveData();
				ForceUpdateCachedVertData();
				UpdateTextFxMesh();
			}
		}

		public UnityEngine.Object ObjectInstance => this;

		public Action OnMeshUpdateCall
		{
			get;
			set;
		}

		public int NumMeshVerts
		{
			get
			{
				if (m_currentVerts == null)
				{
					return 0;
				}
				return m_currentVerts.Length;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (m_animation_manager == null)
			{
				m_animation_manager = new TextFxAnimationManager();
			}
			m_animation_manager.SetParentObjectReferences(base.gameObject, base.transform, this);
		}

		protected override void Awake()
		{
			base.Awake();
			if (m_animation_manager == null)
			{
				m_animation_manager = new TextFxAnimationManager();
			}
			m_animation_manager.SetParentObjectReferences(base.gameObject, base.transform, this);
			if (Application.isPlaying)
			{
				m_animation_manager.OnStart();
			}
		}

		private void Update()
		{
			if (Application.isPlaying && m_animation_manager.Playing)
			{
				m_animation_manager.UpdateAnimation();
				TextFxMarkAsChanged();
			}
		}

		public void ForceUpdateCachedVertData()
		{
			m_animation_manager.PopulateDefaultMeshData(forcePopulate: true);
		}

		public void UpdateTextFxMesh()
		{
			TextFxMarkAsChanged();
		}

		public new void SetText(string new_text)
		{
			base.text = new_text;
			m_textFxAnimDrawCall = false;
		}

		public void SetColour(Color colour)
		{
			color = colour;
			m_animation_manager.SetDataRebuildCallFrame();
		}

		public Vector3 GetMeshVert(int index)
		{
			return m_currentVerts[index];
		}

		public Color GetMeshColour(int index)
		{
			return m_currentColours[index];
		}

		private void TextFxMarkAsChanged()
		{
			m_textFxUpdateGeometryCall = true;
			if (m_textFxUpdateGeometryCall)
			{
				m_textFxAnimDrawCall = true;
			}
			else
			{
				m_textFxAnimDrawCall = false;
			}
			m_textFxUpdateGeometryCall = false;
			GenerateTextMesh();
		}

		protected override void GenerateTextMesh()
		{
			if (!m_textFxAnimDrawCall || _forceNativeDrawCall)
			{
				_forceNativeDrawCall = false;
				base.GenerateTextMesh();
				_strippedText = (base.richText ? TextFxHelperMethods.StripRichTextCode(base.text) : base.text);
				_rawMeshChars = _strippedText.Replace("\n", string.Empty);
				_numRenderedLetters = _rawMeshChars.Length - m_textInfo.spaceCount;
				m_currentVerts = new Vector3[_numRenderedLetters * 4];
				for (int i = 0; i < _numRenderedLetters * 4; i++)
				{
					m_currentVerts[i] = m_textInfo.meshInfo[0].vertices[i];
				}
				m_currentColours = new Color32[_numRenderedLetters * 4];
				for (int j = 0; j < _numRenderedLetters * 4; j++)
				{
					m_currentColours[j] = m_textInfo.meshInfo[0].colors32[j];
				}
				m_animation_manager.UpdateText(_strippedText, new TMPTextDataHandler(m_currentVerts, m_currentColours, _numRenderedLetters), white_space_meshes: false);
				if (m_animation_manager.CheckCurveData())
				{
					m_animation_manager.PopulateDefaultMeshData(forcePopulate: true);
					_vertsToUse = m_animation_manager.MeshVerts;
					if (_vertsToUse.Length == mesh.vertexCount)
					{
						m_currentVerts = _vertsToUse;
						mesh.vertices = _vertsToUse;
					}
					else if (mesh.vertexCount > _vertsToUse.Length)
					{
						if (m_currentVerts.Length != mesh.vertexCount)
						{
							m_currentVerts = new Vector3[mesh.vertexCount];
						}
						for (int k = 0; k < _vertsToUse.Length; k++)
						{
							m_currentVerts[k] = _vertsToUse[k];
						}
						mesh.vertices = m_currentVerts;
					}
				}
				if (Application.isPlaying || m_animation_manager.Playing)
				{
					m_textFxAnimDrawCall = true;
					m_animation_manager.UpdateMesh(use_timer: true, force_render: true);
					GenerateTextMesh();
					return;
				}
			}
			else
			{
				_vertsToUse = m_animation_manager.MeshVerts;
				if (_colsToUse == null || _colsToUse.Length != m_animation_manager.MeshColours.Length)
				{
					_colsToUse = new Color32[m_animation_manager.MeshColours.Length];
				}
				for (int l = 0; l < _colsToUse.Length; l++)
				{
					_colsToUse[l] = m_animation_manager.MeshColours[l];
				}
				if (_vertsToUse.Length == mesh.vertexCount)
				{
					mesh.vertices = _vertsToUse;
					mesh.colors32 = _colsToUse;
					m_currentVerts = _vertsToUse;
					m_currentColours = _colsToUse;
				}
				else if (mesh.vertexCount > _vertsToUse.Length)
				{
					if (m_currentVerts.Length != mesh.vertexCount)
					{
						m_currentVerts = new Vector3[mesh.vertexCount];
					}
					if (m_currentColours.Length != mesh.vertexCount)
					{
						m_currentColours = new Color32[mesh.vertexCount];
					}
					for (int m = 0; m < _vertsToUse.Length; m++)
					{
						m_currentVerts[m] = _vertsToUse[m];
						m_currentColours[m] = _colsToUse[m];
					}
					mesh.vertices = m_currentVerts;
					mesh.colors32 = m_currentColours;
				}
			}
			m_textFxAnimDrawCall = false;
			_forceNativeDrawCall = false;
			if (OnMeshUpdateCall != null)
			{
				OnMeshUpdateCall();
			}
		}
	}
}
