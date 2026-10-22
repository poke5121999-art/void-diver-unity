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

		// TODO(decomp): generic type has no own body in the binary (get_EffectTag @0x0); likely looked up from the buff table by BuffId
		public EStatusEffectTag EffectTag => EStatusEffectTag.None;
	}
}
