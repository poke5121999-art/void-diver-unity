using System.Collections.Generic;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class StageResultPlayerItemGroupPresenter : Presenter
{
	[View]
	private StageResultPlayerItemGroupView _view;

	[InjectModel]
	private StageResultPlayerItemGroupModel _model;

	[Inject]
	private AssetManager _assetManager;

	private readonly List<GoodsSlotPresenter> _cachedSlots = new List<GoodsSlotPresenter>();

	protected void OnInject(StageResultPlayerItemGroupModel model)
	{
		if (_view != null && _view.UserName != null)
		{
			_view.UserName.text = model.UserName;
		}
		GameCommon.Data.Goods[] items = model.Items;
		if (items == null || items.Length == 0)
		{
			foreach (GoodsSlotPresenter cachedSlot in _cachedSlots)
			{
				if (cachedSlot != null)
				{
					cachedSlot.gameObject.SetActive(false);
				}
			}
			return;
		}
		for (int count = _cachedSlots.Count; count < items.Length; count++)
		{
			GameObject gameObject = _assetManager.InstantiateGoodsSlot();
			if (gameObject == null)
			{
				break;
			}
			gameObject.transform.SetParent(base.transform, false);
			GoodsSlotPresenter component = gameObject.GetComponent<GoodsSlotPresenter>();
			if (component != null)
			{
				_cachedSlots.Add(component);
			}
		}
		for (int i = 0; i < items.Length; i++)
		{
			GoodsSlotPresenter goodsSlotPresenter = _cachedSlots[i];
			if (goodsSlotPresenter != null)
			{
				if (!goodsSlotPresenter.gameObject.activeSelf)
				{
					goodsSlotPresenter.gameObject.SetActive(true);
				}
				goodsSlotPresenter.transform.SetSiblingIndex(i);
				GoodsSlotModel goodsSlotModel = new GoodsSlotModel(items[i]);
				goodsSlotModel.UseTooltip = true;
				goodsSlotModel.ShowItemCount = true;
				goodsSlotPresenter.Inject(goodsSlotModel);
			}
		}
		for (int j = items.Length; j < _cachedSlots.Count; j++)
		{
			GoodsSlotPresenter goodsSlotPresenter2 = _cachedSlots[j];
			if (goodsSlotPresenter2 != null && goodsSlotPresenter2.gameObject.activeSelf)
			{
				goodsSlotPresenter2.gameObject.SetActive(false);
			}
		}
	}
}
