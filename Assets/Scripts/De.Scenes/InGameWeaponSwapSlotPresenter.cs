using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class InGameWeaponSwapSlotPresenter : BaseSlotPresenter<InGameWeaponSwapSlotModel>
{
	[InjectModel]
	private InGameWeaponSwapSlotModel _model;

	[View]
	private InGameWeaponSwapSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	private int _lastActiveWeaponIndex;

	protected override InGameWeaponSwapSlotModel SlotModel => _model;

	protected override Button Button => _view.Slot;

	private GamePlayer Player => _gameManager.MyPlayer;

	private void OnInject(InGameWeaponSwapSlotModel model)
	{
		InitSlot(model);
		_gameManager.MyPlayer.RxCharacter.Subscribe(RefreshWeaponIcon).AddTo(Disposable);
	}

	private void RefreshWeaponIcon(Character character)
	{
		if (_model == null || character == null)
		{
			return;
		}
		GoodsSlot goodsSlot = (_model.IsActiveWeapon ? character.ActiveWeaponSlot : character.SubWeaponSlot);
		if (goodsSlot == null)
		{
			return;
		}
		if (goodsSlot.Goods == null || goodsSlot.Goods.Id < 1)
		{
			_lastActiveWeaponIndex = character.ActiveWeaponIndex;
			_view.Normal.gameObject.SetActive(false);
			return;
		}
		int id = goodsSlot.Goods.Id;
		_view.Normal.gameObject.SetActive(true);
		_view.Icon.sprite = _assetManager.GetIconEquipment(id);
		if (character.ActiveWeaponIndex != _lastActiveWeaponIndex)
		{
			_lastActiveWeaponIndex = character.ActiveWeaponIndex;
			_view.SwapedFX.SetActive(false);
			_view.SwapedFX.SetActive(true);
		}
		EGoodsGradeType grade = TEquipment.Table[id].Grade;
		Color color = grade.ToColor(_view.LevelBg.color.a);
		_view.LevelBg.gameObject.SetActive((int)grade > 1);
		_view.LevelBg.color = color;
	}
}
