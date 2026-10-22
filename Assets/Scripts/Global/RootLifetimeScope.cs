using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
	[SerializeField]
	private AssetReferenceGameObject _unityNetworkManager;

	[SerializeField]
	private AssetReferenceGameObject _steamNetworkManager;

	[SerializeField]
	private AssetReferenceGameObject _popupManager;

	[SerializeField]
	private AssetReferenceGameObject _toastManager;

	[SerializeField]
	private AssetReferenceGameObject _soundManager;

	[SerializeField]
	private AssetReferenceGameObject _cursorManager;

	[SerializeField]
	private AssetReferenceGameObject _cameraManager;

	[SerializeField]
	private AssetReferenceGameObject _gameFloatingTextManager;

	[SerializeField]
	private AssetReferenceGameObject _graphicOptionController;

	[SerializeField]
	private AssetReferenceGameObject _blockManager;

	[SerializeField]
	private AssetReferenceGameObject _inputIconsManager;

	[SerializeField]
	private AssetReferenceT<EnvironmentSettings> _environmentSettings;

	[SerializeField]
	private AssetReferenceT<MiniMapSettings> _miniMapSettings;

	[SerializeField]
	private AssetReferenceT<LoungeMapSettings> _loungeMapSettings;

	[SerializeField]
	private AssetReferenceT<FontLocalizationSettings> _fontLocalizationSettings;

	[SerializeField]
	private AssetReferenceT<InputActionAsset> _inputActions;

	protected override void Configure(IContainerBuilder builder)
	{
	}

	private T LoadAsset<T>(AssetReferenceT<T> reference) where T : Object
	{
		return null;
	}

	private TComponent LoadPrefab<TComponent>(AssetReferenceGameObject reference) where TComponent : Component
	{
		return null;
	}
}
