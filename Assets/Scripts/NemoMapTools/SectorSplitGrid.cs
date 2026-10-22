using System.Collections.Generic;
using UnityEngine;

public static class SectorSplitGrid
{
	public enum EStartCorner
	{
		BottomLeft = 0,
		BottomRight = 1,
		TopLeft = 2,
		TopRight = 3
	}

	public struct EffectiveCell
	{
		public int Index;

		public int RawXMin;

		public int RawXMax;

		public int RawZMin;

		public int RawZMax;

		public float WorldXMin;

		public float WorldZMin;

		public float WorldWidth;

		public float WorldHeight;
	}

	public static List<EffectiveCell> Compute(Vector3 pivotPos, float cellW, float cellH, int autoMinCellX, int autoMaxCellX, int autoMinCellZ, int autoMaxCellZ, int maxCellsX, int maxCellsZ, EStartCorner startCorner)
	{
		return null;
	}

	private static int[][] BuildAxisRanges(int rawMin, int rawMax, int effCount, bool positive, bool forceMode)
	{
		return null;
	}
}
