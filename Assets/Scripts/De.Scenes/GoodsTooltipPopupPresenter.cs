using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class GoodsTooltipPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private GoodsTooltipPopupModel _model;

	[View]
	private GoodsTooltipPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.GoodsTooltip.Inject(new GoodsTooltipModel
		{
			Goods = _model.Goods,
			ShowKeyPrompt = true,
			IsCraftingAvailable = (_model.IsCrafting && HasUnlockedCraftingRecipe())
		});
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		UiActions.MarkGoods.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnMarkGoodsClick();
		}).AddTo(InputDisposable);
		if (_model.IsCrafting && HasUnlockedCraftingRecipe())
		{
			UiActions.TooltipMoveToCrafting.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnCraftingNavigateInput();
			}).AddTo(InputDisposable);
		}
	}

	private void OnMarkGoodsClick()
	{
		_playerManager.ToggleMarkGoods(_model.Goods);
	}

	private bool HasUnlockedCraftingRecipe()
	{
		return TCrafting.Table.Values.Where((TCrafting _) =>
		{
			if (_ == null || _playerManager == null)
			{
				return false;
			}
			if (_.UnlockConditions == null || _.UnlockConditions.Length == 0)
			{
				return true;
			}
			return _.UnlockConditions.All(_playerManager.CheckStateCondition);
		}).Any((TCrafting _) => _.ResultGoodsData.Type == _model.Goods.Type && _.ResultGoodsData.Id == _model.Goods.Id);
	}

	private void OnCraftingNavigateInput()
	{
		if (_model.OnCraftingNavigate != null)
		{
			GameCommon.Data.Goods goods = _model.Goods;
			Close();
			_model.OnCraftingNavigate.OnNext(goods);
		}
	}
}
