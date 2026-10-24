using System;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class InventoryGoodsSlotPresenter : BaseSlotPresenter<InventoryGoodsSlotModel>
{
	public enum ELayout
	{
		Normal = 0,
		Locked = 1,
		Unrevealed = 2,
		Revealing = 3
	}

	[SerializeField]
	private Color _outlineBaseColor;

	[SerializeField]
	private Color _outlineSelectedColor;

	[SerializeField]
	private float _dragThreshold = 10f;

	[InjectModel]
	private InventoryGoodsSlotModel _model;

	[View]
	private InventoryGoodsSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameInputManager _inputManager;

	private bool _isFocused;

	private Vector2 _pointerDownPos;

	private bool _isDragging;

	private PlaySoundResult _revealLoopSfxResult;

	private bool _isDisabled;

	private ELayout _prevLayout;

	protected override InventoryGoodsSlotModel SlotModel => _model;

	protected override Button Button => _view.Slot;

	private GamePlayer Player => _gameManager.MyPlayer;

	private InputActions.UiActions UiActions => _inputManager.UiActions;

	private Animator RevealingAnimator => _view.Revealing.GetComponent<Animator>();

	protected override void Start()
	{
		base.Start();
		_view.Slot.OnPointerDownAsObservable().Subscribe(OnSlotDown).AddTo(this);
		_view.Slot.OnPointerUpAsObservable().Subscribe(OnSlotUp).AddTo(this);
		_view.Slot.OnDragAsObservable().Subscribe(OnSlotDrag).AddTo(this);
	}

	private void OnEnable()
	{
		if (_isDisabled)
		{
			_isDisabled = false;
			if (_model != null)
			{
				UpdateLayout(_model.RxLayout.Value);
			}
		}
	}

	private void OnDisable()
	{
		if (_isFocused)
		{
			OnSlotExit();
		}
		_revealLoopSfxResult?.StopSound();
		_revealLoopSfxResult = null;
		_isDisabled = true;
		_view.Revealing_Normal.gameObject.SetActive(false);
		_view.Unrevealed.SetActive(false);
	}

	protected void OnInject(InventoryGoodsSlotModel slotModel)
	{
		_revealLoopSfxResult?.StopSound();
		_revealLoopSfxResult = null;
		InitSlot(slotModel);
		_view.Revealing_Normal.gameObject.SetActive(false);
		_view.Unrevealed.SetActive(false);
		_prevLayout = ELayout.Normal;
		slotModel.Transform = base.gameObject.transform;
		slotModel.RxLayout.Subscribe(UpdateLayout).AddTo(Disposable);
		slotModel.RxGoodsSlot.Skip(1).Subscribe(OnGoodsSlotChanged).AddTo(Disposable);
		_playerManager.OnGoodsMarkChanged.Subscribe(OnGoodsMarkChanged).AddTo(this);
		UiActions.MarkGoods.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnMarkGoodsClick();
		}).AddTo(Disposable);
		slotModel.RxHighlightOn.Subscribe(delegate(bool _)
		{
			_view.Highlight.SetActive(_);
		}).AddTo(Disposable);
	}

	private void UpdateLayout(ELayout layout)
	{
		if (_isDisabled)
		{
			return;
		}
		ELayout prevLayout = _prevLayout;
		_prevLayout = layout;
		bool flag = layout == ELayout.Normal && _model.RxGoodsSlot.Value.IsEmpty;
		_view.WeaponIcon.SetActive(flag && _model.SlotCategory == EGoodsSlotCategory.WeaponSlot);
		_view.AccessoryIcon.SetActive(flag && _model.SlotCategory == EGoodsSlotCategory.AccessorySlot);
		_view.ArtifactIcon.SetActive(flag && _model.SlotCategory == EGoodsSlotCategory.ArtifactSlot);
		if (flag)
		{
			ButtonController.SetLocked(false);
			ButtonController.SetSelected(false);
			_view.Normal.gameObject.SetActive(false);
			return;
		}
		if (_model.RxLayout.Value == ELayout.Normal && IsPointerEntered())
		{
			OnSlotEnter();
		}
		else
		{
			OnSlotExit();
		}
		_view.LevelBg.gameObject.SetActive(false);
		_view.Artifact.gameObject.SetActive(false);
		_view.Icon.gameObject.SetActive(false);
		_view.SubIcon.gameObject.SetActive(false);
		_view.Count.gameObject.SetActive(false);
		_view.ItemMark.gameObject.SetActive(false);
		_view.QuestMark.SetActive(false);
		_view.MissionMark.SetActive(false);
		if (layout == ELayout.Normal)
		{
			SetNormalLayout();
		}
		_view.Normal.gameObject.SetActive(layout < ELayout.Unrevealed);
		_view.Slot.enabled = layout == ELayout.Normal;
		ButtonController.SetLocked(layout == ELayout.Locked);
		if (layout == ELayout.Revealing)
		{
			if (prevLayout != ELayout.Revealing)
			{
				_view.Revealing_Normal.gameObject.SetActive(true);
				RevealingAnimator.SetTrigger("StartTrigger");
				_revealLoopSfxResult = _soundManager.PlaySfx("Looting_Loop");
			}
		}
		else if (prevLayout == ELayout.Revealing)
		{
			RevealingAnimator.SetTrigger("EndTrigger");
		}
		_view.Unrevealed.SetActive(layout == ELayout.Unrevealed);
	}

	private void SetNormalLayout()
	{
		_view.Normal.gameObject.SetActive(true);
		if (_model.RxGoodsSlot.Value == null || _model.RxGoodsSlot.Value.Goods == null)
		{
			_view.Icon.gameObject.SetActive(false);
			_view.ItemMark.gameObject.SetActive(false);
			_view.SubIcon.gameObject.SetActive(false);
			_view.Count.gameObject.SetActive(false);
			_view.LevelBg.gameObject.SetActive(false);
			return;
		}
		_view.Icon.gameObject.SetActive(true);
		_view.Icon.sprite = _assetManager.GetIconGoods(_model.RxGoodsSlot.Value.Type, _model.RxGoodsSlot.Value.Id);
		_view.ItemMark.gameObject.SetActive(_playerManager.IsMarked(_model.RxGoodsSlot.Value.Goods));
		EGoodsType type = _model.RxGoodsSlot.Value.Type;
		_view.SubIcon.gameObject.SetActive(type == EGoodsType.Blueprint);
		if (type == EGoodsType.Blueprint)
		{
			GoodsData resultGoodsDataByUsedItem = TCrafting.GetResultGoodsDataByUsedItem(_model.GoodsId);
			_view.SubIcon.sprite = _assetManager.GetIconGoods(resultGoodsDataByUsedItem.Type, resultGoodsDataByUsedItem.Id);
		}
		switch (_model.RxGoodsSlot.Value.Category)
		{
		case EGoodsCategory.Currency:
			_view.Count.gameObject.SetActive(_model.ShowItemCount);
			_view.Count_Text.text = string.Format("{0}", _model.GoodsSlot.Count);
			_view.LevelBg.gameObject.SetActive(false);
			break;
		case EGoodsCategory.Item:
			_view.Count.gameObject.SetActive(_model.ShowItemCount);
			SetItemCountText();
			SetItemFrameColor();
			SetItemTaskIcons();
			break;
		case EGoodsCategory.Equipment:
			_view.Count.gameObject.SetActive(false);
			SetEquipmentFrameColor();
			break;
		case EGoodsCategory.Bag:
			_view.Count.gameObject.SetActive(false);
			SetBagFrameColor();
			break;
		}

		void SetItemFrameColor()
		{
			TItem tItem = TItem.Table[_model.RxGoodsSlot.Value?.Id ?? 0];
			EGoodsGradeType grade = tItem.Grade;
			Color color = grade.ToColor(_view.LevelBg.color.a);
			_view.LevelBg.gameObject.SetActive(grade > EGoodsGradeType.Normal);
			_view.LevelBg.color = color;
		}

		void SetItemTaskIcons()
		{
			TItem tItem2 = TItem.Table[_model.RxGoodsSlot.Value?.Id ?? 0];
			_view.QuestMark.SetActive(tItem2.Category == EItemCategory.Quest);
			_view.MissionMark.SetActive(tItem2.Category == EItemCategory.Mission);
		}

		void SetItemCountText()
		{
			bool flag = false;
			if (_model.CheckMaxItemCount && (_model.SlotCategory == EGoodsSlotCategory.InventorySlot || _model.SlotCategory == EGoodsSlotCategory.SafeSlot || _model.SlotCategory == EGoodsSlotCategory.BagSlot))
			{
				long count = _model.RxGoodsSlot.Value.Count;
				flag = Player.GetInventoryItemCountMax(_model.RxGoodsSlot.Value.Id) <= count;
			}
			string text = string.Format("{0}", _model.RxGoodsSlot.Value.Count);
			_view.Count_Text.text = (flag ? text.WithColor("#a74200") : text);
		}

		void SetEquipmentFrameColor()
		{
			TEquipment tEquipment = TEquipment.Table[_model.RxGoodsSlot.Value?.Id ?? 0];
			EGoodsGradeType grade2 = tEquipment.Grade;
			Color color2 = grade2.ToColor(_view.LevelBg.color.a);
			_view.LevelBg.gameObject.SetActive(grade2 > EGoodsGradeType.Normal);
			_view.LevelBg.color = color2;
			_view.Artifact.gameObject.SetActive(tEquipment.IsArtifact());
			if (tEquipment.IsArtifact())
			{
				GameObject[] artifact_Group = _view.Artifact_Group;
				foreach (GameObject gameObject in artifact_Group)
				{
					Graphic component = gameObject.GetComponent<Graphic>();
					Color color3 = new Color(color2.r, color2.g, color2.b, component.color.a);
					component.color = color3;
				}
			}
		}

		void SetBagFrameColor()
		{
			TBag tBag = TBag.Table[_model.RxGoodsSlot.Value?.Id ?? 0];
			EGoodsGradeType grade3 = tBag.Grade;
			Color color4 = grade3.ToColor(_view.LevelBg.color.a);
			_view.LevelBg.gameObject.SetActive(grade3 > EGoodsGradeType.Normal);
			_view.LevelBg.color = color4;
		}
	}

	private void OnGoodsSlotChanged(GoodsSlot goodsSlot)
	{
		bool flag = goodsSlot != null && goodsSlot.IsRevealed;
		if (_model.RxLayout.Value == ELayout.Revealing && flag)
		{
			_revealLoopSfxResult?.StopSound();
			_revealLoopSfxResult = null;
			_soundManager.PlaySfx(goodsSlot.Grade.GetRevealSfx());
			_model.RxLayout.Value = ELayout.Normal;
		}
		if (_model.RxLayout.Value == ELayout.Normal)
		{
			UpdateLayout(_model.RxLayout.Value);
		}
	}

	private void OnGoodsMarkChanged((EGoodsType goodsType, int id, bool isMarked) data)
	{
		if (_model.RxLayout.Value == ELayout.Normal && (_model.RxGoodsSlot.Value?.Type ?? EGoodsType.None) == data.goodsType && (_model.RxGoodsSlot.Value?.Id ?? 0) == data.id)
		{
			_view.ItemMark.gameObject.SetActive(_playerManager.IsMarked(_model.RxGoodsSlot.Value.Goods));
		}
	}

	private void OnMarkGoodsClick()
	{
		if (_isFocused && _model.RxLayout.Value == ELayout.Normal && !_model.RxGoodsSlot.Value.IsEmpty)
		{
			_playerManager.ToggleMarkGoods(_model.RxGoodsSlot.Value.Goods);
		}
	}

	private void OnSlotDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && _model.RxLayout.Value == ELayout.Normal)
		{
			_pointerDownPos = eventData.position;
			_isDragging = false;
		}
	}

	private void OnSlotDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && _model.RxLayout.Value == ELayout.Normal && !_isDragging && Vector2.Distance(eventData.position, _pointerDownPos) >= _dragThreshold)
		{
			_isDragging = true;
			_model.OnDrag.OnNext(_model);
		}
	}

	private void OnSlotUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && _model.RxLayout.Value == ELayout.Normal && _isDragging)
		{
			_model.OnEndDrag.OnNext(_model);
		}
	}

	protected override void OnSlotEnter()
	{
		if (_model != null && _model.RxLayout.Value == ELayout.Normal)
		{
			_isFocused = true;
			base.OnSlotEnter();
		}
	}

	protected override void OnSlotExit()
	{
		if (_isFocused)
		{
			_isFocused = false;
			base.OnSlotExit();
		}
	}

	protected override void OnLeftClick()
	{
		if (!_isDragging && _model != null && _model.RxLayout.Value == ELayout.Normal)
		{
			TryShowGoodsTooltipPopup();
			base.OnLeftClick();
		}
	}

	protected override void OnRightClick()
	{
		if (_model != null && _model.RxLayout.Value == ELayout.Normal)
		{
			TryShowGoodsTooltipPopup();
			base.OnRightClick();
		}
	}

	private bool TryShowGoodsTooltipPopup()
	{
		if (!_model.UseTooltip)
		{
			return false;
		}
		if (_model.RxGoodsSlot.Value == null)
		{
			return false;
		}
		if (_model.RxGoodsSlot.Value.IsEmpty)
		{
			return false;
		}
		if (_model.IsUnknown)
		{
			_toastManager.NoticeFail(TLocalizedText.WarningUnknownGoods.LocalText);
			return false;
		}
		_popupManager.Open<GoodsTooltipPopupPresenter>(new GoodsTooltipPopupModel
		{
			Goods = _model.GoodsSlot.Goods,
			IsCrafting = _model.IsCrafting,
			OnCraftingNavigate = _model.OnCraftingNavigate
		});
		return true;
	}

	private bool IsPointerEntered()
	{
		if (_inputManager.GetSelectedObject() == _view.Slot.gameObject)
		{
			return true;
		}
		if (EventSystem.current == null)
		{
			return false;
		}
		PointerEventData eventData = new PointerEventData(EventSystem.current)
		{
			position = Mouse.current.position.ReadValue()
		};
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, list);
		return list.Any((RaycastResult _) => _.gameObject == _view.BackLine.gameObject);
	}
}
