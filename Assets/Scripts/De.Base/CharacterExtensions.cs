using System;
using System.Collections.Generic;
using SomeNet.Utility;
using GameCommon.Data;
using GameCommon.Enum;

public static class CharacterExtensions
{
	public static (Character, Goods[]) ApplyStageDead(this Character character, ERoomDifficulty difficulty)
	{
		if (character == null || (int)difficulty < 2)
		{
			return (character, Array.Empty<Goods>());
		}
		float removalPercent = difficulty.GetGoodsRemovalPercent();
		List<Goods> lostGoodsList = new List<Goods>();
		Character clone = character.Clone();
		HandleSlots(clone.WeaponSlots);
		HandleSlots(clone.ArtifactSlots);
		HandleSlots(clone.AccessorySlots);
		return (clone, lostGoodsList.ToArray());

		void HandleSlots(GoodsSlot[] slots)
		{
			foreach (GoodsSlot slot in slots)
			{
				if (slot.Goods != null && RandomUtility.GetRandomBoolFromPercentage(removalPercent))
				{
					lostGoodsList.Add(slot.Goods);
					slot.Goods = null;
				}
			}
		}
	}

	public static Character NormalizeCount(this Character character)
	{
		Character clone = character.Clone();
		Normalize(clone.WeaponSlots);
		Normalize(clone.AccessorySlots);
		Normalize(clone.ArtifactSlots);
		return clone;

		static void Normalize(GoodsSlot[] slots)
		{
			foreach (GoodsSlot slot in slots)
			{
				if (slot.Goods != null && slot.Goods.GainCount > 0)
				{
					slot.Goods = slot.Goods.CloneWithoutGain();
				}
			}
		}
	}
}
