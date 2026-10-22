using Febucci.UI;
using GameCommon.Enum;
using NemoLib.Mvp;
using TMPro;
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

	protected override GameObject Bubble => null;

	protected override TypewriterByCharacter BubbleTextWriter => null;

	protected override TextMeshProUGUI Monologue => null;

	private void Start()
	{
	}

	private void InitDialogMark()
	{
	}

	private void UpdateDialogMarks()
	{
	}

	private void InitFunctionHud()
	{
	}

	private bool HasNpcFunction(ENpcFunctionType npcFunctionType)
	{
		return false;
	}

	private void UpdateStorageUis()
	{
	}

	private void OnCampaignManagerWaitOtherPlayer(bool isWait)
	{
	}

	private void OnStorageChanged()
	{
	}
}
