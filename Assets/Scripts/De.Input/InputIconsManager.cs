using InputIcons;
using UnityEngine;

public class InputIconsManager : MonoBase
{
	[SerializeField]
	private InputIconsManagerSO _managerSo;

	[SerializeField]
	private InputIconSetConfiguratorSO _configuratorSo;

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void Initialize()
	{
		if (_managerSo == null || _configuratorSo == null)
		{
			Debug.LogError("[InputIconsManager] SO references are not assigned.");
			return;
		}
		InputIconSetConfiguratorSO.Instance = _configuratorSo;
		InputIconsManagerSO.Instance = _managerSo;
		_managerSo.Initialize();
	}
}
