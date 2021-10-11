using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace TextFx
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[ExecuteInEditMode]
	[AddComponentMenu("TextFx/TextFxNative")]
	public class TextFxNative : MonoBehaviour, TextFxAnimationInterface
	{
		public class TextFxTextDataHandler : TextFxAnimationManager.GuiTextDataHandler
		{
			private Vector3[] m_posData;

			private Color[] m_colData;

			public int NumVerts => m_posData.Length;

			public int NumVertsPerLetter => 4;

			public int ExtraVertsPerLetter => 0;

			public TextFxTextDataHandler(Vector3[] posData, Color[] colData)
			{
				m_posData = posData;
				m_colData = colData;
			}

			public Vector3[] GetLetterBaseVerts(int letterIndex)
			{
				return new Vector3[4]
				{
					m_posData[letterIndex * 4],
					m_posData[letterIndex * 4 + 1],
					m_posData[letterIndex * 4 + 2],
					m_posData[letterIndex * 4 + 3]
				};
			}

			public Color[] GetLetterBaseCols(int letterIndex)
			{
				return new Color[4]
				{
					m_colData[letterIndex * 4],
					m_colData[letterIndex * 4 + 1],
					m_colData[letterIndex * 4 + 2],
					m_colData[letterIndex * 4 + 3]
				};
			}

			public Vector3[] GetLetterExtraVerts(int letterIndex)
			{
				return null;
			}

			public Color[] GetLetterExtraCols(int letterIndex)
			{
				return null;
			}
		}

		private const float FONT_SCALE_FACTOR = 10f;

		private const float BASE_LINE_HEIGHT = 1.05f;

		public string m_text = "";

		public Font m_font;

		private int m_font_texture_width;

		private int m_font_texture_height;

		public TextAsset m_font_data_file;

		public Material m_font_material;

		public Vector2 m_px_offset = new Vector2(0f, 0f);

		public float m_character_size = 1f;

		public bool m_use_colour_gradient;

		public Color m_textColour = Color.white;

		public VertexColour m_textColourGradient = new VertexColour(Color.white);

		public TextDisplayAxis m_display_axis;

		public TextAnchor m_text_anchor = TextAnchor.MiddleCenter;

		public TextAlignment m_text_alignment;

		public float m_line_height_factor = 1f;

		public float m_max_width;

		public bool m_override_font_baseline;

		public float m_font_baseline_override;

		[SerializeField]
		private TextFxAnimationManager m_animation_manager;

		[SerializeField]
		private GameObject m_gameobject_reference;

		[SerializeField]
		private bool m_renderToCurve;

		[SerializeField]
		private TextFxBezierCurve m_bezierCurve;

		[SerializeField]
		private Vector3[] m_mesh_verts;

		[SerializeField]
		private Vector2[] m_mesh_uvs;

		[SerializeField]
		private Color[] m_mesh_cols;

		[SerializeField]
		private Vector3[] m_mesh_normals;

		[SerializeField]
		private int[] m_mesh_triangles;

		[SerializeField]
		private float m_font_baseline;

		[SerializeField]
		private CustomFontCharacterData m_custom_font_data;

		[SerializeField]
		private string m_current_font_data_file_name = "";

		[SerializeField]
		private string m_current_font_name = "";

		[SerializeField]
		private Action<Font> m_fontRebuildCallback;

		private CombineInstance[] m_mesh_combine_instance;

		private Transform m_transform_reference;

		private Renderer m_renderer;

		private MeshFilter m_mesh_filter;

		private Mesh m_mesh;

		private float m_total_text_width;

		private float m_total_text_height;

		private float m_line_height;

		public string AssetNameSuffix => "";

		public float MovementScale => 1f;

		public int LayerOverride => -1;

		public TEXTFX_IMPLEMENTATION TextFxImplementation => TEXTFX_IMPLEMENTATION.NATIVE;

		public TextAlignment TextAlignment => m_text_alignment;

		public bool FlippedMeshVerts => false;

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

		private float LineHeightFactor => m_line_height_factor * 1.05f;

		private float FontScale => 10f / m_character_size;

		public float FontBaseLine
		{
			get
			{
				if (!m_override_font_baseline)
				{
					return m_font_baseline / FontScale;
				}
				return m_font_baseline_override;
			}
		}

		public bool IsFontBaseLineSet
		{
			get
			{
				if (!m_override_font_baseline)
				{
					return m_font_baseline != 0f;
				}
				return true;
			}
		}

		public Vector3 Position
		{
			get
			{
				if (!(m_transform != null))
				{
					return base.transform.position;
				}
				return m_transform.position;
			}
		}

		public Vector3 Scale
		{
			get
			{
				if (!(m_transform != null))
				{
					return base.transform.localScale;
				}
				return m_transform.localScale;
			}
		}

		public Quaternion Rotation
		{
			get
			{
				if (!(m_transform != null))
				{
					return base.transform.rotation;
				}
				return m_transform.rotation;
			}
		}

		public string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				SetText(value);
			}
		}

		public float LineHeight => m_line_height;

		public Transform m_transform => m_transform_reference;

		public bool IsFontDataAssigned
		{
			get
			{
				if (m_font != null)
				{
					return true;
				}
				if (m_font_data_file != null && m_font_material != null)
				{
					return true;
				}
				return false;
			}
		}

		public int NumMeshVerts => m_mesh.vertexCount;

		public void ClearFontCharacterData()
		{
			if (m_custom_font_data != null)
			{
				m_custom_font_data.m_character_infos.Clear();
				m_custom_font_data = null;
			}
		}

		private void OnEnable()
		{
			if (m_animation_manager == null)
			{
				m_animation_manager = new TextFxAnimationManager();
			}
			m_animation_manager.SetParentObjectReferences(base.gameObject, base.transform, this);
			m_mesh_filter = base.gameObject.GetComponent<MeshFilter>();
			m_transform_reference = base.transform;
			if (m_mesh_filter.sharedMesh != null)
			{
				TextFxNative[] array = UnityEngine.Object.FindObjectsOfType(typeof(TextFxNative)) as TextFxNative[];
				for (int i = 0; i < array.Length; i++)
				{
					MeshFilter mesh_filter = array[i].m_mesh_filter;
					if (mesh_filter != null && mesh_filter.sharedMesh == m_mesh_filter.sharedMesh && mesh_filter != m_mesh_filter)
					{
						m_mesh_filter.mesh = new Mesh();
						m_mesh = m_mesh_filter.sharedMesh;
						SetText(m_text);
					}
				}
				m_mesh = m_mesh_filter.sharedMesh;
			}
			else
			{
				m_mesh = new Mesh();
				m_mesh_filter.mesh = m_mesh;
				if (IsFontDataAssigned)
				{
					SetText(m_text);
				}
			}
			if (m_font != null)
			{
				m_fontRebuildCallback = delegate(Font rebuiltFont)
				{
					FontImportDetected(rebuiltFont);
				};
				Font.textureRebuilt += m_fontRebuildCallback;
				m_font.RequestCharactersInTexture(m_text);
			}
		}

		private void FontImportDetected(Font changedFont)
		{
			if (!(m_font == null) && changedFont.Equals(m_font))
			{
				m_current_font_name = "";
				SetText(m_text);
			}
		}

		private void OnDisable()
		{
			if (m_font != null)
			{
				Font.textureRebuilt -= FontImportDetected;
			}
		}

		private void Start()
		{
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
				UpdateTextFxMesh();
			}
		}

		public void ForceUpdateCachedVertData()
		{
			m_animation_manager.PopulateDefaultMeshData(forcePopulate: true);
			Vector3[] meshVerts = m_animation_manager.MeshVerts;
			for (int i = 0; i < meshVerts.Length; i++)
			{
				m_mesh_verts[i] = meshVerts[i];
			}
		}

		public void UpdateTextFxMesh()
		{
			UpdateMesh(animationCall: true);
		}

		public Vector3 GetMeshVert(int index)
		{
			return m_mesh.vertices[index];
		}

		public Color GetMeshColour(int index)
		{
			return m_mesh.colors[index];
		}

		public void SetText(string new_text)
		{
			SetTextMesh(new_text);
		}

		public void SetColour(Color colour)
		{
			m_textColour = colour;
			SetText(m_text);
		}

		private string GetHumanReadableCharacterString(char character)
		{
			if (character.Equals('\n'))
			{
				return "[NEW LINE]";
			}
			if (character.Equals(' '))
			{
				return "[SPACE]";
			}
			if (character.Equals('\r'))
			{
				return "[CARRIAGE RETURN]";
			}
			if (character.Equals('\t'))
			{
				return "[TAB]";
			}
			return character.ToString() ?? "";
		}

		private bool GetCharacterInfo(char m_character, ref CustomCharacterInfo char_info)
		{
			if (m_character.Equals('\n') || m_character.Equals('\r'))
			{
				return true;
			}
			if (m_font != null)
			{
				if (!m_current_font_name.Equals(m_font.name))
				{
					Dictionary<float, int> dictionary = new Dictionary<float, int>();
					CharacterInfo[] characterInfo = m_font.characterInfo;
					foreach (CharacterInfo characterInfo2 in characterInfo)
					{
						if ((characterInfo2.index >= 97 && characterInfo2.index < 123) || (characterInfo2.index >= 65 && characterInfo2.index < 91))
						{
							Rect vert = characterInfo2.vert;
							float num = 0f - vert.y;
							vert = characterInfo2.vert;
							float num2 = num - vert.height;
							if (dictionary.ContainsKey(num2))
							{
								Dictionary<float, int> dictionary2 = dictionary;
								float key = num2;
								dictionary2[key]++;
							}
							else
							{
								dictionary[num2] = 1;
							}
						}
					}
					int num3 = 0;
					int num4 = 0;
					int num5 = -1;
					float font_baseline = -1f;
					foreach (int value in dictionary.Values)
					{
						if (num5 == -1 || value > num4)
						{
							num5 = num3;
							num4 = value;
						}
						num3++;
					}
					num3 = 0;
					foreach (float key2 in dictionary.Keys)
					{
						if (num3 == num5)
						{
							font_baseline = key2;
							break;
						}
						num3++;
					}
					m_font_baseline = font_baseline;
					m_current_font_name = m_font.name;
				}
				CharacterInfo info = default(CharacterInfo);
				m_font.GetCharacterInfo(m_character, out info);
				char_info.flipped = info.flipped;
				char_info.uv = info.uv;
				char_info.vert = info.vert;
				char_info.width = info.width;
				char_info.vert.x /= FontScale;
				char_info.vert.y /= FontScale;
				char_info.vert.width /= FontScale;
				char_info.vert.height /= FontScale;
				char_info.width /= FontScale;
				if (info.width == 0f)
				{
					UnityEngine.Debug.LogWarning("Character '" + GetHumanReadableCharacterString(m_character) + "' not found. Check that font '" + m_font.name + "' supports this character.");
				}
				return true;
			}
			if (m_font_data_file != null)
			{
				if (m_custom_font_data == null || !m_font_data_file.name.Equals(m_current_font_data_file_name))
				{
					if (m_font_data_file.text.Substring(0, 5).Equals("<?xml"))
					{
						m_current_font_data_file_name = m_font_data_file.name;
						m_custom_font_data = new CustomFontCharacterData();
						XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(m_font_data_file.text));
						int num6 = 0;
						int num7 = 0;
						while (xmlTextReader.Read())
						{
							if (xmlTextReader.IsStartElement())
							{
								if (xmlTextReader.Name.Equals("common"))
								{
									num6 = int.Parse(xmlTextReader.GetAttribute("scaleW"));
									num7 = int.Parse(xmlTextReader.GetAttribute("scaleH"));
									m_font_baseline = int.Parse(xmlTextReader.GetAttribute("base"));
								}
								else if (xmlTextReader.Name.Equals("char"))
								{
									int num8 = int.Parse(xmlTextReader.GetAttribute("x"));
									int num9 = int.Parse(xmlTextReader.GetAttribute("y"));
									float num10 = float.Parse(xmlTextReader.GetAttribute("width"));
									float num11 = float.Parse(xmlTextReader.GetAttribute("height"));
									float x = float.Parse(xmlTextReader.GetAttribute("xoffset"));
									float num12 = float.Parse(xmlTextReader.GetAttribute("yoffset"));
									float width = float.Parse(xmlTextReader.GetAttribute("xadvance"));
									CustomCharacterInfo customCharacterInfo = new CustomCharacterInfo();
									customCharacterInfo.flipped = false;
									customCharacterInfo.uv = new Rect((float)num8 / (float)num6, 1f - (float)num9 / (float)num7 - num11 / (float)num7, num10 / (float)num6, num11 / (float)num7);
									customCharacterInfo.vert = new Rect(x, 0f - num12, num10, 0f - num11);
									customCharacterInfo.width = width;
									m_custom_font_data.m_character_infos.Add(int.Parse(xmlTextReader.GetAttribute("id")), customCharacterInfo);
								}
							}
						}
					}
					else if (m_font_data_file.text.Substring(0, 4).Equals("info"))
					{
						m_current_font_data_file_name = m_font_data_file.name;
						m_custom_font_data = new CustomFontCharacterData();
						int num13 = 0;
						int num14 = 0;
						string[] array = m_font_data_file.text.Split('\n');
						foreach (string text in array)
						{
							if (text.Length >= 5 && text.Substring(0, 5).Equals("char "))
							{
								string[] array2 = ParseFieldData(text, new string[8]
								{
									"id=",
									"x=",
									"y=",
									"width=",
									"height=",
									"xoffset=",
									"yoffset=",
									"xadvance="
								});
								int num15 = int.Parse(array2[1]);
								int num16 = int.Parse(array2[2]);
								float num17 = float.Parse(array2[3]);
								float num18 = float.Parse(array2[4]);
								float x2 = float.Parse(array2[5]);
								float num19 = float.Parse(array2[6]);
								float width2 = float.Parse(array2[7]);
								CustomCharacterInfo customCharacterInfo2 = new CustomCharacterInfo();
								customCharacterInfo2.flipped = false;
								customCharacterInfo2.uv = new Rect((float)num15 / (float)num13, 1f - (float)num16 / (float)num14 - num18 / (float)num14, num17 / (float)num13, num18 / (float)num14);
								customCharacterInfo2.vert = new Rect(x2, 0f - num19 + 1f, num17, 0f - num18);
								customCharacterInfo2.width = width2;
								m_custom_font_data.m_character_infos.Add(int.Parse(array2[0]), customCharacterInfo2);
							}
							else if (text.Length >= 6 && text.Substring(0, 6).Equals("common"))
							{
								string[] array2 = ParseFieldData(text, new string[3]
								{
									"scaleW=",
									"scaleH=",
									"base="
								});
								num13 = int.Parse(array2[0]);
								num14 = int.Parse(array2[1]);
								m_font_baseline = int.Parse(array2[2]);
							}
						}
					}
				}
				if (m_custom_font_data.m_character_infos.ContainsKey(m_character))
				{
					m_custom_font_data.m_character_infos[m_character].ScaleClone(FontScale, ref char_info);
					return true;
				}
			}
			return false;
		}

		private string[] ParseFieldData(string data_string, string[] fields)
		{
			string[] array = new string[fields.Length];
			int num = 0;
			foreach (string text in fields)
			{
				int num2 = data_string.IndexOf(text) + text.Length;
				int num3 = data_string.IndexOf(" ", num2);
				array[num] = data_string.Substring(num2, num3 - num2);
				num++;
			}
			return array;
		}

		private void SetTextMesh(string new_text)
		{
			if (this == null || Equals(null))
			{
				return;
			}
			if (m_renderer == null)
			{
				m_renderer = GetComponent<Renderer>();
			}
			bool flag = false;
			if ((m_renderer.sharedMaterial == null || m_renderer.sharedMaterial != m_font_material) && m_font_material != null)
			{
				m_renderer.sharedMaterial = m_font_material;
			}
			else if (m_font != null)
			{
				if (m_renderer.sharedMaterial == null || m_renderer.sharedMaterial != m_font_material)
				{
					m_font_material = m_font.material;
					m_renderer.sharedMaterial = m_font_material;
				}
				if (m_renderer.sharedMaterial != null)
				{
					flag = true;
				}
			}
			if (!flag && (m_renderer.sharedMaterial == null || m_font_data_file == null))
			{
				m_font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
				m_font_material = m_font.material;
				m_renderer.sharedMaterial = m_font_material;
				m_font_data_file = null;
			}
			m_text = new_text;
			new_text = new_text.Replace("\r", "");
			string text = m_text.Replace(" ", "").Replace("\n", "").Replace("\r", "")
				.Replace("\t", "");
			int length = new_text.Length;
			int length2 = text.Length;
			m_mesh_verts = new Vector3[length2 * 4];
			m_mesh_uvs = new Vector2[length2 * 4];
			m_mesh_cols = new Color[length2 * 4];
			m_mesh_normals = new Vector3[length2 * 4];
			m_mesh_triangles = new int[length2 * 2 * 3];
			int[] array = new int[length2];
			List<float> line_widths = new List<float>();
			List<CustomCharacterInfo> list = new List<CustomCharacterInfo>();
			CustomCharacterInfo char_info = new CustomCharacterInfo();
			CustomCharacterInfo last_char_info = null;
			if (m_font != null)
			{
				m_font.RequestCharactersInTexture(m_text);
				if (m_font_material.mainTexture.width != m_font_texture_width || m_font_material.mainTexture.height != m_font_texture_height)
				{
					m_font_texture_width = m_font_material.mainTexture.width;
					m_font_texture_height = m_font_material.mainTexture.height;
					SetText(m_text);
					return;
				}
			}
			float y_max = 0f;
			float y_min = 0f;
			float x_max = 0f;
			float x_min = 0f;
			float text_width = 0f;
			float text_height = 0f;
			int line_letter_idx = 0;
			float line_height_offset = 0f;
			float total_text_width = 0f;
			float total_text_height = 0f;
			float line_width_at_last_space = 0f;
			float space_char_offset = 0f;
			int num = -1;
			float last_space_y_max = 0f;
			float last_space_y_min = 0f;
			float num2 = 0f;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			m_line_height = 0f;
			Action action = delegate
			{
				if (m_display_axis == TextDisplayAxis.HORIZONTAL)
				{
					float num11 = Mathf.Abs(y_max - y_min) * LineHeightFactor;
					if (num11 > m_line_height)
					{
						m_line_height = num11;
					}
					if (last_char_info != null)
					{
						text_width += 0f - last_char_info.width + last_char_info.vert.width + last_char_info.vert.x;
					}
					line_widths.Add(text_width);
					line_height_offset += num11;
					if (text_width > total_text_width)
					{
						total_text_width = text_width;
					}
					total_text_height += num11;
				}
				else
				{
					float num12 = Mathf.Abs(x_max - x_min) * LineHeightFactor;
					if (num12 > m_line_height)
					{
						m_line_height = num12;
					}
					line_widths.Add(num12);
					line_height_offset += num12;
					total_text_width += num12;
					if (text_height < total_text_height)
					{
						total_text_height = text_height;
					}
				}
				line_letter_idx = 0;
				text_width = 0f;
				line_width_at_last_space = 0f;
				space_char_offset = 0f;
				last_space_y_max = 0f;
				last_space_y_min = 0f;
				text_height = 0f;
				last_char_info = null;
			};
			for (int i = 0; i < length; i++)
			{
				char character = new_text[i];
				if (GetCharacterInfo(character, ref char_info))
				{
					if (character.Equals('\t'))
					{
						continue;
					}
					if (character.Equals(' '))
					{
						if (m_display_axis == TextDisplayAxis.HORIZONTAL)
						{
							line_width_at_last_space = text_width;
							space_char_offset = char_info.width;
							last_space_y_max = y_max;
							last_space_y_min = y_min;
							num = num3;
							list = new List<CustomCharacterInfo>();
							text_width += char_info.width;
						}
						else
						{
							char_info.vert.height = 0f - char_info.width;
						}
						num2 += ((m_display_axis == TextDisplayAxis.HORIZONTAL) ? char_info.width : (0f - char_info.width));
						num5++;
					}
					else if (character.Equals('\n'))
					{
						action();
						num = -1;
						num2 = 0f;
						num4++;
						num5++;
					}
					else
					{
						if (m_display_axis == TextDisplayAxis.HORIZONTAL)
						{
							if (num >= 0)
							{
								list.Add(new CustomCharacterInfo(char_info));
							}
							if (line_letter_idx == 0 || char_info.vert.y > y_max)
							{
								y_max = char_info.vert.y;
							}
							if (line_letter_idx == 0 || char_info.vert.y + char_info.vert.height < y_min)
							{
								y_min = char_info.vert.y + char_info.vert.height;
							}
							text_width += ((i == length - 1) ? (char_info.vert.width + char_info.vert.x) : char_info.width);
							if (m_max_width > 0f && num >= 0 && ((i == length - 1) ? text_width : (text_width - char_info.width + char_info.vert.width + char_info.vert.x)) > m_max_width)
							{
								float num6 = text_width - line_width_at_last_space - space_char_offset;
								float num7 = last_space_y_min;
								float num8 = last_space_y_max;
								text_width = line_width_at_last_space;
								y_max = last_space_y_max;
								y_min = last_space_y_min;
								num2 = 0f;
								num4++;
								action();
								text_width = num6;
								y_min = num7;
								y_max = num8;
								for (int j = num; j < num3; j++)
								{
									CustomCharacterInfo customCharacterInfo = list[j - num];
									array[j] = num4;
									Vector3 b = (m_display_axis == TextDisplayAxis.HORIZONTAL) ? new Vector3(num2 + customCharacterInfo.vert.x, 0f, 0f) : new Vector3(customCharacterInfo.vert.x, num2, 0f);
									m_mesh_verts[j * 4 + (customCharacterInfo.flipped ? 3 : 0)] = new Vector3(customCharacterInfo.vert.width, FontBaseLine + customCharacterInfo.vert.y, 0f) + b;
									m_mesh_verts[j * 4 + ((!customCharacterInfo.flipped) ? 1 : 0)] = new Vector3(0f, FontBaseLine + customCharacterInfo.vert.y, 0f) + b;
									m_mesh_verts[j * 4 + (customCharacterInfo.flipped ? 1 : 2)] = new Vector3(0f, customCharacterInfo.vert.height + FontBaseLine + customCharacterInfo.vert.y, 0f) + b;
									m_mesh_verts[j * 4 + (customCharacterInfo.flipped ? 2 : 3)] = new Vector3(customCharacterInfo.vert.width, customCharacterInfo.vert.height + FontBaseLine + customCharacterInfo.vert.y, 0f) + b;
									num2 += ((m_display_axis == TextDisplayAxis.HORIZONTAL) ? (customCharacterInfo.width + m_px_offset.x / FontScale) : (customCharacterInfo.vert.height + (0f - m_px_offset.y) / FontScale));
								}
								num = -1;
							}
						}
						else
						{
							if (line_letter_idx == 0 || char_info.vert.x + char_info.vert.width > x_max)
							{
								x_max = char_info.vert.x + char_info.vert.width;
							}
							if (line_letter_idx == 0 || char_info.vert.x < x_min)
							{
								x_min = char_info.vert.x;
							}
							text_height += char_info.vert.height;
						}
						Vector3 b2 = (m_display_axis == TextDisplayAxis.HORIZONTAL) ? new Vector3(num2 + char_info.vert.x, 0f, 0f) : new Vector3(char_info.vert.x, num2, 0f);
						m_mesh_verts[num3 * 4 + (char_info.flipped ? 3 : 0)] = new Vector3(char_info.vert.width, FontBaseLine + char_info.vert.y, 0f) + b2;
						m_mesh_verts[num3 * 4 + ((!char_info.flipped) ? 1 : 0)] = new Vector3(0f, FontBaseLine + char_info.vert.y, 0f) + b2;
						m_mesh_verts[num3 * 4 + (char_info.flipped ? 1 : 2)] = new Vector3(0f, char_info.vert.height + FontBaseLine + char_info.vert.y, 0f) + b2;
						m_mesh_verts[num3 * 4 + (char_info.flipped ? 2 : 3)] = new Vector3(char_info.vert.width, char_info.vert.height + FontBaseLine + char_info.vert.y, 0f) + b2;
						Rect uv = char_info.uv;
						m_mesh_uvs[num3 * 4 + (char_info.flipped ? 3 : 0)] = new Vector2(uv.x + uv.width, uv.y + uv.height);
						m_mesh_uvs[num3 * 4 + ((!char_info.flipped) ? 1 : 2)] = new Vector2(uv.x, uv.y + uv.height);
						m_mesh_uvs[num3 * 4 + (char_info.flipped ? 1 : 2)] = new Vector2(uv.x, uv.y);
						m_mesh_uvs[num3 * 4 + ((!char_info.flipped) ? 3 : 0)] = new Vector2(uv.x + uv.width, uv.y);
						m_mesh_normals[num3 * 4] = (m_mesh_normals[num3 * 4 + 1] = (m_mesh_normals[num3 * 4 + 2] = (m_mesh_normals[num3 * 4 + 3] = Vector3.back)));
						m_mesh_triangles[num3 * 6] = num3 * 4 + 2;
						m_mesh_triangles[num3 * 6 + 1] = num3 * 4 + 1;
						m_mesh_triangles[num3 * 6 + 2] = num3 * 4;
						m_mesh_triangles[num3 * 6 + 3] = num3 * 4 + 3;
						m_mesh_triangles[num3 * 6 + 4] = num3 * 4 + 2;
						m_mesh_triangles[num3 * 6 + 5] = num3 * 4;
						m_mesh_cols[num3 * 4 + (char_info.flipped ? 3 : 0)] = (m_use_colour_gradient ? m_textColourGradient.top_right : m_textColour);
						m_mesh_cols[num3 * 4 + ((!char_info.flipped) ? 1 : 0)] = (m_use_colour_gradient ? m_textColourGradient.top_left : m_textColour);
						m_mesh_cols[num3 * 4 + (char_info.flipped ? 1 : 2)] = (m_use_colour_gradient ? m_textColourGradient.bottom_left : m_textColour);
						m_mesh_cols[num3 * 4 + (char_info.flipped ? 2 : 3)] = (m_use_colour_gradient ? m_textColourGradient.bottom_right : m_textColour);
						array[num3] = num4;
						num3++;
						num2 += ((m_display_axis == TextDisplayAxis.HORIZONTAL) ? (char_info.width + m_px_offset.x / FontScale) : (char_info.vert.height + (0f - m_px_offset.y) / FontScale));
						last_char_info = char_info;
					}
				}
				line_letter_idx++;
			}
			if (m_display_axis == TextDisplayAxis.HORIZONTAL)
			{
				float num9 = Mathf.Abs(y_max - y_min);
				line_widths.Add(text_width);
				if (text_width > total_text_width)
				{
					total_text_width = text_width;
				}
				total_text_height += num9;
				if (m_line_height == 0f)
				{
					m_line_height = total_text_height;
				}
			}
			else
			{
				float num10 = Mathf.Abs(x_max - x_min);
				line_widths.Add(num10);
				total_text_width += num10;
				if (text_height < total_text_height)
				{
					total_text_height = text_height;
				}
			}
			m_total_text_width = ((m_max_width > 0f) ? m_max_width : total_text_width);
			m_total_text_height = total_text_height * (float)((m_display_axis == TextDisplayAxis.HORIZONTAL) ? 1 : (-1));
			Vector3[] array2 = new Vector3[line_widths.Count];
			for (int k = 0; k < line_widths.Count; k++)
			{
				array2[k] = CalculateTextPositionalOffset(m_text_anchor, m_display_axis, m_text_alignment, line_widths[k]);
			}
			for (int l = 0; l < length2; l++)
			{
				Vector3 b3 = (m_display_axis == TextDisplayAxis.HORIZONTAL) ? new Vector3(0f, (float)(-array[l]) * LineHeight, 0f) : new Vector3((float)array[l] * LineHeight, 0f, 0f);
				m_mesh_verts[l * 4] += array2[array[l]] + b3;
				m_mesh_verts[l * 4 + 1] += array2[array[l]] + b3;
				m_mesh_verts[l * 4 + 2] += array2[array[l]] + b3;
				m_mesh_verts[l * 4 + 3] += array2[array[l]] + b3;
			}
			m_animation_manager.UpdateText(m_text, new TextFxTextDataHandler(m_mesh_verts, m_mesh_cols), white_space_meshes: false);
			if (!m_animation_manager.Playing)
			{
				m_animation_manager.UpdateMesh(use_timer: true, force_render: true);
				if (m_animation_manager.CheckCurveData())
				{
					ForceUpdateCachedVertData();
				}
				UpdateMesh(Application.isPlaying);
			}
		}

		private Vector3 CalculateTextPositionalOffset(TextAnchor anchor, TextDisplayAxis display_axis, TextAlignment alignment, float line_width)
		{
			Vector3 zero = Vector3.zero;
			switch (anchor)
			{
			case TextAnchor.MiddleLeft:
			case TextAnchor.MiddleCenter:
			case TextAnchor.MiddleRight:
				zero += new Vector3(0f, m_total_text_height / 2f - FontBaseLine, 0f);
				break;
			case TextAnchor.LowerLeft:
			case TextAnchor.LowerCenter:
			case TextAnchor.LowerRight:
				zero += new Vector3(0f, m_total_text_height - m_line_height, 0f);
				break;
			default:
				zero += new Vector3(0f, 0f - FontBaseLine, 0f);
				break;
			}
			float num = 0f;
			if (display_axis == TextDisplayAxis.HORIZONTAL)
			{
				switch (alignment)
				{
				case TextAlignment.Center:
					num = (m_total_text_width - line_width) / 2f;
					break;
				case TextAlignment.Right:
					num = m_total_text_width - line_width;
					break;
				}
			}
			else
			{
				switch (alignment)
				{
				case TextAlignment.Center:
					zero -= new Vector3(0f, (m_total_text_height - m_line_height) / 2f, 0f);
					break;
				case TextAlignment.Right:
					zero -= new Vector3(0f, m_total_text_height - m_line_height, 0f);
					break;
				}
			}
			switch (anchor)
			{
			case TextAnchor.UpperRight:
			case TextAnchor.MiddleRight:
			case TextAnchor.LowerRight:
				return zero - new Vector3(m_total_text_width - num, 0f, 0f);
			case TextAnchor.UpperCenter:
			case TextAnchor.MiddleCenter:
			case TextAnchor.LowerCenter:
				return zero - new Vector3(m_total_text_width / 2f - num, 0f, 0f);
			default:
				return zero + new Vector3(num, 0f, 0f);
			}
		}

		public void SetFont(Font font)
		{
			m_font_data_file = null;
			m_font = font;
			m_font_material = null;
			if (m_fontRebuildCallback == null)
			{
				m_fontRebuildCallback = delegate(Font rebuiltFont)
				{
					FontImportDetected(rebuiltFont);
				};
				Font.textureRebuilt += m_fontRebuildCallback;
			}
			SetText(m_text);
		}

		public void SetFont(TextAsset font_data, Material font_material)
		{
			m_font = null;
			m_font_data_file = font_data;
			m_font_material = font_material;
			SetText(m_text);
		}

		private void UpdateMesh(bool animationCall)
		{
			if (m_mesh_uvs == null)
			{
				UnityEngine.Debug.LogError("UpdateMesh() - No mesh UV data available");
				return;
			}
			if (m_mesh == null)
			{
				OnEnable();
			}
			m_mesh.triangles = null;
			m_mesh.vertices = ((animationCall && m_animation_manager.MeshVerts != null) ? m_animation_manager.MeshVerts : m_mesh_verts);
			m_mesh.colors = ((animationCall && m_animation_manager.MeshColours != null) ? m_animation_manager.MeshColours : m_mesh_cols);
			m_mesh.uv = m_mesh_uvs;
			m_mesh.triangles = m_mesh_triangles;
			m_mesh.normals = m_mesh_normals;
			if (OnMeshUpdateCall != null)
			{
				OnMeshUpdateCall();
			}
		}

		private void OnDestroy()
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(m_mesh);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(m_mesh);
			}
		}
	}
}
