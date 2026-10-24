using GameCommon.Enum;
using UnityEngine;

namespace GameCommon.Data
{
	public class BuffEffectInfo<T> where T : BuffEffect
	{
		public T BuffEffect;

		public int BuffId;

		public int Stack;

		public ulong[] CasterIds;

		public Vector3 Point;

		// BuffEffectInfo<object>.get_EffectTag @0x182403300 (machine code checked): TBuff.Table[BuffId].EffectTag (field +0x28)
		public EStatusEffectTag EffectTag => Table.TBuff.Table[BuffId].EffectTag;
	}
}
