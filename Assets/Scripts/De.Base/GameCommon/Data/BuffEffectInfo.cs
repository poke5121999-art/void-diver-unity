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

		public EStatusEffectTag EffectTag => EStatusEffectTag.None;
	}
}
