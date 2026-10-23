using UnityEngine;

[CreateAssetMenu(fileName = "UI Material Settings", menuName = "UI/UI Material Settings")]
public class UIMaterialSettings : ScriptableObject
{
	[Header("UI Default Materials")]
	[SerializeField]
	private Material _defaultFrameMaterial;

	[SerializeField]
	private Material _defaultPanelMaterial;

	[SerializeField]
	private Material _dissolveFrameMaterial;

	[Header("Default Frame Material Properties")]
	[Space(10f)]
	[SerializeField]
	private float _frameInner;

	[SerializeField]
	private float _frameOutter = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _frameAlpha = 1f;

	[ColorUsage(true, true)]
	[SerializeField]
	private Color _dissolveColorFrame = new Color(0f, 1f, 0f, 1f);

	[SerializeField]
	private float _dissolvePatternScaleFrame = 1f;

	[SerializeField]
	private Vector2 _dissolveScaleFrame = Vector2.one;

	[ColorUsage(true, true)]
	[SerializeField]
	private Color _smokeColor = new Color(1f, 0f, 0f, 1f);

	[SerializeField]
	private float _smokeSpeed = 1f;

	[SerializeField]
	private float _smokeScale = 1f;

	[ColorUsage(true, true)]
	[SerializeField]
	private Color _ditheringColorFrame = new Color(0f, 0f, 0f, 1f);

	[SerializeField]
	private float _ditheringSpeedFrame = 1f;

	[SerializeField]
	private float _ditheringScaleFrame = 1f;

	[Header("Default Panel Material Properties")]
	[Space(10f)]
	[ColorUsage(true, true)]
	[SerializeField]
	private Color _ditheringColorPanel = new Color(0f, 0f, 0f, 1f);

	[SerializeField]
	private float _ditheringSpeedPanel = 1f;

	[SerializeField]
	private float _ditheringScalePanel = 1f;

	[ColorUsage(true, true)]
	[SerializeField]
	private Color _backgroundColor = new Color(0f, 0f, 0f, 1f);

	[Header("Dissolve Frame Material Properties")]
	[Space(10f)]
	[ColorUsage(true, true)]
	[SerializeField]
	private Color _dissolveColorDissolve = new Color(0f, 1f, 0f, 1f);

	[SerializeField]
	private float _dissolvePatternScaleDissolve = 1f;

	[SerializeField]
	private Vector2 _dissolveScaleDissolve = Vector2.one;

	private static int _frameInnerID;

	private static int _frameOutterID;

	private static int _frameAlphaID;

	private static int _dissolveColorID;

	private static int _dissolvePatternScaleID;

	private static int _dissolveScaleID;

	private static int _smokeColorID;

	private static int _smokeSpeedID;

	private static int _smokeScaleID;

	private static int _ditheringColorID;

	private static int _ditheringSpeedID;

	private static int _ditheringScaleID;

	private static int _backgroundColorID;

	private static bool _propertyIDsInitialized;

	public Material DefaultFrameMaterial => _defaultFrameMaterial;

	public Material DefaultPanelMaterial => _defaultPanelMaterial;

	public Material DissolveFrameMaterial => _dissolveFrameMaterial;

	private static void InitializePropertyIDs()
	{
		if (!_propertyIDsInitialized)
		{
			_frameInnerID = Shader.PropertyToID("_FrameInner");
			_frameOutterID = Shader.PropertyToID("_FrameOutter");
			_frameAlphaID = Shader.PropertyToID("_FrameAlpha");
			_dissolveColorID = Shader.PropertyToID("_DissolveColor");
			_dissolvePatternScaleID = Shader.PropertyToID("_DissolvePatternScale");
			_dissolveScaleID = Shader.PropertyToID("_DissolveScale");
			_smokeColorID = Shader.PropertyToID("_SmokeColor");
			_smokeSpeedID = Shader.PropertyToID("_SmokeSpeed");
			_smokeScaleID = Shader.PropertyToID("_SmokeScale");
			_ditheringColorID = Shader.PropertyToID("_DitheringColor");
			_ditheringSpeedID = Shader.PropertyToID("_DitheringSpeed");
			_ditheringScaleID = Shader.PropertyToID("_DitheringScale");
			_backgroundColorID = Shader.PropertyToID("_BackgroundColor");
			_propertyIDsInitialized = true;
		}
	}

	public void ApplyDefaultFrameProperties()
	{
		InitializePropertyIDs();
		if (_defaultFrameMaterial != null)
		{
			_defaultFrameMaterial.SetFloat(_frameInnerID, _frameInner);
			_defaultFrameMaterial.SetFloat(_frameOutterID, _frameOutter);
			_defaultFrameMaterial.SetFloat(_frameAlphaID, _frameAlpha);
			_defaultFrameMaterial.SetColor(_dissolveColorID, _dissolveColorFrame);
			_defaultFrameMaterial.SetFloat(_dissolvePatternScaleID, _dissolvePatternScaleFrame);
			_defaultFrameMaterial.SetVector(_dissolveScaleID, _dissolveScaleFrame);
			_defaultFrameMaterial.SetColor(_smokeColorID, _smokeColor);
			_defaultFrameMaterial.SetFloat(_smokeSpeedID, _smokeSpeed);
			_defaultFrameMaterial.SetFloat(_smokeScaleID, _smokeScale);
			_defaultFrameMaterial.SetColor(_ditheringColorID, _ditheringColorFrame);
			_defaultFrameMaterial.SetFloat(_ditheringSpeedID, _ditheringSpeedFrame);
			_defaultFrameMaterial.SetFloat(_ditheringScaleID, _ditheringScaleFrame);
		}
	}

	public void ApplyDefaultPanelProperties()
	{
		InitializePropertyIDs();
		if (_defaultPanelMaterial != null)
		{
			_defaultPanelMaterial.SetColor(_ditheringColorID, _ditheringColorPanel);
			_defaultPanelMaterial.SetFloat(_ditheringSpeedID, _ditheringSpeedPanel);
			_defaultPanelMaterial.SetFloat(_ditheringScaleID, _ditheringScalePanel);
			_defaultPanelMaterial.SetColor(_backgroundColorID, _backgroundColor);
		}
	}

	public void ApplyDissolveFrameProperties()
	{
		InitializePropertyIDs();
		if (_dissolveFrameMaterial != null)
		{
			_dissolveFrameMaterial.SetColor(_dissolveColorID, _dissolveColorDissolve);
			_dissolveFrameMaterial.SetFloat(_dissolvePatternScaleID, _dissolvePatternScaleDissolve);
			_dissolveFrameMaterial.SetVector(_dissolveScaleID, _dissolveScaleDissolve);
		}
	}

	public void ApplyAllMaterialProperties()
	{
		ApplyDefaultFrameProperties();
		ApplyDefaultPanelProperties();
		ApplyDissolveFrameProperties();
	}

	private void OnEnable()
	{
		InitializePropertyIDs();
	}
}
