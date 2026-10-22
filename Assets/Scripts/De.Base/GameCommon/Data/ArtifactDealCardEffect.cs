using GameCommon.Enum;

namespace GameCommon.Data
{
	public struct ArtifactDealCardEffect
	{
		public EArtifactDealCardEffectType EffectType { get; set; }

		public int Value { get; set; }

		public ArtifactDealCardEffect(EArtifactDealCardEffectType effectType, int value)
		{
			EffectType = EArtifactDealCardEffectType.None;
			Value = 0;
		}

		public static ArtifactDealCardEffect Empty()
		{
			return default;
		}

		public static bool TryParse(string str, out ArtifactDealCardEffect effect)
		{
			effect = default;
			return false;
		}
	}
}
