using NemoLib.Log;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class IndicatorVfx : NetworkBase
{
	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private SeriLogger _logger;

	[SerializeField]
	private AnimationCurve _curve;

	[SerializeField]
	private SkinnedMeshRenderer _skinnedMeshRenderer;

	private MaterialPropertyBlock _materialPropertyBlock;

	private readonly int _progressPropertyID;

	private static readonly int _coneAngleId;

	private IndicatorInfo _indicatorInfo;

	private UnitModel _casterUnitModel;

	private float _angleOffset;

	private bool _isFlipped;

	private int _skillId;

	private float _skillTimeScale;

	private Vector3 _calculatedAimPos;

	private NetworkVariable<float> _Progress;

	public void Init(IndicatorInfo indicatorInfo, UnitModel casterUnitModel, Vector3 position, float angleOffset, bool isFlipped, int skillId, float skillTimeScale)
	{
	}

	private void UpdateTransform(Vector3 position, Vector3 aimPos)
	{
	}

	public void DestroyIndicateVfx()
	{
	}

	private void SpawnHitBox()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	private void OnProgressChanged(float oldValue, float newValue)
	{
	}

	private void ApplyConeAngle(float angle)
	{
	}

	protected override void __initializeVariables()
	{
	}

	protected override void __initializeRpcs()
	{
	}

	protected internal override string __getTypeName()
	{
		return null;
	}
}
