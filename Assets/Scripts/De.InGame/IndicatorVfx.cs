using System;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using Cysharp.Threading.Tasks;
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

	private readonly int _progressPropertyID = Shader.PropertyToID("_Progress");

	private static readonly int _coneAngleId = Shader.PropertyToID("_ConeAngle");

	private IndicatorInfo _indicatorInfo;

	private UnitModel _casterUnitModel;

	private float _angleOffset;

	private bool _isFlipped;

	private int _skillId;

	private float _skillTimeScale;

	private Vector3 _calculatedAimPos;

	private NetworkVariable<float> _Progress = new NetworkVariable<float>();

	public void Init(IndicatorInfo indicatorInfo, UnitModel casterUnitModel, Vector3 position, float angleOffset, bool isFlipped, int skillId, float skillTimeScale)
	{
		float elapsed;
		_indicatorInfo = indicatorInfo;
		_casterUnitModel = casterUnitModel;
		_angleOffset = angleOffset;
		_isFlipped = isFlipped;
		_skillTimeScale = skillTimeScale;
		_skillId = skillId;
		switch (_indicatorInfo.IndicatorVfxType)
		{
		case EIndicatorVfxType.Circle:
		case EIndicatorVfxType.Square:
		case EIndicatorVfxType.Cone:
			UpdateTransform(position, casterUnitModel.CalculatedDeltaAimPos);
			break;
		case EIndicatorVfxType.AimingLine:
			Observable.EveryUpdate().Subscribe(delegate
			{
				UpdateTransform(position, casterUnitModel.CalculatedDeltaAimPos);
			}).AddTo(this);
			break;
		default:
			_logger.Error(string.Format("IndicatorVfxEvent Type {0} is None", _indicatorInfo.IndicatorVfxType));
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (_indicatorInfo.IsCancelOnOwnerDie)
		{
			_casterUnitModel.RxState.Where((EActionState state) => state == EActionState.Dying || state == EActionState.Dead).Take(1).Subscribe(delegate
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}).AddTo(this);
		}
		elapsed = 0f;
		Observable.EveryFixedUpdate().TakeWhile((long _) =>
		{
			elapsed += Time.fixedDeltaTime * _skillTimeScale;
			return elapsed < _indicatorInfo.FillDuration;
		}).Subscribe(delegate
		{
			_Progress.Value = _curve.Evaluate(elapsed / _indicatorInfo.FillDuration);
		}).AddTo(this);
		if (_indicatorInfo.HitBoxSpawnDelay > 0f)
		{
			Observable.Timer(TimeSpan.FromSeconds(_indicatorInfo.HitBoxSpawnDelay / _skillTimeScale)).Subscribe(delegate
			{
				SpawnHitBox();
			}).AddTo(this);
		}
		Observable.Timer(TimeSpan.FromSeconds(_indicatorInfo.ShowDuration / _skillTimeScale)).Subscribe(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}).AddTo(this);
	}

	private void UpdateTransform(Vector3 position, Vector3 aimPos)
	{
		_calculatedAimPos = Quaternion.Euler(0f, _angleOffset, 0f) * aimPos;
		Vector3 normalized = _calculatedAimPos.normalized;
		Quaternion quaternion = ((!(normalized != Vector3.zero)) ? base.transform.rotation : Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z), Vector3.up));
		base.transform.position = position + quaternion * _indicatorInfo.SpawnOffset;
		base.transform.rotation = quaternion;
		if (_indicatorInfo.SpawnHitBoxAtOwnerPosition)
		{
			_calculatedAimPos = base.transform.position - _casterUnitModel.transform.position;
		}
		switch (_indicatorInfo.IndicatorVfxType)
		{
		case EIndicatorVfxType.Circle:
			base.transform.localScale = new Vector3(_indicatorInfo.CircleRadius * 2f, 1f, _indicatorInfo.CircleRadius * 2f);
			break;
		case EIndicatorVfxType.Square:
			base.transform.localScale = new Vector3(_indicatorInfo.SquareSize.x, 1f, _indicatorInfo.SquareSize.y);
			break;
		case EIndicatorVfxType.AimingLine:
		{
			Vector3 normalized2 = aimPos.normalized;
			float z = ((!Physics.Raycast(position, normalized2, out var hitInfo, aimPos.magnitude, Const.LayerMask.HIGH_OBSTACLE)) ? aimPos.magnitude : hitInfo.distance);
			base.transform.localScale = new Vector3(0.1f, 1f, z);
			break;
		}
		case EIndicatorVfxType.Cone:
			base.transform.localScale = new Vector3(_indicatorInfo.ConeRadius, 1f, _indicatorInfo.ConeRadius);
			ApplyConeAngle(_indicatorInfo.ConeAngle);
			break;
		default:
			_logger.Error(string.Format("IndicatorVfxEvent.IndicatorVfxType {0} is not supported.", _indicatorInfo.IndicatorVfxType));
			break;
		}
	}

	public void DestroyIndicateVfx()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void SpawnHitBox()
	{
		if (_indicatorInfo.HitBoxId > 0 && !(_casterUnitModel == null))
		{
			Vector3 position = (_indicatorInfo.SpawnHitBoxAtOwnerPosition ? _casterUnitModel.transform : base.transform).position;
			_objectManager.SpawnHitBoxAsync(_indicatorInfo.HitBoxId, _casterUnitModel, new ActionStatsData(_casterUnitModel), _casterUnitModel.GetHitLayerInfo(), _calculatedAimPos, position, _isFlipped, _skillId).Forget();
		}
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_materialPropertyBlock = new MaterialPropertyBlock();
		_Progress.OnValueChanged += OnProgressChanged;
		if (base.IsHost && !_indicatorInfo.IsIndependent && _casterUnitModel != null)
		{
			base.transform.parent = _casterUnitModel.transform;
		}
	}

	private void OnProgressChanged(float oldValue, float newValue)
	{
		if (!(_skinnedMeshRenderer == null))
		{
			_skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);
			_materialPropertyBlock.SetFloat(_progressPropertyID, newValue);
			_skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
		}
	}

	private void ApplyConeAngle(float angle)
	{
		if (_materialPropertyBlock == null)
		{
			_materialPropertyBlock = new MaterialPropertyBlock();
		}
		_skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);
		_materialPropertyBlock.SetFloat(_coneAngleId, angle);
		_skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
	}
}
