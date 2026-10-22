using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class BackgroundSeeThroughController : MonoBehaviour
{
	[Header("Background See-Through Hard Setting")]
	[SerializeField]
	private bool _seeThroughEnable;

	[Header("Background See-Through Assign")]
	[SerializeField]
	private BackgroundSeeThroughSettings _backgroundSeeThroughSettings;

	[Header("Background See-Through Setting")]
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
	private float _seeThroughBuidlingAlpha;

	[SerializeField]
	private float _seeThroughBuildingHeightMax;

	[SerializeField]
	private float _seeThroughBuildingHeightMin;

	[Space(10f)]
	[SerializeField]
	private Color _characterStencilColor;

	[SerializeField]
	private Color _enemyStencilColor;

	private static readonly int _seeThroughEnableID;

	private static readonly int _alwaysStencilID;

	private static readonly int _seeThroughAlphaID;

	private static readonly int _seeThroughCircleAlphaID;

	private static readonly int _seeThroughCircleRangeID;

	private static readonly int _seeThroughAreaColorID;

	private static readonly int _seeThroughHeightMaxID;

	private static readonly int _seeThroughHeightMinID;

	private static readonly int _seeThroughBuildingAlphaID;

	private static readonly int _seeThroughBuildingHeightMaxID;

	private static readonly int _seeThroughBuildingHeightMinID;

	private static readonly int _characterStencilColorID;

	private static readonly int _enemyStencilColorID;

	private bool _isInitialized;

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnValidate()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void UpdateProperties()
	{
	}

	private void ApplyScriptableSettingsToLocalFields()
	{
	}

	private void UpdateScriptableSettings()
	{
	}
}
