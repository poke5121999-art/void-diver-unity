using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class AreaDecorationObject : MonoBase
{
	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	private int _groupId;

	private string[] _prefabNames;

	private bool _isFlipped;

	private GameObject _currentChild;

	public void Init(int groupId, string[] prefabNames, bool isFlipped)
	{
		_groupId = groupId;
		_prefabNames = prefabNames;
		_isFlipped = isFlipped;
		_gameLoungeManager.OnAreaDecorationChanged
			.Where(((EAreaDecorationType areaDecorationType, int oldAreaDecorationSettingId, int newAreaDecorationSettingId) x) =>
				TAreaDecoration.Table.TryGetValue(x.newAreaDecorationSettingId, out var row) && row.GroupId == _groupId)
			.Subscribe(((EAreaDecorationType areaDecorationType, int oldAreaDecorationSettingId, int newAreaDecorationSettingId) x) =>
				Apply(x.newAreaDecorationSettingId))
			.AddTo(this);
		if (_gameContext.ActiveAreaDecorations != null && _gameContext.ActiveAreaDecorations.TryGetValue(_groupId, out var settingId))
		{
			Apply(settingId);
		}
	}

	private void Apply(int areaDecorationSettingId)
	{
		DespawnCurrent();
		TAreaDecoration row = TAreaDecoration.Table[areaDecorationSettingId];
		int index = row.Index;
		if (index < 0 || _prefabNames == null || index >= _prefabNames.Length)
		{
			return;
		}
		string text = _prefabNames[index];
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		_currentChild = _assetManager.InstantiateAreaDecorationObject(text);
		if (_currentChild == null)
		{
			_logger.Error("AreaDecorationObject prefab not found: " + text);
			return;
		}
		_currentChild.transform.SetParent(base.transform, false);
		_currentChild.transform.localPosition = Vector3.zero;
		if (_isFlipped)
		{
			RuntimeFlipUtility.ApplyFlip(_currentChild);
		}
	}

	private void DespawnCurrent()
	{
		if (_currentChild != null)
		{
			Object.Destroy(_currentChild);
			_currentChild = null;
		}
	}
}
