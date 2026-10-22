using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Background SeeThrough Settings", menuName = "NEMO Art/Background SeeThrough Settings")]
public class BackgroundSeeThroughSettings : ScriptableObject
{
	[Space(10f)]
	[SerializeField]
	private bool _alwaysStencil;

	[Space(10f)]
	[SerializeField]
	[Range(0f, 1f)]
	private float _seeThroughAlpha = 0.375f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _seeThroughCircleAlpha = 0.125f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _seeThroughCircleRange = 1f;

	[Space(10f)]
	[SerializeField]
	private Color _seeThroughAreaColor = new Color(0f, 0f, 0f, 1f);

	[SerializeField]
	private float _seeThroughHeightMax = 0.3f;

	[SerializeField]
	private float _seeThroughHeightMin = 0.15f;

	[Space(10f)]
	[SerializeField]
	[FormerlySerializedAs("_seeThroughHeightAlpha")]
	private float _seeThroughBuildingAlpha = 0.15f;

	[SerializeField]
	private float _seeThroughBuildingHeightMax = 2.5f;

	[SerializeField]
	private float _seeThroughBuildingHeightMin = 3f;

	[Space(10f)]
	[SerializeField]
	private Color _characterStencilColor = new Color(0f, 0f, 0.5f, 1f);

	[SerializeField]
	private Color _enemyStencilColor = new Color(0.5f, 0f, 0f, 1f);

	public bool AlwaysStencil
	{
		get
		{
			return _alwaysStencil;
		}
		set
		{
			_alwaysStencil = value;
		}
	}

	public float SeeThroughAlpha
	{
		get
		{
			return _seeThroughAlpha;
		}
		set
		{
			_seeThroughAlpha = value;
		}
	}

	public float SeeThroughCircleAlpha
	{
		get
		{
			return _seeThroughCircleAlpha;
		}
		set
		{
			_seeThroughCircleAlpha = value;
		}
	}

	public float SeeThroughCircleRange
	{
		get
		{
			return _seeThroughCircleRange;
		}
		set
		{
			_seeThroughCircleRange = value;
		}
	}

	public Color SeeThroughAreaColor
	{
		get
		{
			return _seeThroughAreaColor;
		}
		set
		{
			_seeThroughAreaColor = value;
		}
	}

	public float SeeThroughHeightMax
	{
		get
		{
			return _seeThroughHeightMax;
		}
		set
		{
			_seeThroughHeightMax = value;
		}
	}

	public float SeeThroughHeightMin
	{
		get
		{
			return _seeThroughHeightMin;
		}
		set
		{
			_seeThroughHeightMin = value;
		}
	}

	public float SeeThroughBuildingAlpha
	{
		get
		{
			return _seeThroughBuildingAlpha;
		}
		set
		{
			_seeThroughBuildingAlpha = value;
		}
	}

	public float SeeThroughBuildingHeightMax
	{
		get
		{
			return _seeThroughBuildingHeightMax;
		}
		set
		{
			_seeThroughBuildingHeightMax = value;
		}
	}

	public float SeeThroughBuildingHeightMin
	{
		get
		{
			return _seeThroughBuildingHeightMin;
		}
		set
		{
			_seeThroughBuildingHeightMin = value;
		}
	}

	public Color CharacterStencilColor
	{
		get
		{
			return _characterStencilColor;
		}
		set
		{
			_characterStencilColor = value;
		}
	}

	public Color EnemyStencilColor
	{
		get
		{
			return _enemyStencilColor;
		}
		set
		{
			_enemyStencilColor = value;
		}
	}
}
