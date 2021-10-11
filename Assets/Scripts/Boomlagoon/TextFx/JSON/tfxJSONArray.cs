using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Boomlagoon.TextFx.JSON
{
	public class tfxJSONArray : IEnumerable<tfxJSONValue>, IEnumerable
	{
		private readonly List<tfxJSONValue> values = new List<tfxJSONValue>();

		public tfxJSONValue this[int index]
		{
			get
			{
				return values[index];
			}
			set
			{
				values[index] = value;
			}
		}

		public int Length => values.Count;

		public tfxJSONArray()
		{
		}

		public tfxJSONArray(tfxJSONArray array)
		{
			values = new List<tfxJSONValue>();
			foreach (tfxJSONValue value in array.values)
			{
				values.Add(new tfxJSONValue(value));
			}
		}

		public void Add(tfxJSONValue value)
		{
			values.Add(value);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			foreach (tfxJSONValue value in values)
			{
				stringBuilder.Append(value.ToString());
				stringBuilder.Append(',');
			}
			if (values.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		public IEnumerator<tfxJSONValue> GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return values.GetEnumerator();
		}

		public static tfxJSONArray Parse(string jsonString)
		{
			return tfxJSONObject.Parse("{ \"array\" :" + jsonString + "}")?.GetValue("array").Array;
		}

		public void Clear()
		{
			values.Clear();
		}

		public void Remove(int index)
		{
			if (index >= 0 && index < values.Count)
			{
				values.RemoveAt(index);
			}
			else
			{
				JSONLogger.Error("index out of range: " + index + " (Expected 0 <= index < " + values.Count + ")");
			}
		}

		public static tfxJSONArray operator +(tfxJSONArray lhs, tfxJSONArray rhs)
		{
			tfxJSONArray tfxJSONArray = new tfxJSONArray(lhs);
			foreach (tfxJSONValue value in rhs.values)
			{
				tfxJSONArray.Add(value);
			}
			return tfxJSONArray;
		}
	}
}
