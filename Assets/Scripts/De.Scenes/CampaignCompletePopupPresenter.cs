using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class CampaignCompletePopupPresenter : GamePopupPresenter
{
	[View]
	private CampaignCompletePopupView _view;

	[InjectModel]
	private CampaignCompletePopupModel _model;

	[Inject]
	private GameContext _gameContext;

	public override bool IsInputActionPopup => false;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override string OpenSfx => "CampaignSuccess";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		TCampaign tCampaign = TCampaign.Table[_model.CampaignId];
		_view.Title.text = tCampaign.Name;
		_view.CategoryText.text = tCampaign.GetChapterString();
		_view.BG.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		float userExpPercent = TDifficulty.Table[_gameContext.Difficulty].UserExpPercent;
		float userExpPercent2 = TVariantDifficulty.Table[tCampaign.VariantDifficulty].UserExpPercent;
		float num = ((_gameContext.Players.Count < 2) ? 100f : TConst.PartyPlayExpMultiplyPercent.FloatValue);
		float num2 = (float)tCampaign.GainedExp * (userExpPercent / 100f * userExpPercent2 / 100f * num / 100f);
		GoodsData[] rewards = tCampaign.GetRewards(_model.IsFirstClear);
		int num3 = rewards?.Length ?? 0;
		for (int i = 0; i < _view.Reward_Slots.Length; i++)
		{
			GameObject gameObject = _view.Reward_Slots[i];
			if (i < num3)
			{
				InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(rewards[i].ToGoodsAsBase());
				inventoryGoodsSlotModel.UseTooltip = true;
				inventoryGoodsSlotModel.ShowItemCount = true;
				gameObject.GetComponent<InventoryGoodsSlotPresenter>().Inject(inventoryGoodsSlotModel);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
		_view.Reward_ExpClear_Value.text = num2.ToString("+#;-#;0");
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickAsObservable().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		UiActions.Skip.OnClickAsObservable().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	private void OnAnimationEnd()
	{
		Close();
	}
}
