using DG.Tweening;
using NemoLib.AssetManagement;
using UniRx;
using UnityEngine;

public class ChainSkillVfx : SkillVfx
{
	[SerializeField]
	private float _chainLength = 10f;

	[SerializeField]
	private Transform _chainTransform;

	[SerializeField]
	private Transform _playerChainTransform;

	[SerializeField]
	private float _chainOffset = 0.5f;

	private float _elapsedTime;

	private bool _isCollisionFinished;

	private Vector3 _chainInitialLocalPosition;

	private Quaternion _chainInitialLocalRotation;

	private Vector3 _chainInitialLocalScale;

	private bool _chainInitialActive;

	private Vector3 _playerChainInitialLocalPosition;

	private Quaternion _playerChainInitialLocalRotation;

	private Vector3 _playerChainInitialLocalScale;

	private bool _playerChainInitialActive;

	protected override void Awake()
	{
		base.Awake();
		CacheInitialTransform();
	}

	private void Start()
	{
		PoolableObject component = GetComponent<PoolableObject>();
		if (component != null)
		{
			component.OnDespawned.Subscribe(OnDespawned).AddTo(this);
		}
	}

	private void CacheInitialTransform()
	{
		if (_chainTransform != null)
		{
			_chainInitialLocalPosition = _chainTransform.localPosition;
			_chainInitialLocalRotation = _chainTransform.localRotation;
			_chainInitialLocalScale = _chainTransform.localScale;
			_chainInitialActive = _chainTransform.gameObject.activeSelf;
		}
		if (_playerChainTransform != null)
		{
			_playerChainInitialLocalPosition = _playerChainTransform.localPosition;
			_playerChainInitialLocalRotation = _playerChainTransform.localRotation;
			_playerChainInitialLocalScale = _playerChainTransform.localScale;
			_playerChainInitialActive = _playerChainTransform.gameObject.activeSelf;
		}
	}

	protected override void FixedUpdate()
	{
		if (OwnerGo == null && HitBox == null)
		{
			return;
		}
		if (HitBox != null)
		{
			_elapsedTime += Time.fixedDeltaTime;
			if (_elapsedTime > HitBox.HitBoxInfo.collisionEndTime && HitBox.HitBoxInfo.collisionEndTime > 0f)
			{
				if (_isCollisionFinished)
				{
					return;
				}
				_isCollisionFinished = true;
				_chainTransform.gameObject.SetActive(value: false);
				_playerChainTransform.gameObject.SetActive(value: true);
				_playerChainTransform.parent = null;
				Vector3 normalized = (OwnerGo.transform.position - base.transform.position).normalized;
				_playerChainTransform.position = new Vector3(normalized.x * _chainOffset + OwnerGo.transform.position.x, _playerChainTransform.position.y, normalized.z * _chainOffset + OwnerGo.transform.position.z);
				_playerChainTransform.rotation = _chainTransform.rotation;
				_playerChainTransform.localScale = _chainTransform.localScale;
				_playerChainTransform.DOScaleZ(0f, HitBox.HitBoxInfo.duration - HitBox.HitBoxInfo.collisionEndTime);
				return;
			}
		}
		Vector3 vector = OwnerGo.transform.position - base.transform.position;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		if (_chainLength <= 0f)
		{
			Debug.LogError("Chain length must be greater than 0");
			return;
		}
		if (vector != Vector3.zero)
		{
			_chainTransform.rotation = Quaternion.LookRotation(-vector.normalized, Vector3.up);
		}
		_chainTransform.localScale = new Vector3(1f, 1f, Mathf.Max((magnitude + _chainOffset) / _chainLength, 0f));
	}

	private void OnDespawned(PoolableObject poolable)
	{
		_elapsedTime = 0f;
		_isCollisionFinished = false;
		if (_playerChainTransform != null)
		{
			_playerChainTransform.DOKill();
			_playerChainTransform.SetParent(base.transform, worldPositionStays: false);
			_playerChainTransform.localPosition = _playerChainInitialLocalPosition;
			_playerChainTransform.localRotation = _playerChainInitialLocalRotation;
			_playerChainTransform.localScale = _playerChainInitialLocalScale;
			_playerChainTransform.gameObject.SetActive(_playerChainInitialActive);
		}
		if (_chainTransform != null)
		{
			_chainTransform.localPosition = _chainInitialLocalPosition;
			_chainTransform.localRotation = _chainInitialLocalRotation;
			_chainTransform.localScale = _chainInitialLocalScale;
			_chainTransform.gameObject.SetActive(_chainInitialActive);
		}
	}

	private void OnDestroy()
	{
		if (_playerChainTransform != null && _playerChainTransform.parent != base.transform)
		{
			Destroy(_playerChainTransform.gameObject);
		}
	}
}
