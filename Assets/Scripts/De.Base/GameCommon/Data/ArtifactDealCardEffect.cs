using System.Globalization;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public struct ArtifactDealCardEffect
	{
		public EArtifactDealCardEffectType EffectType { get; set; }

		public int Value { get; set; }

		public ArtifactDealCardEffect(EArtifactDealCardEffectType effectType, int value)
		{
			EffectType = effectType;
			Value = value;
		}

		public static ArtifactDealCardEffect Empty()
		{
			return default;
		}

		public override string ToString()
		{
			return $"{EffectType}:{Value}";
		}

		public static bool TryParse(string str, out ArtifactDealCardEffect effect)
		{
			effect = default;
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			string[] parts = str.Split(':');
			if (parts.Length != 2 || !System.Enum.TryParse(parts[0], out EArtifactDealCardEffectType type) || !int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
			{
				return false;
			}
			effect = new ArtifactDealCardEffect(type, value);
			return true;
		}
	}
}
