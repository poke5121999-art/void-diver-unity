using GameCommon.Enum;
using UnityEngine;
using VContainer;

public class PingObject : MonoBase
{
	[SerializeField]
	private EPingType _pingType;

	[Inject]
	private GameObjectManager _objectManager;

	public EPingType PingType => EPingType.None;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}
}
