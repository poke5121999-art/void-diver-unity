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
	private float _seeThroughBuidlingAlpha = 0.15f;

	[SerializeField]
	private float _seeThroughBuildingHeightMax = 2.5f;

	[SerializeField]
	private float _seeThroughBuildingHeightMin = 3f;

	[Space(10f)]
	[SerializeField]
	private Color _characterStencilColor = new Color(0f, 0f, 0.5f, 1f);

	[SerializeField]
	private Color _enemyStencilColor = new Color(0.5f, 0f, 0f, 1f);

	private static readonly int _seeThroughEnableID = Shader.PropertyToID("_GlobalBackgroundSeeThroughEnable");

	private static readonly int _alwaysStencilID = Shader.PropertyToID("_GlobalAlwaysStencil");

	private static readonly int _seeThroughAlphaID = Shader.PropertyToID("_GlobalSeeThroughAlpha");

	private static readonly int _seeThroughCircleAlphaID = Shader.PropertyToID("_GlobalSeeThroughCircleAlpha");

	private static readonly int _seeThroughCircleRangeID = Shader.PropertyToID("_GlobalSeeThroughCircleRange");

	private static readonly int _seeThroughAreaColorID = Shader.PropertyToID("_GlobalSeeThroughAreaColor");

	private static readonly int _seeThroughHeightMaxID = Shader.PropertyToID("_GlobalSeeThroughHeightMax");

	private static readonly int _seeThroughHeightMinID = Shader.PropertyToID("_GlobalSeeThroughHeightMin");

	private static readonly int _seeThroughBuildingAlphaID = Shader.PropertyToID("_GlobalSeeThroughBuildingAlpha");

	private static readonly int _seeThroughBuildingHeightMaxID = Shader.PropertyToID("_GlobalSeeThroughBuildingHeightMax");

	private static readonly int _seeThroughBuildingHeightMinID = Shader.PropertyToID("_GlobalSeeThroughBuildingHeightMin");

	private static readonly int _characterStencilColorID = Shader.PropertyToID("_GlobalCharacterStencilColor");

	private static readonly int _enemyStencilColorID = Shader.PropertyToID("_GlobalEnemyStencilColor");

	private bool _isInitialized;

	private void Start()
	{
		ApplyEnableFlag();
		ApplyScriptableSettingsToLocalFields();
		UpdateProperties();
		_isInitialized = true;
	}

	private void OnEnable()
	{
		ApplyEnableFlag();
		ApplyScriptableSettingsToLocalFields();
		UpdateProperties();
		_isInitialized = true;
	}

	private void ApplyEnableFlag()
	{
		Shader.SetGlobalFloat(_seeThroughEnableID, (Application.isPlaying && _seeThroughEnable) ? 1f : 0f);
	}

	private void OnValidate()
	{
		ApplyEnableFlag();
		if (_isInitialized)
		{
			UpdateScriptableSettings();
		}
		UpdateProperties();
	}

	private void OnDisable()
	{
		Shader.SetGlobalFloat(_seeThroughEnableID, 0f);
	}

	private void OnDestroy()
	{
		Shader.SetGlobalFloat(_seeThroughEnableID, 0f);
	}

	private void UpdateProperties()
	{
		if (_backgroundSeeThroughSettings == null)
		{
			return;
		}
		Shader.SetGlobalFloat(_alwaysStencilID, _backgroundSeeThroughSettings.AlwaysStencil ? 1f : 0f);
		Shader.SetGlobalFloat(_seeThroughAlphaID, _backgroundSeeThroughSettings.SeeThroughAlpha);
		Shader.SetGlobalFloat(_seeThroughCircleAlphaID, _backgroundSeeThroughSettings.SeeThroughCircleAlpha);
		Shader.SetGlobalFloat(_seeThroughCircleRangeID, _backgroundSeeThroughSettings.SeeThroughCircleRange);
		Shader.SetGlobalColor(_seeThroughAreaColorID, _backgroundSeeThroughSettings.SeeThroughAreaColor);
		Shader.SetGlobalFloat(_seeThroughHeightMaxID, _backgroundSeeThroughSettings.SeeThroughHeightMax);
		Shader.SetGlobalFloat(_seeThroughHeightMinID, _backgroundSeeThroughSettings.SeeThroughHeightMin);
		Shader.SetGlobalFloat(_seeThroughBuildingAlphaID, _backgroundSeeThroughSettings.SeeThroughBuildingAlpha);
		Shader.SetGlobalFloat(_seeThroughBuildingHeightMaxID, _backgroundSeeThroughSettings.SeeThroughBuildingHeightMax);
		Shader.SetGlobalFloat(_seeThroughBuildingHeightMinID, _backgroundSeeThroughSettings.SeeThroughBuildingHeightMin);
		Shader.SetGlobalColor(_characterStencilColorID, _backgroundSeeThroughSettings.CharacterStencilColor);
		Shader.SetGlobalColor(_enemyStencilColorID, _backgroundSeeThroughSettings.EnemyStencilColor);
	}

	private void ApplyScriptableSettingsToLocalFields()
	{
		if (_backgroundSeeThroughSettings == null)
		{
			return;
		}
		_alwaysStencil = _backgroundSeeThroughSettings.AlwaysStencil;
		_seeThroughAlpha = _backgroundSeeThroughSettings.SeeThroughAlpha;
		_seeThroughCircleAlpha = _backgroundSeeThroughSettings.SeeThroughCircleAlpha;
		_seeThroughCircleRange = _backgroundSeeThroughSettings.SeeThroughCircleRange;
		_seeThroughAreaColor = _backgroundSeeThroughSettings.SeeThroughAreaColor;
		_seeThroughHeightMax = _backgroundSeeThroughSettings.SeeThroughHeightMax;
		_seeThroughHeightMin = _backgroundSeeThroughSettings.SeeThroughHeightMin;
		_seeThroughBuidlingAlpha = _backgroundSeeThroughSettings.SeeThroughBuildingAlpha;
		_seeThroughBuildingHeightMax = _backgroundSeeThroughSettings.SeeThroughBuildingHeightMax;
		_seeThroughBuildingHeightMin = _backgroundSeeThroughSettings.SeeThroughBuildingHeightMin;
		_characterStencilColor = _backgroundSeeThroughSettings.CharacterStencilColor;
		_enemyStencilColor = _backgroundSeeThroughSettings.EnemyStencilColor;
	}

	private void UpdateScriptableSettings()
	{
		if (_backgroundSeeThroughSettings == null)
		{
			return;
		}
		_backgroundSeeThroughSettings.AlwaysStencil = _alwaysStencil;
		_backgroundSeeThroughSettings.SeeThroughAlpha = _seeThroughAlpha;
		_backgroundSeeThroughSettings.SeeThroughCircleAlpha = _seeThroughCircleAlpha;
		_backgroundSeeThroughSettings.SeeThroughCircleRange = _seeThroughCircleRange;
		_backgroundSeeThroughSettings.SeeThroughAreaColor = _seeThroughAreaColor;
		_backgroundSeeThroughSettings.SeeThroughHeightMax = _seeThroughHeightMax;
		_backgroundSeeThroughSettings.SeeThroughHeightMin = _seeThroughHeightMin;
		_backgroundSeeThroughSettings.SeeThroughBuildingAlpha = _seeThroughBuidlingAlpha;
		_backgroundSeeThroughSettings.SeeThroughBuildingHeightMax = _seeThroughBuildingHeightMax;
		_backgroundSeeThroughSettings.SeeThroughBuildingHeightMin = _seeThroughBuildingHeightMin;
		_backgroundSeeThroughSettings.CharacterStencilColor = _characterStencilColor;
		_backgroundSeeThroughSettings.EnemyStencilColor = _enemyStencilColor;
	}
}
