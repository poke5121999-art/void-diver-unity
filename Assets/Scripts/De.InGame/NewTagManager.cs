using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using VContainer;

public class NewTagManager : NativeBase, IDisposable
{
	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameChatManager _chatManager;

	private readonly HashSet<int> _archiveItemIds = new HashSet<int>();

	private readonly HashSet<int> _archiveEquipmentIds = new HashSet<int>();

	private readonly HashSet<int> _archiveMonsterGroupIds = new HashSet<int>();

	private bool _isInitialized;

	private CompositeDisposable _disposable;

	public void Initialize()
	{
		if (!_isInitialized)
		{
			_disposable = new CompositeDisposable();
			_playerManager.OnItemCollected.Subscribe(OnItemCollected).AddTo(_disposable);
			_playerManager.OnItemFirstUsed.Subscribe(OnItemFirstUsed).AddTo(_disposable);
			_playerManager.OnEquipmentCollected.Subscribe(OnEquipmentCollected).AddTo(_disposable);
			_playerManager.OnMonsterRecordUpdated.Subscribe(OnMonsterRecordUpdated).AddTo(_disposable);
		}
	}

	void IDisposable.Dispose()
	{
		if (_disposable != null)
		{
			_disposable.Dispose();
		}
	}

	private void AddArchiveRecordChat(string name)
	{
		_chatManager.AddChat(new ChatInfo
		{
			Type = EChatType.System,
			Text = TLocalizedText.ArchiveRecordComplete.Format(name),
			UtcTime = DateTime.UtcNow
		});
	}

	private void OnItemCollected(int itemId)
	{
		TItem tItem = TItem.Table[itemId];
		EGoodsType goodsType = tItem.GoodsType;
		if (goodsType != EGoodsType.Note && goodsType != EGoodsType.Blueprint && goodsType != EGoodsType.MusicDisc && _archiveItemIds.Add(itemId))
		{
			AddArchiveRecordChat(tItem.Name);
		}
	}

	private void OnItemFirstUsed(int itemId)
	{
		TItem tItem = TItem.Table[itemId];
		EGoodsType goodsType = tItem.GoodsType;
		if ((goodsType == EGoodsType.Note || goodsType == EGoodsType.Blueprint || goodsType == EGoodsType.MusicDisc) && _archiveItemIds.Add(itemId))
		{
			AddArchiveRecordChat(tItem.Name);
		}
	}

	private void OnEquipmentCollected(int equipmentId)
	{
		if (_archiveEquipmentIds.Add(equipmentId))
		{
			TEquipment tEquipment = TEquipment.Table[equipmentId];
			AddArchiveRecordChat(tEquipment.Name);
		}
	}

	private void OnMonsterRecordUpdated(int monsterGroupId)
	{
		if (_archiveMonsterGroupIds.Add(monsterGroupId))
		{
			TMonster tMonster = TMonster.GroupTable[monsterGroupId].First();
			AddArchiveRecordChat(tMonster.Name);
		}
	}

	public bool IsNewArchiveItem(int itemId)
	{
		return _archiveItemIds.Contains(itemId);
	}

	public void ClearNewArchiveItem(int itemId)
	{
		_archiveItemIds.Remove(itemId);
	}

	public bool IsNewArchiveEquipment(int equipmentId)
	{
		return _archiveEquipmentIds.Contains(equipmentId);
	}

	public void ClearNewArchiveEquipment(int equipmentId)
	{
		_archiveEquipmentIds.Remove(equipmentId);
	}

	public bool IsNewArchiveMonster(int monsterGroupId)
	{
		return _archiveMonsterGroupIds.Contains(monsterGroupId);
	}

	public void ClearNewArchiveMonster(int monsterGroupId)
	{
		_archiveMonsterGroupIds.Remove(monsterGroupId);
	}
}
