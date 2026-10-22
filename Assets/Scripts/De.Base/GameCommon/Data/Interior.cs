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
		}

		public override string ToString()
		{
			return null;
		}

		public static bool TryParse(string str, out Interior target)
		{
			target = null;
			return false;
		}
	}
}
