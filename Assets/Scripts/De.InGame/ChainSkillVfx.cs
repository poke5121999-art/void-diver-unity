using NemoLib.AssetManagement;
using UnityEngine;

public class ChainSkillVfx : SkillVfx
{
	[SerializeField]
	private float _chainLength;

	[SerializeField]
	private Transform _chainTransform;

	[SerializeField]
	private Transform _playerChainTransform;

	[SerializeField]
	private float _chainOffset;

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
	}

	private void Start()
	{
	}

	private void CacheInitialTransform()
	{
	}

	protected override void FixedUpdate()
	{
	}

	private void OnDespawned(PoolableObject poolable)
	{
	}

	private void OnDestroy()
	{
	}
}
