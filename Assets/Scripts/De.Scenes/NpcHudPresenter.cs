using System.Collections.Generic;
using System.Linq;
using Febucci.UI;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

public class NpcHudPresenter : BaseUnitHudPresenter
{
	[SerializeField]
	private NpcObject _npcObject;

	[View]
	private NpcHudView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _gameCampaignManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameNpcDialogManager _npcDialogManager;

	protected override GameObject Bubble => _view.Bubble;

	protected override TypewriterByCharacter BubbleTextWriter => _view.Bubble_Text.GetComponent<TypewriterByCharacter>();

	protected override TextMeshProUGUI Monologue => _view.Monologue;

	private void Start()
	{
		TNpc tNpc = TNpc.Table[_npcObject.NpcId];
		_view.Name.text = tNpc.Name;
		_view.SubName.text = tNpc.SubName;
		_npcObject.OnShowBubbleText.Subscribe(ShowBubbleText).AddTo(this);
		_npcObject.OnShowMonologueText.Subscribe(ShowMonologueText).AddTo(this);
		InitDialogMark();
		InitFunctionHud();
	}

	private void InitDialogMark()
	{
		UpdateDialogMarks();
		_npcDialogManager.OnDialogChanged.Where((int _) => _ == _npcObject.NpcId).Subscribe(delegate
		{
			UpdateDialogMarks();
		}).AddTo(this);
	}

	private void UpdateDialogMarks()
	{
		_view.CampaignStartMark.SetActive(value: false);
		_view.MissionStartMark.SetActive(value: false);
		_view.LoungeStartMark.SetActive(value: false);
		_view.CompleteMark.SetActive(value: false);
		List<DialogInfo> list = _npcDialogManager.GetDialogInfos(_npcObject.NpcId).Where((DialogInfo _) => _.Purpose != EDialogPurpose.None).Where((DialogInfo _) => _.Type != EDialogType.Talk).ToList();
		if (!list.Any((DialogInfo _) => _.Purpose == EDialogPurpose.Complete))
		{
			DialogInfo dialogInfo = list.Where((DialogInfo _) => _.Purpose == EDialogPurpose.Start).OrderBy((DialogInfo _) => _.Priority).FirstOrDefault();
			if (dialogInfo == null)
			{
				return;
			}
			switch (dialogInfo.Type)
			{
			case EDialogType.Quest:
				_view.CampaignStartMark.SetActive(value: true);
				break;
			case EDialogType.Mission:
				_view.MissionStartMark.SetActive(value: true);
				break;
			case EDialogType.Lounge:
				_view.LoungeStartMark.SetActive(value: true);
				break;
			}
		}
		else
		{
			_view.CompleteMark.SetActive(value: true);
		}
	}

	private void InitFunctionHud()
	{
		if (HasNpcFunction(ENpcFunctionType.Campaign))
		{
			_gameCampaignManager.OnHostWaitOtherPlayerCampaignCompleted.Subscribe(OnCampaignManagerWaitOtherPlayer).AddTo(this);
		}
		if (HasNpcFunction(ENpcFunctionType.Storage))
		{
			UpdateStorageUis();
			_playerManager.OnStorageChanged.Subscribe(delegate
			{
				UpdateStorageUis();
			}).AddTo(this);
		}
	}

	private bool HasNpcFunction(ENpcFunctionType npcFunctionType)
	{
		if (_npcObject.NpcFunctionIds == null)
		{
			return false;
		}
		return _npcObject.NpcFunctionIds.Where((int _) => _gameManager.IsHost || TNpcFunction.Table[_].GuestInteractable).Any((int _) => TNpcFunction.Table[_].Type == npcFunctionType);
	}

	private void UpdateStorageUis()
	{
		bool flag = _playerManager.HasStorageSlots();
		_view.Info.SetActive(flag);
		if (flag)
		{
			_view.Info_Text.text = TLocalizedText.AvailableRewardFormat.Format(_playerManager.GetStorageSlotCount());
		}
	}

	private void OnCampaignManagerWaitOtherPlayer(bool isWait)
	{
		_view.QuestHostWaitOtherClientMark.SetActive(isWait);
	}

	private void OnStorageChanged()
	{
		UpdateStorageUis();
	}
}
