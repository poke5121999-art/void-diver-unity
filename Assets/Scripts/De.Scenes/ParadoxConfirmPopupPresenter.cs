using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ParadoxConfirmPopupPresenter : GamePopupPresenter
{
	[View]
	private ParadoxConfirmPopupView _view;

	[InjectModel]
	private ParadoxConfirmPopupModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private ToastManager _toastManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "InventoryPopupOpen";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		TParadox tParadox = TParadox.Table[_model.ParadoxId];
		bool isPositive = tParadox.IsPositive;
		bool hasEnough = _playerManager.HasEnoughGoods(_model.Cost);
		_view.Title_Text.text = _model.Text;
		_view.Title_SubText.gameObject.SetActive(!string.IsNullOrEmpty(_model.SubText));
		if (!string.IsNullOrEmpty(_model.SubText))
		{
			_view.Title_SubText.text = _model.SubText;
		}
		_view.Paradox_Name.text = tParadox.Name;
		_view.Paradox_Level.text = string.Format("Lv. {0}", tParadox.Level);
		_view.Desc.text = tParadox.Desc;
		_view.Paradox_Cost_Icon.sprite = _assetManager.GetIconGoods(_model.Cost.Type, _model.Cost.Id);
		_view.Paradox_Cost_Value.text = _model.Cost.Count.ToString();
		_view.Paradox_Cost_Value.color = UiUtility.HexToColor(hasEnough ? "#DCDCDC" : "#A45646");
		_view.Paradox_Cost_Caption.text = _model.CostCaptionText;
		Color color = UiUtility.HexToColor(isPositive ? "#58744C" : "#954E4E");
		_view.Paradox_Level.color = color;
		_view.Paradox_Icon.color = color;
		_view.Paradox_Icon.transform.localScale = (isPositive ? new Vector3(1f, -1f, 1f) : Vector3.one);
		_view.Cancel_Button.OnClickWithSfx().Subscribe(delegate
		{
			Close(false);
		}).AddTo(this);
		_view.Confirm_Button.OnClickWithSfx().Subscribe(delegate
		{
			Close(true);
		}).AddTo(this);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		SetNavHorizontal(new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button });
		SetDefaultNav(_view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close(false);
		}).AddTo(InputDisposable);
		UiKeyboardActions.Confirm.OnClickWithSfx().Subscribe(delegate
		{
			Close(true);
		}).AddTo(InputDisposable);
		UiKeyboardActions.Cancel.OnClickWithSfx().Subscribe(delegate
		{
			Close(false);
		}).AddTo(InputDisposable);
	}
}
