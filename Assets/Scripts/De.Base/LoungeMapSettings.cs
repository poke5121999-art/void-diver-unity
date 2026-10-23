using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/LoungeMapSettings")]
public class LoungeMapSettings : ScriptableObject
{
	[Min(1f)]
	public int Width = 3;

	[Min(1f)]
	public int Height = 3;

	public int[] SectorIds = new int[9] { 9000, 9000, 9000, 9000, 9002, 9000, 9000, 9000, 9000 };

	public int[] MapSize => new int[2] { Width, Height };

	private void OnValidate()
	{
		int num = Height * Width;
		if (SectorIds == null || SectorIds.Length != num)
		{
			int[] array = new int[num];
			if (SectorIds != null)
			{
				for (int i = 0; i < Math.Min(SectorIds.Length, num); i++)
				{
					array[i] = SectorIds[i];
				}
			}
			SectorIds = array;
		}
	}
}
