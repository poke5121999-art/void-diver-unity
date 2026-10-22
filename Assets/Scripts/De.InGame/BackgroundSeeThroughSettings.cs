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
	private float _seeThroughAlpha;

	[SerializeField]
	[Range(0f, 1f)]
	private float _seeThroughCircleAlpha;

	[SerializeField]
	[Range(0f, 1f)]
	private float _seeThroughCircleRange;

	[Space(10f)]
	[SerializeField]
	private Color _seeThroughAreaColor;

	[SerializeField]
	private float _seeThroughHeightMax;

	[SerializeField]
	private float _seeThroughHeightMin;

	[Space(10f)]
	[SerializeField]
	[FormerlySerializedAs("_seeThroughHeightAlpha")]
	private float _seeThroughBuildingAlpha;

	[SerializeField]
	private float _seeThroughBuildingHeightMax;

	[SerializeField]
	private float _seeThroughBuildingHeightMin;

	[Space(10f)]
	[SerializeField]
	private Color _characterStencilColor;

	[SerializeField]
	private Color _enemyStencilColor;

	public bool AlwaysStencil
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public float SeeThroughAlpha
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float SeeThroughCircleAlpha
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float SeeThroughCircleRange
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public Color SeeThroughAreaColor
	{
		get
		{
			return default;
		}
		set
		{
		}
	}

	public float SeeThroughHeightMax
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float SeeThroughHeightMin
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float SeeThroughBuildingAlpha
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float SeeThroughBuildingHeightMax
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float SeeThroughBuildingHeightMin
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public Color CharacterStencilColor
	{
		get
		{
			return default;
		}
		set
		{
		}
	}

	public Color EnemyStencilColor
	{
		get
		{
			return default;
		}
		set
		{
		}
	}
}
