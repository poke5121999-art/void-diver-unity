using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MinimapOuterImage : MonoBehaviour
{
	[SerializeField]
	private Image _miniMapOuterImage;

	[SerializeField]
	private GridLayoutGroup _gridLayoutGroup;

	[SerializeField]
	private MiniMapSettings _miniMapSettings;

	[FormerlySerializedAs("_minimapSightSettings")]
	[SerializeField]
	private MinimapVisualSettings _minimapVisualSettings;

	private int _sectorCount;

	private int ColumnCount => 0;

	private int RowCount => 0;

	public void Initialize(int sectorCount)
	{
	}

	private bool IsValidComponents()
	{
		return false;
	}
}
