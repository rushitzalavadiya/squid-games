using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace HSVPickerDemo
{
	public class HexRGB : MonoBehaviour
	{
		public InputField textColor;

		public HSVPicker hsvpicker;

		public void ManipulateViaRGB2Hex()
		{
			string text = ColorToHex(hsvpicker.currentColor);
			textColor.text = text;
		}

		public static string ColorToHex(Color color)
		{
			int num = Mathf.RoundToInt(color.r * 255f);
			int num2 = Mathf.RoundToInt(color.g * 255f);
			int num3 = Mathf.RoundToInt(color.b * 255f);
			return $"#{num:X2}{num2:X2}{num3:X2}";
		}

		public void ManipulateViaHex2RGB()
		{
			string text = textColor.text;
			Color color = Hex2RGB(text);
			hsvpicker.AssignColor(color);
		}

		private static Color NormalizeVector4(Vector3 v, float r, float a)
		{
			float r2 = v.x / r;
			float g = v.y / r;
			float b = v.z / r;
			return new Color(r2, g, b, a);
		}

		private Color Hex2RGB(string hexColor)
		{
			if (hexColor.IndexOf('#') != -1)
			{
				hexColor = hexColor.Replace("#", "");
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (hexColor.Length == 6)
			{
				num = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
				num2 = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
				num3 = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);
			}
			else if (hexColor.Length == 3)
			{
				num = int.Parse(hexColor[0].ToString() + hexColor[0].ToString(), NumberStyles.AllowHexSpecifier);
				num2 = int.Parse(hexColor[1].ToString() + hexColor[1].ToString(), NumberStyles.AllowHexSpecifier);
				num3 = int.Parse(hexColor[2].ToString() + hexColor[2].ToString(), NumberStyles.AllowHexSpecifier);
			}
			return new Color32((byte)num, (byte)num2, (byte)num3, byte.MaxValue);
		}
	}
}
