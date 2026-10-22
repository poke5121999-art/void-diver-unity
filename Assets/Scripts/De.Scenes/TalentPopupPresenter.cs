using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TalentPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private TalentPopupModel _model;

	[View]
	private TalentPopupView _view;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	private Dictionary<(int SourceTalentId, int TargetTalentId), UINodeLink> _nodeLinks = new Dictionary<(int SourceTalentId, int TargetTalentId), UINodeLink>();

	private Dictionary<int, Transform> _talentSlotTfs = new Dictionary<int, Transform>();

	private Dictionary<int, TalentSlotModel> _talentSlotModels = new Dictionary<int, TalentSlotModel>();

	private TalentSlotModel _focusedSlot;

	private TalentSlotModel _selectedSlot;

	private TalentInfoPanelModel _infoPanelModel;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private int NumberOfSlotsEachRow => 5;

	public override string OpenSfx => "TalentOpen";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private void Start()
	{
		_view.TreeScroll.decelerationRate = Const.Scroller.DECELERATION_RATE;
		_view.TreeScroll.scrollSensitivity = Const.Scroller.SCROLL_SENSITIVITY;
	}

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Title_Text.text = _model.Title;
		InitInfoPanel();
		BuildTalentSlots();
		_infoPanelModel.TalentId = 0;
		_view.InfoPanel.Inject(_infoPanelModel);
		await UniTask.Yield();
		InitGamePadNav();
		_playerManager.RxTalent.Subscribe(OnTalentUpdated).AddTo(this);
	}

	private void InitInfoPanel()
	{
		_infoPanelModel = new TalentInfoPanelModel();
		_infoPanelModel.OutOfLeft.Subscribe(delegate
		{
			OnBack();
		}).AddTo(this);
		_view.InfoPanel.Inject(_infoPanelModel);
	}

	private void InitGamePadNav()
	{
		TTalent[] array = TTalent.GroupTable[_model.TalentGroupId];
		foreach (TTalent talentData in array)
		{
			int talentId = talentData.Id;
			Button componentInChildren = _talentSlotTfs[talentId].GetComponentInChildren<Button>();
			if (talentData.RootTalentIds != null && talentData.RootTalentIds.Length != 0)
			{
				int key = talentData.RootTalentIds.OrderBy((int id) => TTalent.Table[id].DisplayPoint[0]).First();
				ConnectNav(componentInChildren, ENavDir.Up, _talentSlotTfs[key].GetComponentInChildren<Button>());
			}
			TTalent tTalent = array.Where((TTalent d) => d.RootTalentIds.Contains(talentData.Id)).OrderBy((TTalent d) => d.DisplayPoint[0]).FirstOrDefault();
			if (tTalent != null)
			{
				ConnectNav(componentInChildren, ENavDir.Down, _talentSlotTfs[tTalent.Id].GetComponentInChildren<Button>());
			}
			TTalent tTalent2 = array.Where((TTalent d) => d.DisplayPoint[1] == talentData.DisplayPoint[1] && d.DisplayPoint[0] < talentData.DisplayPoint[0]).OrderByDescending((TTalent d) => d.DisplayPoint[0]).FirstOrDefault();
			if (tTalent2 != null)
			{
				ConnectNav(componentInChildren, ENavDir.Left, _talentSlotTfs[tTalent2.Id].GetComponentInChildren<Button>());
			}
			TTalent tTalent3 = array.Where((TTalent d) => d.DisplayPoint[1] == talentData.DisplayPoint[1] && d.DisplayPoint[0] > talentData.DisplayPoint[0]).OrderBy((TTalent d) => d.DisplayPoint[0]).FirstOrDefault();
			if (tTalent3 != null)
			{
				ConnectNav(componentInChildren, ENavDir.Right, _talentSlotTfs[tTalent3.Id].GetComponentInChildren<Button>());
			}
			componentInChildren.OnSelectAsObservable().Subscribe(delegate
			{
				OnButtonSelected(talentId);
			}).AddTo(this);
		}
		TTalent tTalent4 = array.OrderBy((TTalent d) => d.DisplayPoint[1]).ThenBy((TTalent d) => d.DisplayPoint[0]).First();
		SetDefaultNav(_talentSlotTfs[tTalent4.Id].GetComponentInChildren<Button>());
	}

	private void OnButtonSelected(int talentId)
	{
		Button componentInChildren = _talentSlotTfs[talentId].GetComponentInChildren<Button>();
		TalentSlotModel focusedSlot = _talentSlotModels[talentId];
		ScrollToSelectable(componentInChildren);
		_focusedSlot = focusedSlot;
		if (_selectedSlot == null)
		{
			UpdateInfo(_focusedSlot);
		}
	}

	private void ScrollToSelectable(Button button)
	{
		RectTransform content = _view.TreeScroll.content;
		RectTransform viewport = _view.TreeScroll.viewport;
		float height = content.rect.height;
		float height2 = viewport.rect.height;
		if (height <= height2)
		{
			return;
		}
		RectTransform rectTransform = button.transform as RectTransform;
		Vector3 vector = content.InverseTransformPoint(rectTransform.position);
		float num = 0f - vector.y - rectTransform.rect.height * 0.5f;
		float num2 = rectTransform.rect.height * 0.5f - vector.y;
		float num3 = height - height2;
		float num4 = (1f - _view.TreeScroll.verticalNormalizedPosition) * num3;
		if (!(num4 <= num) || !(num2 <= num4 + height2))
		{
			if (num4 <= num)
			{
				num = num2 - height2;
			}
			_view.TreeScroll.verticalNormalizedPosition = 1f - Mathf.Clamp01(num / num3);
		}
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			OnBack();
		}).AddTo(InputDisposable);
		if (_selectedSlot == null)
		{
			_infoPanelModel.RxIsInputEnabled.Value = false;
			return;
		}
		SuspendNav();
		_infoPanelModel.RxIsInputEnabled.Value = true;
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		_infoPanelModel.RxIsInputEnabled.Value = false;
	}

	private void OnBack()
	{
		if (_selectedSlot == null)
		{
			Close();
			return;
		}
		UpdateSelectedObject(null);
		UpdateInfo(_focusedSlot);
	}

	private void BuildTalentSlots()
	{
		TTalent[] array = TTalent.GroupTable[_model.TalentGroupId];
		int num = array.Max((TTalent t) => t.DisplayPoint[1] + 1);
		for (int i = 0; i < _view.SlotPoints.Length; i++)
		{
			_view.SlotPoints[i].SetActive(i < num * 5);
		}
		_talentSlotTfs = new Dictionary<int, Transform>();
		foreach (TTalent tTalent in array)
		{
			TalentSlotModel talentSlotModel = new TalentSlotModel();
			talentSlotModel.TalentId = tTalent.Id;
			talentSlotModel.OnClick.Subscribe(OnSlotClick).AddTo(this);
			talentSlotModel.OnEnter.Subscribe(OnSlotEnter).AddTo(this);
			talentSlotModel.OnExit.Subscribe(OnSlotExit).AddTo(this);
			Transform transform = InstantiateTalentSlot(tTalent);
			transform.GetComponent<TalentSlotPresenter>().Inject(talentSlotModel);
			_talentSlotModels.Add(tTalent.Id, talentSlotModel);
			_talentSlotTfs.Add(tTalent.Id, transform);
		}
		foreach (TTalent tTalent2 in array)
		{
			if (tTalent2.RootTalentIds == null || tTalent2.RootTalentIds.Length == 0)
			{
				continue;
			}
			int[] rootTalentIds = tTalent2.RootTalentIds;
			foreach (int num2 in rootTalentIds)
			{
				RectTransform sourceNodeTf = _talentSlotTfs[num2] as RectTransform;
				RectTransform targetNodeTf = _talentSlotTfs[tTalent2.Id] as RectTransform;
				UINodeLink uINodeLink = _assetManager.InstantiateNodeLink();
				uINodeLink.transform.SetParent(_view.LineRoot.transform, worldPositionStays: false);
				uINodeLink.sourceNodeTf = sourceNodeTf;
				uINodeLink.targetNodeTf = targetNodeTf;
				_nodeLinks.Add((num2, tTalent2.Id), uINodeLink);
			}
		}
		Transform InstantiateTalentSlot(TTalent talentData)
		{
			int[] displayPoint = talentData.DisplayPoint;
			Transform parent = _view.SlotPoints[displayPoint[1] * 5 + displayPoint[0]].transform;
			GameObject gameObject = _assetManager.InstantiateTalentSlot();
			gameObject.transform.SetParent(parent, worldPositionStays: false);
			RectTransform rectTransform = gameObject.transform as RectTransform;
			rectTransform.SetFullStretch();
			return rectTransform;
		}
	}

	private void UpdateNodeLinks()
	{
		TTalent[] array = TTalent.GroupTable[_model.TalentGroupId];
		foreach (TTalent tTalent in array)
		{
			if (tTalent.RootTalentIds == null || tTalent.RootTalentIds.Length == 0)
			{
				continue;
			}
			int[] rootTalentIds = tTalent.RootTalentIds;
			foreach (int num in rootTalentIds)
			{
				_nodeLinks[(num, tTalent.Id)].IsActivated = _playerManager.HasTalent(num);
			}
		}
	}

	private void UpdateSelectedObject(TalentSlotModel slotModel)
	{
		TalentSlotModel selectedSlot = _selectedSlot;
		if (selectedSlot != null)
		{
			selectedSlot.RxIsSelected.Value = false;
		}
		if (slotModel != null)
		{
			slotModel.RxIsSelected.Value = true;
		}
		_selectedSlot = slotModel;
		if (!IsInputEnable)
		{
			return;
		}
		if (_selectedSlot == null)
		{
			_infoPanelModel.RxIsInputEnabled.Value = false;
			ResumeNav();
			if (selectedSlot != null && InputManager.IsGamePad)
			{
				Button componentInChildren = _talentSlotTfs[selectedSlot.TalentId].GetComponentInChildren<Button>();
				InputManager.SelectObject(componentInChildren.gameObject);
			}
		}
		else
		{
			SuspendNav();
			_infoPanelModel.EntryDirection = ENavDir.Right;
			_infoPanelModel.RxIsInputEnabled.Value = true;
		}
	}

	private void UpdateInfo(TalentSlotModel slotModel)
	{
		_infoPanelModel.TalentId = slotModel?.TalentId ?? 0;
		_view.InfoPanel.Inject(_infoPanelModel);
	}

	private void OnTalentUpdated(Talent talent)
	{
		UpdateNodeLinks();
	}

	private void OnSlotEnter(TalentSlotModel slotModel)
	{
		_focusedSlot = slotModel;
	}

	private void OnSlotExit(TalentSlotModel slotModel)
	{
		if (_focusedSlot == slotModel)
		{
			_focusedSlot = null;
		}
	}

	private void OnSlotClick(TalentSlotModel slotModel)
	{
		UpdateSelectedObject(slotModel);
		UpdateInfo(slotModel);
	}
}
