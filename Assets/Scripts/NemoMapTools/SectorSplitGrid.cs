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
		bool positiveX;
		bool positiveZ;
		if (startCorner == EStartCorner.BottomLeft)
		{
			positiveX = true;
			positiveZ = true;
		}
		else
		{
			positiveX = startCorner == EStartCorner.TopLeft;
			positiveZ = startCorner == EStartCorner.BottomRight;
		}
		bool forceX = maxCellsX > 0;
		bool forceZ = maxCellsZ > 0;
		int rawCountX = autoMaxCellX - autoMinCellX + 1;
		int rawCountZ = autoMaxCellZ - autoMinCellZ + 1;
		if (rawCountX <= 0 || rawCountZ <= 0)
		{
			return new List<EffectiveCell>();
		}
		int effX = maxCellsX > 0 ? maxCellsX : rawCountX;
		int effZ = maxCellsZ > 0 ? maxCellsZ : rawCountZ;
		int[][] rangesX = BuildAxisRanges(autoMinCellX, autoMaxCellX, effX, positiveX, forceX);
		int[][] rangesZ = BuildAxisRanges(autoMinCellZ, autoMaxCellZ, effZ, positiveZ, forceZ);
		List<EffectiveCell> list = new List<EffectiveCell>(effZ * effX);
		int num = 0;
		for (int i = 0; i < effX; i++)
		{
			int xMin = Mathf.Min(rangesX[i][0], rangesX[i][1]);
			int xMax = Mathf.Max(rangesX[i][0], rangesX[i][1]);
			for (int j = 0; j < effZ; j++)
			{
				int zMin = Mathf.Min(rangesZ[j][0], rangesZ[j][1]);
				int zMax = Mathf.Max(rangesZ[j][0], rangesZ[j][1]);
				list.Add(new EffectiveCell
				{
					Index = num++,
					RawXMin = xMin,
					RawXMax = xMax,
					RawZMin = zMin,
					RawZMax = zMax,
					WorldXMin = (float)xMin * cellW + pivotPos.x,
					WorldZMin = (float)zMin * cellH + pivotPos.z,
					WorldWidth = (float)(xMax - xMin + 1) * cellW,
					WorldHeight = (float)(zMax - zMin + 1) * cellH
				});
			}
		}
		return list;
	}

	private static int[][] BuildAxisRanges(int rawMin, int rawMax, int effCount, bool positive, bool forceMode)
	{
		int[][] array = new int[effCount][];
		if (!forceMode)
		{
			if (positive)
			{
				for (int i = 0; i < effCount; i++)
				{
					array[i] = new int[2] { rawMin + i, rawMin + i };
				}
			}
			else
			{
				int v = rawMax;
				for (int j = 0; j < effCount; j++)
				{
					array[j] = new int[2] { v, v };
					v--;
				}
			}
			return array;
		}
		if (effCount == 1)
		{
			array[0] = new int[2]
			{
				rawMin >= 0 ? 0 : rawMin,
				rawMax <= 0 ? 0 : rawMax
			};
			return array;
		}
		if (positive)
		{
			array[0] = new int[2] { rawMin >= 0 ? 0 : rawMin, 0 };
			for (int k = 1; k < effCount - 1; k++)
			{
				array[k] = new int[2] { k, k };
			}
			array[effCount - 1] = new int[2] { effCount - 1, Mathf.Max(rawMax, effCount - 1) };
		}
		else
		{
			array[0] = new int[2] { 0, rawMax <= 0 ? 0 : rawMax };
			for (int l = 1; l < effCount - 1; l++)
			{
				array[l] = new int[2] { -l, -l };
			}
			array[effCount - 1] = new int[2] { Mathf.Min(rawMin, 1 - effCount), 1 - effCount };
		}
		return array;
	}
}
