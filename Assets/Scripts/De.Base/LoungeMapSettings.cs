using UnityEngine;

[CreateAssetMenu(menuName = "Settings/LoungeMapSettings")]
public class LoungeMapSettings : ScriptableObject
{
	[Min(1f)]
	public int Width;

	[Min(1f)]
	public int Height;

	public int[] SectorIds;

	public int[] MapSize => null;

	private void OnValidate()
	{
	}
}
