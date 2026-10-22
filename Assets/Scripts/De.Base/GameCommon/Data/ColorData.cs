using System;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class ColorData
	{
		public float r;

		public float g;

		public float b;

		public float a;

		public ColorData(Color c)
		{
		}

		public Color ToColor()
		{
			return default;
		}
	}
}
