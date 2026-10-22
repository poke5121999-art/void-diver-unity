using System;
using System.Collections.Generic;
using UniRx;
using VContainer;

public class NewTagManager : NativeBase, IDisposable
{
	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameChatManager _chatManager;

	private readonly HashSet<int> _archiveItemIds;

	private readonly HashSet<int> _archiveEquipmentIds;

	private readonly HashSet<int> _archiveMonsterGroupIds;

	private bool _isInitialized;

	private CompositeDisposable _disposable;

	public void Initialize()
	{
	}

	void IDisposable.Dispose()
	{
	}

	private void OnItemCollected(int itemId)
	{
	}

	private void OnItemFirstUsed(int itemId)
	{
	}

	private void OnEquipmentCollected(int equipmentId)
	{
	}

	private void OnMonsterRecordUpdated(int monsterGroupId)
	{
	}

	public bool IsNewArchiveItem(int itemId)
	{
		return false;
	}

	public void ClearNewArchiveItem(int itemId)
	{
	}

	public bool IsNewArchiveEquipment(int equipmentId)
	{
		return false;
	}

	public void ClearNewArchiveEquipment(int equipmentId)
	{
	}

	public bool IsNewArchiveMonster(int monsterGroupId)
	{
		return false;
	}

	public void ClearNewArchiveMonster(int monsterGroupId)
	{
	}
}
