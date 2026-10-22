using GameCommon.Enum;
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

	private bool _destroyOnUnitDeath;

	private bool _isInitialized;

	private CompositeDisposable _disposable;

	public Subject<Unit> OnDespawned { get; }

	private void Awake()
	{
	}

	public void Init(UnitModel unitModel, EUnitBoneType boneType, Vector3 offset, bool inheritMoveDir = false, bool destroyOnUnitDeath = false, bool updateByAimDir = false)
	{
	}

	private void Update()
	{
	}

	private void Despawn()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}
}
