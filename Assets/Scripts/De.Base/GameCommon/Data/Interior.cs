using System.Globalization;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public class Interior
	{
		public EInteriorType Type;

		public int Id;

		public int Value;

		public Interior(EInteriorType type, int id, int value = 0)
		{
			Type = type;
			Id = id;
			Value = value;
		}

		public override string ToString()
		{
			return Value == 0 ? $"{Type}:{Id}" : $"{Type}:{Id}:{Value}";
		}

		public static bool TryParse(string str, out Interior target)
		{
			target = null;
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			string[] parts = str.Split(':');
			if (parts.Length < 2 || !System.Enum.TryParse(parts[0], out EInteriorType type) || !int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int id))
			{
				return false;
			}
			int value = 0;
			if (parts.Length >= 3 && !int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
			{
				return false;
			}
			target = new Interior(type, id, value);
			return true;
		}
	}
}
