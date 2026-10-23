using System;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using UniRx;
using UnityEngine;

public class UnitFollower : MonoBehaviour
{
	private UnitView _unitView;

	private UnitModel _unitModel;

	private EUnitBoneType _boneType;

	private Vector3 _offset;

	private bool _inheritMoveDir;

	private bool _updateByAimDir;

	private bool _destroyOnUnitDeath = true;

	private bool _isInitialized;

	private CompositeDisposable _disposable;

	public Subject<Unit> OnDespawned { get; } = new Subject<Unit>();

	private void Awake()
	{
		PoolableObject component = GetComponent<PoolableObject>();
		if (component != null)
		{
			component.OnDespawned.Subscribe(delegate
			{
				_isInitialized = false;
			}).AddTo(this);
		}
	}

	public void Init(UnitModel unitModel, EUnitBoneType boneType, Vector3 offset, bool inheritMoveDir = false, bool destroyOnUnitDeath = false, bool updateByAimDir = false)
	{
		if (_disposable != null)
		{
			_disposable.Dispose();
		}
		_disposable = new CompositeDisposable();
		_unitModel = unitModel;
		_unitView = unitModel.GetComponent<UnitView>();
		_offset = offset;
		_destroyOnUnitDeath = destroyOnUnitDeath;
		_inheritMoveDir = inheritMoveDir;
		_updateByAimDir = updateByAimDir;
		_boneType = boneType;
		_isInitialized = true;
		if (_unitView != null)
		{
			_unitView.OnModelAnchorActiveChanged.Subscribe(delegate(bool isActive)
			{
				base.gameObject.SetActive(isActive);
			}).AddTo(_disposable);
		}
	}

	private void Update()
	{
		if (!_isInitialized)
		{
			return;
		}
		if (_unitView == null)
		{
			Despawn();
			return;
		}
		if (_destroyOnUnitDeath)
		{
			EActionState state = _unitModel.State;
			if (state == EActionState.Dying || state == EActionState.Dead || state == EActionState.ReservedDespawn)
			{
				Despawn();
				return;
			}
		}
		if (_boneType == EUnitBoneType.None)
		{
			Vector3 normalized = (_inheritMoveDir ? _unitModel.MoveDir : _unitModel.Forward).normalized;
			if (normalized == Vector3.zero)
			{
				base.transform.position = _unitView.transform.position + _offset;
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(normalized, Vector3.up);
			base.transform.position = _unitView.transform.position + quaternion * _offset;
		}
		else
		{
			base.transform.position = _offset + _unitView.GetBoneOffset(_boneType) + _unitView.transform.position;
		}
		if (_updateByAimDir)
		{
			Vector3 normalized2 = _unitModel.CalculatedDeltaAimPos.normalized;
			if (!(normalized2 == Vector3.zero))
			{
				base.transform.rotation = Quaternion.LookRotation(normalized2, Vector3.up);
			}
		}
	}

	private void Despawn()
	{
		_isInitialized = false;
		OnDespawned.OnNext(Unit.Default);
		base.gameObject.DespawnOrDestroy();
	}

	private void OnDisable()
	{
		_disposable?.Dispose();
		_disposable = null;
	}

	private void OnDestroy()
	{
		OnDespawned.Dispose();
	}
}
