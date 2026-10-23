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
			r = c.r;
			g = c.g;
			b = c.b;
			a = c.a;
		}

		public Color ToColor()
		{
			return new Color(r, g, b, a);
		}
	}
}
