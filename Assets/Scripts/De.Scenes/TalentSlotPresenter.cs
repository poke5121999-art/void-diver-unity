using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class TalentSlotPresenter : BaseSlotPresenter<TalentSlotModel>
{
	[InjectModel]
	private TalentSlotModel _model;

	[View]
	private TalentSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	protected override TalentSlotModel SlotModel => _model;

	protected override Button Button => _view.Button;

	private void OnInject(TalentSlotModel model)
	{
		InitSlot(model);
		_view.Icon.SetSprite(_assetManager.GetIconTalent(model.TalentId));
		_playerManager.RxProfile.Subscribe(delegate
		{
			OnUpdateUis();
		}).AddTo(base.Disposable);
		_playerManager.RxTalent.Subscribe(delegate
		{
			OnUpdateUis();
		}).AddTo(base.Disposable);
		_playerManager.OnOwnedGoodsChanged.Subscribe(delegate
		{
			OnUpdateUis();
		}).AddTo(base.Disposable);
	}

	private void OnUpdateUis()
	{
		TTalent tTalent = TTalent.Table[_model.TalentId];
		EventCondition[] unlockConditions = tTalent.GetUnlockConditions();
		EUnlockState talentState = _playerManager.GetTalentState(_model.TalentId);
		bool flag = unlockConditions.FirstOrDefault((EventCondition c) => !_playerManager.CheckStateCondition(c)).Type == EEventConditionType.None;
		_view.Active.gameObject.SetActive(talentState == EUnlockState.Unlocked);
		_view.Locked.gameObject.SetActive(talentState == EUnlockState.Locked);
		bool flag2 = _playerManager.HasEnoughGoods(tTalent.MaterialGoodsDatas);
		_view.UnlockableTag.SetActive(talentState != EUnlockState.Unlocked && flag2 && flag);
		string colorCode = ((talentState == EUnlockState.Unlocked) ? "#A45646" : "#6C6D6D");
		_view.Icon.color = GameCommon.Utility.ColorUtility.ParseOrWhite(colorCode);
	}
}
