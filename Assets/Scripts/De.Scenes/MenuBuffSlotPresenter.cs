using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class MenuBuffSlotPresenter : BaseSlotPresenter<MenuBuffSlotModel>
{
	[InjectModel]
	private MenuBuffSlotModel _model;

	[View]
	private MenuBuffSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	protected override MenuBuffSlotModel SlotModel => _model;

	protected override Button Button => _view.Normal;

	private UnitBuff UnitBuff => _model.UnitBuff;

	private void OnInject(MenuBuffSlotModel model)
	{
		InitSlot(model);
		TBuff tBuff = TBuff.Table[_model.UnitBuff.Id];
		_view.IconBuff.sprite = ((tBuff.EffectTag == EStatusEffectTag.None) ? _assetManager.GetIconBuff(_model.UnitBuff.Id) : _assetManager.GetIconStatusEffectTag(tBuff.EffectTag));
		_view.IconBuff_TextStack.text = ((tBuff.MaxStackCount >= 2) ? _model.UnitBuff.StackCount.ToString() : string.Empty);
		_view.TextDesc.text = tBuff.Name ?? string.Empty;
	}
}
