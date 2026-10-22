using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ArchiveGlossaryPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private ArchiveGlossaryPopupModel _model;

	[View]
	private ArchiveGlossaryPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	private TGlossary[] _glossaryDatas;

	private TextSlotScrollerModel _scrollerModel;

	private TextSlotModel _selectedSlot;

	private TextSlotModel _focusedSlot;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	public override string OpenSfx => "PopUpOpen";

	private void Start()
	{
		_view.Glossary_Scroll.decelerationRate = Const.Scroller.DECELERATION_RATE;
		_view.Glossary_Scroll.scrollSensitivity = Const.Scroller.SCROLL_SENSITIVITY;
	}

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Title.text = EArchiveCategory.Glossary.ToLocalText();
		_view.TitleSub.text = _model.FilterCategory.ToLocalText();
		_view.Empty.SetActive(true);
		_view.Glossary.SetActive(false);
		_glossaryDatas = TGlossary.GroupTable[_model.FilterCategory].Where((TGlossary x) => x != null && (x.UnlockConditions == null || x.UnlockConditions.Length == 0 || x.UnlockConditions.All(_playerManager.CheckStateCondition))).OrderBy((TGlossary x) => LocalizationUtility.GetLocalizedText(string.Format("{0}_{1}_{2}", "TGlossary", "Name", x.Id))).ToArray();
		string[] texts = _glossaryDatas.Select((TGlossary x) => LocalizationUtility.GetLocalizedText(string.Format("{0}_{1}_{2}", "TGlossary", "Name", x.Id))).ToArray();
		_scrollerModel = new TextSlotScrollerModel
		{
			Texts = texts
		};
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(this);
		_scrollerModel.OnSlotExit.Subscribe(OnSlotExit).AddTo(this);
		_view.Scroller.GetComponent<TextSlotScrollerPresenter>().Inject(_scrollerModel);
		_view.BackButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBack();
		}).AddTo(this);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		Observable.EveryUpdate().Select((long _) => GameInputManager.UiPadActions.ScrollRect.ReadValue<Vector2>()).Subscribe(OnUpdateScroll)
			.AddTo(InputDisposable);
		GameInputManager gameInputManager = GameInputManager;
		if (gameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnBack();
			}).AddTo(InputDisposable);
			_scrollerModel.RxIsInputEnabled.Value = _selectedSlot == null;
		}
	}

	protected override void OnInputDisabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnUpdateScroll(Vector2 vector2)
	{
		if (Mathf.Abs(vector2.y) >= 0.1f)
		{
			ScrollRect glossary_Scroll = _view.Glossary_Scroll;
			glossary_Scroll.verticalNormalizedPosition += vector2.y * Time.deltaTime;
		}
	}

	private void OnBack()
	{
		if (_selectedSlot == null)
		{
			Close();
			return;
		}
		_selectedSlot.RxIsSelected.Value = false;
		_selectedSlot = null;
		if (IsInputEnable)
		{
			_scrollerModel.RxIsInputEnabled.Value = true;
		}
		TGlossary data = null;
		int? num = ((_focusedSlot != null) ? new int?(_focusedSlot.Index) : null);
		if (num.HasValue)
		{
			data = _glossaryDatas[num.Value];
		}
		ShowGlossary(data);
	}

	private void OnSlotClick(TextSlotModel slotModel)
	{
		if (_selectedSlot != null)
		{
			_selectedSlot.RxIsSelected.Value = false;
		}
		_selectedSlot = slotModel;
		slotModel.RxIsSelected.Value = true;
		if (IsInputEnable)
		{
			_scrollerModel.RxIsInputEnabled.Value = false;
		}
		ShowGlossary(_glossaryDatas[slotModel.Index]);
	}

	private void OnSlotEnter(TextSlotModel slotModel)
	{
		_focusedSlot = slotModel;
		if (_selectedSlot == null)
		{
			ShowGlossary(_glossaryDatas[slotModel.Index]);
		}
	}

	private void OnSlotExit(TextSlotModel slotModel)
	{
		_focusedSlot = null;
		if (_selectedSlot == null)
		{
			ShowGlossary(null);
		}
	}

	private void ShowGlossary(TGlossary data)
	{
		if (_selectedSlot == null)
		{
			ClearNav();
		}
		else
		{
			SetDefaultNav(_view.BackButton);
		}
		_view.Empty.SetActive(data == null);
		_view.Glossary.SetActive(data != null);
		_view.BackButton.gameObject.SetActive(data != null);
		if (data != null)
		{
			_view.Glossary_Title.GetComponentInChildren<TextMeshProUGUI>().text = data.Name;
			_view.Glossary_Text.text = data.Desc;
		}
	}
}
