using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class ParadoxSlotPresenter : BaseSlotPresenter<ParadoxSlotModel>
{
	[View]
	private ParadoxSlotView _view;

	[InjectModel]
	private ParadoxSlotModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private GamePlayer MyPlayer => _gameManager.MyPlayer;

	protected override ParadoxSlotModel SlotModel => _model;

	private void OnInject(ParadoxSlotModel model)
	{
		InitSlot(model);
		TParadox tParadox = TParadox.Table[_model.ParadoxId];
		bool isPositive = tParadox.IsPositive;
		bool flag = _model.ParadoxId == MyPlayer.Character.LockedParadoxId;
		bool flag2 = _playerManager.HasEnoughGoods(_model.Cost);
		_view.Paradox_Name.text = tParadox.Name;
		_view.Paradox_Level.text = string.Format("Lv. {0}", tParadox.Level);
		_view.Desc.text = tParadox.Desc;
		_view.LockIcon.gameObject.SetActive(flag);
		_view.Cost.SetActive(_model.Cost.Count > 0);
		_view.Cost_Icon.sprite = _assetManager.GetIconGoods(_model.Cost.Type, _model.Cost.Id);
		_view.Cost_Value.text = _model.Cost.Count.ToNumberString();
		_view.Cost_Value.color = UiUtility.HexToColor(flag2 ? "#DCDCDC" : "#A45646");
		Color color = UiUtility.HexToColor(isPositive ? "#58744C" : "#954E4E");
		_view.Paradox_Level.color = color;
		_view.Paradox_Icon.color = color;
		_view.Paradox_Icon.transform.localScale = (isPositive ? new Vector3(1f, -1f, 1f) : Vector3.one);
		model.RxIsLocked.Value = flag;
	}
}
