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

	private int ColumnCount
	{
		get
		{
			if (!_gridLayoutGroup)
			{
				return 0;
			}
			return _gridLayoutGroup.constraintCount;
		}
	}

	private int RowCount
	{
		get
		{
			int columnCount = ColumnCount;
			if (columnCount < 1 || _sectorCount < 1)
			{
				return 0;
			}
			return (int)System.Math.Ceiling((float)_sectorCount / (float)columnCount);
		}
	}

	public void Initialize(int sectorCount)
	{
		if (IsValidComponents())
		{
			_sectorCount = sectorCount;
			_miniMapOuterImage.color = Color.white;
			RectTransform rectTransform = _miniMapOuterImage.rectTransform;
			rectTransform.pivot = Vector2.zero;
			Vector2 resolution = _miniMapSettings.GetResolution();
			Vector2 one = Vector2.one;
			float num = one.x * _minimapVisualSettings.OuterSizeUnit * (float)ColumnCount;
			float num2 = one.y * _minimapVisualSettings.OuterSizeUnit * (float)ColumnCount;
			rectTransform.localPosition = new Vector3(0f - num, 0f - num2, 0f);
			rectTransform.sizeDelta = new Vector2((float)ColumnCount * resolution.x + num + num, (float)RowCount * resolution.y + num2 + num2);
			_miniMapOuterImage.material = _minimapVisualSettings.OuterMaterial;
		}
	}

	private bool IsValidComponents()
	{
		if (_miniMapOuterImage == null)
		{
			_miniMapOuterImage = GetComponent<Image>();
		}
		if (_miniMapOuterImage != null && _gridLayoutGroup != null && _miniMapSettings != null)
		{
			return _minimapVisualSettings != null;
		}
		return false;
	}
}
