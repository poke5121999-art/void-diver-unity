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
	private float _frameOutter;

	[Range(0f, 1f)]
	[SerializeField]
	private float _frameAlpha;

	[ColorUsage(true, true)]
	[SerializeField]
	private Color _dissolveColorFrame;

	[SerializeField]
	private float _dissolvePatternScaleFrame;

	[SerializeField]
	private Vector2 _dissolveScaleFrame;

	[ColorUsage(true, true)]
	[SerializeField]
	private Color _smokeColor;

	[SerializeField]
	private float _smokeSpeed;

	[SerializeField]
	private float _smokeScale;

	[ColorUsage(true, true)]
	[SerializeField]
	private Color _ditheringColorFrame;

	[SerializeField]
	private float _ditheringSpeedFrame;

	[SerializeField]
	private float _ditheringScaleFrame;

	[Header("Default Panel Material Properties")]
	[Space(10f)]
	[ColorUsage(true, true)]
	[SerializeField]
	private Color _ditheringColorPanel;

	[SerializeField]
	private float _ditheringSpeedPanel;

	[SerializeField]
	private float _ditheringScalePanel;

	[ColorUsage(true, true)]
	[SerializeField]
	private Color _backgroundColor;

	[Header("Dissolve Frame Material Properties")]
	[Space(10f)]
	[ColorUsage(true, true)]
	[SerializeField]
	private Color _dissolveColorDissolve;

	[SerializeField]
	private float _dissolvePatternScaleDissolve;

	[SerializeField]
	private Vector2 _dissolveScaleDissolve;

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

	public Material DefaultFrameMaterial => null;

	public Material DefaultPanelMaterial => null;

	public Material DissolveFrameMaterial => null;

	private static void InitializePropertyIDs()
	{
	}

	public void ApplyDefaultFrameProperties()
	{
	}

	public void ApplyDefaultPanelProperties()
	{
	}

	public void ApplyDissolveFrameProperties()
	{
	}

	public void ApplyAllMaterialProperties()
	{
	}

	private void OnEnable()
	{
	}
}
