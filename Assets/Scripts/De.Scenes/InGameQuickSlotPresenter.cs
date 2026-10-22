using DG.Tweening;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class InGameQuickSlotPresenter : BaseSlotPresenter<InGameQuickSlotModel>
{
	[InjectModel]
	private InGameQuickSlotModel _model;

	[View]
	private InGameQuickSlotView _view;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private Tweener _tweener;

	private GoodsSlotModel _goodsSlotModel;

	private CompositeDisposable _coolTimeDisposable;

	private int _itemId;

	protected override InGameQuickSlotModel SlotModel => null;

	protected override Button Button => null;

	private EConsumableType ConsumableType => EConsumableType.None;

	protected override void OnDestroy()
	{
	}

	private void OnInject(InGameQuickSlotModel model)
	{
	}

	private void BindCoolTime(CharacterModel characterModel)
	{
	}

	private void RefreshGoodsSlot()
	{
	}

	private void UpdateCountText()
	{
	}

	private void ApplyMaxStackColor()
	{
	}

	private void UpdateItemTypeCoolTime()
	{
	}

	private void UpdateCoolTimeText()
	{
	}

	private void OnCompleteCoolTimeTween()
	{
	}

	private float GetRemainCoolTime()
	{
		return 0f;
	}
}
