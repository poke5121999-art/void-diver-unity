using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using Spine.Unity;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TycoonMainPagePresenter : BasePanelPresenter
{
	[View]
	private TycoonMainPageView _view;

	[InjectModel]
	private TycoonMainPageModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameUiEventSystem _uiEventSystem;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameEventSystem _eventSystem;

	private SalesSlotScrollerModel _scrollerModel;

	private SalesSlotModel _selectedSlot;

	private CompositeDisposable _disposable;

	private readonly List<Button> _activeButtons = new List<Button>();

	private CompositeDisposable _inputDisposable;

	private CancellationTokenSource _skeletonCts;

	private bool _isButtonMode;

	private bool _suppressSlotPopup;

	protected override BasePanelModel PanelModel => _model;

	private SalesSlotScrollerPresenter _scrollerPresenter => _view.ScrollView.GetComponent<SalesSlotScrollerPresenter>();

	private SalesSlot SelectedSlot
	{
		get
		{
			if (_selectedSlot == null)
			{
				return null;
			}
			if (!_playerManager.SalesSlots.TryGetValue(_selectedSlot.Id, out var value))
			{
				return null;
			}
			return value;
		}
	}

	private Employee SelectedEmployee
	{
		get
		{
			SalesSlot selectedSlot = SelectedSlot;
			if (selectedSlot == null || selectedSlot.EmployeeId < 1)
			{
				return null;
			}
			if (!_playerManager.Employees.TryGetValue(selectedSlot.EmployeeId, out var value))
			{
				return null;
			}
			return value;
		}
	}

	private bool IsCurrentSlotStarted
	{
		get
		{
			SalesSlot selectedSlot = SelectedSlot;
			if (selectedSlot == null)
			{
				return false;
			}
			Employee selectedEmployee = SelectedEmployee;
			bool flag = selectedEmployee != null && selectedEmployee.IsPanic;
			return selectedSlot.IsFinished || flag || (selectedSlot.EmployeeId >= 1 && selectedSlot.Goods != null && selectedSlot.IsStarted);
		}
	}

	private void OnInject(TycoonMainPageModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
		_selectedSlot = null;
		ResetDisplay();
		_tycoonManager.CheckAndUnlockSalesSlots();
		_scrollerModel = new SalesSlotScrollerModel
		{
			SlotModels = BuildSalesSlotModels(),
			InitialSelectedId = _model.InitialSalesSlotId
		};
		_scrollerModel.OnSlotClick.Subscribe(OnSlotSelected).AddTo(_disposable);
		_suppressSlotPopup = true;
		_scrollerPresenter.Inject(_scrollerModel);
		SalesSlotModel salesSlotModel = _scrollerModel.SlotModels.Find((SalesSlotModel s) => s.Id == _model.InitialSalesSlotId);
		if (salesSlotModel == null)
		{
			salesSlotModel = _scrollerModel.SlotModels.FirstOrDefault();
		}
		if (salesSlotModel != null)
		{
			_scrollerModel.OnSlotClick.OnNext(salesSlotModel);
		}
		_suppressSlotPopup = false;
		_tycoonManager.OnEmployeesChanged.Subscribe(delegate
		{
			UpdateDisplay();
		}).AddTo(_disposable);
		_tycoonManager.OnSalesSlotsChanged.Subscribe(delegate
		{
			UpdateDisplay();
		}).AddTo(_disposable);
		_view.BtnSetArtifact.OnClickWithSfx().Subscribe(delegate
		{
			OnSetArtifactClick();
		}).AddTo(_disposable);
		_view.BtnSettingEmployee.OnClickWithSfx().Subscribe(delegate
		{
			OnSettingEmployeeClick();
		}).AddTo(_disposable);
		_view.BtnChangeEmployee.OnClickWithSfx().Subscribe(delegate
		{
			OnChangeEmployeeClick();
		}).AddTo(_disposable);
		_view.BtnCancelArtifact.OnClickWithSfx().Subscribe(delegate
		{
			OnCancelArtifactClick();
		}).AddTo(_disposable);
		_view.BtnStart.OnClickWithSfx().Subscribe(delegate
		{
			OnStartClick();
		}).AddTo(_disposable);
		_view.BtnResult.OnClickWithSfx().Subscribe(delegate
		{
			OnResultClick();
		}).AddTo(_disposable);
		_scrollerModel.OutOfRight.Subscribe(delegate
		{
			OnScrollerOutOfRight();
		}).AddTo(_disposable);
		model.OutOfLeft.Subscribe(delegate
		{
			OnButtonsOutOfLeft();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
		_popupManager.OnBeforeOpenPopup.Subscribe(delegate
		{
			ResetSpinePhysics();
		}).AddTo(_disposable);
		_popupManager.OnPopupClosed.Subscribe(delegate
		{
			ResetSpinePhysics();
		}).AddTo(_disposable);
	}

	protected override void OnDestroy()
	{
		_inputDisposable?.Dispose();
		_disposable?.Dispose();
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
		base.OnDestroy();
	}

	private void CancelSkeletonLoad()
	{
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
	}

	private void OnSlotSelected(SalesSlotModel slotModel)
	{
		if (_selectedSlot != null && _selectedSlot.Id == slotModel.Id)
		{
			return;
		}
		_selectedSlot = slotModel;
		_view.Text.text = TTycoonSalesSlot.Table[slotModel.Id].Name;
		UpdateDisplay();
		if (!_suppressSlotPopup)
		{
			TryOpenSlotPopup();
		}
	}

	private void TryOpenSlotPopup()
	{
		SalesSlot selectedSlot = SelectedSlot;
		if (selectedSlot != null && !selectedSlot.IsDisabledForTurn && !IsCurrentSlotStarted)
		{
			if (selectedSlot.Goods == null)
			{
				OpenArtifactSettingAsync().Forget();
			}
			else if (selectedSlot.EmployeeId < 1)
			{
				OpenEmployeeSettingAsync().Forget();
			}
		}
	}

	private void ResetDisplay()
	{
		_view.EmployeeSpine_Empty.SetActive(value: true);
		_view.EmployeeSpine_Skeleton.gameObject.SetActive(value: false);
		_view.EmployeeSpine_EmployeeInfo.SetActive(value: false);
		_view.EmployeeSpine_TextBubble.SetActive(value: false);
		_view.EmployeeSpine_TextBubblePanic.SetActive(value: false);
		_view.ArtifactInfo.SetActive(value: false);
		_view.ImgArtifact.gameObject.SetActive(value: false);
		_view.TextBubbleEmpty.SetActive(value: true);
		_view.TextBubbleComplete.SetActive(value: false);
		_view.ServingNow.SetActive(value: false);
		_view.ArtifactGradeVfx_Rare.SetActive(value: false);
		_view.ArtifactGradeVfx_Elite.SetActive(value: false);
		_view.ArtifactGradeVfx_Epic.SetActive(value: false);
		_view.ArtifactGradeVfx_Legend.SetActive(value: false);
		_view.ArtifactGradeVfx_Myth.SetActive(value: false);
	}

	private void ResetSpinePhysics()
	{
		Spine.Skeleton skeleton = _view.EmployeeSpine_Skeleton.Skeleton;
		if (skeleton == null || skeleton.PhysicsConstraints == null)
		{
			return;
		}
		for (int i = 0; i < skeleton.PhysicsConstraints.Count; i++)
		{
			skeleton.PhysicsConstraints.Items[i].Reset();
		}
	}

	private void UpdateDisplay()
	{
		SalesSlot selectedSlot = SelectedSlot;
		if (selectedSlot == null)
		{
			return;
		}
		bool isDisabledForTurn = selectedSlot.IsDisabledForTurn;
		_view.SlotInfo.SetActive(!isDisabledForTurn);
		_view.TextBreak.gameObject.SetActive(isDisabledForTurn);
		if (!isDisabledForTurn)
		{
			Employee selectedEmployee = SelectedEmployee;
			CancelSkeletonLoad();
			_skeletonCts = new CancellationTokenSource();
			UpdateEmployeeDisplayAsync(selectedSlot, selectedEmployee, _skeletonCts.Token).Forget();
			UpdateArtifactDisplay(selectedSlot, selectedEmployee);
			UpdateButtons(selectedSlot, selectedEmployee);
		}
	}

	private async UniTaskVoid UpdateEmployeeDisplayAsync(SalesSlot slot, Employee employee, CancellationToken ct)
	{
		int employeeId = slot.EmployeeId;
		_view.EmployeeSpine_Empty.SetActive(employeeId < 1);
		_view.EmployeeSpine_Skeleton.gameObject.SetActive(employeeId > 0);
		_view.EmployeeSpine_EmployeeInfo.SetActive(employeeId > 0);
		_view.EmployeeSpine_TextBubble.SetActive(value: false);
		_view.EmployeeSpine_TextBubblePanic.SetActive(value: false);
		if (employeeId < 1 || employee == null)
		{
			return;
		}
		TEmployee tEmployee = TEmployee.Table[slot.EmployeeId];
		int npcId = tEmployee.NpcId;
		string text = tEmployee.Name;
		_view.EmployeeSpine_EmployeeInfo_Layout.SetActive(!employee.IsPanic);
		_view.EmployeeSpine_EmployeeInfo_LayoutPanic.SetActive(employee.IsPanic);
		if (!employee.IsPanic)
		{
			_view.EmployeeSpine_EmployeeInfo_Layout_TextName.text = text;
			_view.EmployeeSpine_EmployeeInfo_Layout_TextLv.text = TLocalizedText.LevelFormat.Format(employee.Level);
		}
		else
		{
			_view.EmployeeSpine_EmployeeInfo_LayoutPanic_TextName.text = text;
			_view.EmployeeSpine_EmployeeInfo_LayoutPanic_TextLv.text = TLocalizedText.LevelFormat.Format(employee.Level);
			string randomText = GetRandomText(tEmployee.BubbleTextsOnFinishPanic);
			_view.EmployeeSpine_TextBubblePanic.SetActive(value: true);
			_view.EmployeeSpine_TextBubblePanic_Text.text = randomText;
			if (!string.IsNullOrEmpty(randomText))
			{
				_soundManager.PlaySfx(GetRandomSaySfx());
			}
		}
		PanicGaugeUtility.Apply(_view.EmployeeSpine_EmployeeInfo_PanicSlider, _view.EmployeeSpine_EmployeeInfo_PanicSlider_Gauge_img_back, _view.EmployeeSpine_EmployeeInfo_PanicSlider_Gauge_img_front, employee.Stress, employee.IsPanic);
		_view.EmployeeSpine_EmployeeInfo_PanicSlider_ConditionFace.sprite = _assetManager.GetIconEmployeeCondition(employee.Stress, employee.IsPanic);
		EmployeeExpUtility.Apply(_view.EmployeeSpine_EmployeeInfo_ExpSlider, employee.Level, employee.Exp);
		int? num = null;
		if (slot.Goods != null && !employee.IsPanic)
		{
			num = _tycoonManager.CalculateTurnsUntilPanic(employee, slot.Goods);
		}
		bool flag = num.HasValue && num.Value != int.MaxValue;
		_view.EmployeeSpine_EmployeeInfo_TextDesc.gameObject.SetActive(flag);
		if (flag)
		{
			_view.EmployeeSpine_EmployeeInfo_TextDesc.text = string.Format(TLocalizedText.TycoonEmployeePanicFormat.LocalText, num.Value);
		}
		UpdateBubbleText(slot, employee, tEmployee);
		await SetEmployeeSkeletonAsync(_view.EmployeeSpine_Skeleton, npcId, slot.EmployeeId, ct);
		if (!ct.IsCancellationRequested)
		{
			string animationName = (IsCurrentSlotStarted ? _tycoonManager.GetEmployeeAnimationName(slot.EmployeeId) : "idle");
			_view.EmployeeSpine_Skeleton.AnimationState.SetAnimation(0, animationName, loop: true);
		}
	}

	private void UpdateBubbleText(SalesSlot slot, Employee employee, TEmployee employeeData)
	{
		if (employee == null || employee.IsPanic)
		{
			return;
		}
		Goods goods = slot.Goods;
		bool flag = goods != null && _tycoonManager.CalculateTurnsUntilPanic(employee, goods) < 2;
		string text = null;
		bool flag2 = slot.EmployeeId >= 1 && slot.Goods != null && slot.IsStarted && !slot.IsFinished;
		if (employeeData != null)
		{
			string[] texts = ((!slot.IsFinished) ? ((!flag2) ? (flag ? employeeData.BubbleTextsOnReadyToSellHighStress : employeeData.BubbleTextsOnReadyToSell) : (flag ? employeeData.BubbleTextsOnSellingHighStress : employeeData.BubbleTextsOnSelling)) : (flag ? employeeData.BubbleTextsOnFinishHighStress : employeeData.BubbleTextsOnFinish));
			text = GetRandomText(texts);
			if (!string.IsNullOrEmpty(text))
			{
				_view.EmployeeSpine_TextBubble.SetActive(value: true);
				_view.EmployeeSpine_TextBubble_Text.text = text;
				_soundManager.PlaySfx(GetRandomSaySfx());
			}
		}
	}

	private static string GetRandomText(string[] texts)
	{
		if (texts == null || texts.Length == 0)
		{
			return null;
		}
		return texts[Random.Range(0, texts.Length)];
	}

	private static string GetRandomSaySfx()
	{
		return Random.Range(0, 3) switch
		{
			0 => "Employee_Say1",
			1 => "Employee_Say2",
			_ => "Employee_Say3",
		};
	}

	private async UniTask SetEmployeeSkeletonAsync(SkeletonGraphic skeleton, int npcId, int employeeId, CancellationToken ct)
	{
		UnitSkeletonData unitSkeletonData = await _assetManager.GetNpcSkeletonDataAsync(npcId);
		if (!ct.IsCancellationRequested && !(unitSkeletonData == null))
		{
			skeleton.AnimationState.ClearTracks();
			skeleton.skeletonDataAsset = unitSkeletonData.Nw;
			skeleton.Initialize(overwrite: true);
			skeleton.Skeleton.SetSkin(employeeId.ToString());
			skeleton.Skeleton.SetSlotsToSetupPose();
			skeleton.Skeleton.UpdateCache();
		}
	}

	private void UpdateArtifactDisplay(SalesSlot slot, Employee employee)
	{
		Goods goods = slot.Goods;
		bool flag = goods != null;
		bool flag2 = employee != null && employee.IsPanic;
		bool isFinished = slot.IsFinished;
		int employeeId = slot.EmployeeId;
		bool flag3 = flag && employeeId > 0 && slot.IsStarted && !slot.IsFinished && !flag2;
		_view.ArtifactInfo.SetActive(flag);
		_view.TextBubbleEmpty.SetActive(goods == null && employeeId < 1);
		_view.TextBubbleComplete.SetActive(isFinished && !flag2);
		_view.ImgArtifact.gameObject.SetActive(flag);
		_view.ServingNow.SetActive(flag3);
		if (flag && isFinished && !flag2)
		{
			UpdateCompleteBubbleGold(slot, employee);
		}
		if (goods == null)
		{
			_view.ArtifactGradeVfx_Rare.SetActive(value: false);
			_view.ArtifactGradeVfx_Elite.SetActive(value: false);
			_view.ArtifactGradeVfx_Epic.SetActive(value: false);
			_view.ArtifactGradeVfx_Legend.SetActive(value: false);
			_view.ArtifactGradeVfx_Myth.SetActive(value: false);
			return;
		}
		TEquipment tEquipment = TEquipment.Table[goods.Id];
		_view.ArtifactInfo_TextArtifactName.text = tEquipment.Name;
		EGoodsGradeType grade = tEquipment.Grade;
		_view.ArtifactInfo_GradeLayout.SetActive(grade != EGoodsGradeType.None);
		if (grade != EGoodsGradeType.None)
		{
			_view.ArtifactInfo_GradeLayout_Bg.color = grade.ToColor(_view.ArtifactInfo_GradeLayout_Bg.color.a);
			_view.ArtifactInfo_GradeLayout_Text.color = grade.ToColor(_view.ArtifactInfo_GradeLayout_Text.color.a);
			_view.ArtifactInfo_GradeLayout_Text.text = grade.ToLocalText();
		}
		float num = slot.MaxWorkProgress;
		if (num <= 0f)
		{
			num = _tycoonManager.CalculatePreviewMaxWorkProgress(goods);
		}
		if (num > 0f)
		{
			_view.ArtifactInfo_SellSlider.gameObject.SetActive(value: true);
			_view.ArtifactInfo_SellSlider.value = 1f - slot.WorkProgress / num;
		}
		else
		{
			_view.ArtifactInfo_SellSlider.gameObject.SetActive(value: false);
		}
		_view.ImgArtifact.sprite = _assetManager.GetIconEquipment(goods.Id);
		UpdateArtifactGradeVfx(tEquipment.Grade);
		if (employeeId > 0 && employee != null && !slot.IsFinished && !flag2)
		{
			int num2 = _tycoonManager.CalculateTurnsUntilComplete(slot, employee);
			_view.ArtifactInfo_TextDesc.text = string.Format(TLocalizedText.TycoonArtifactSellFormat.LocalText, num2);
		}
		else
		{
			_view.ArtifactInfo_TextDesc.text = string.Empty;
		}
	}

	private void UpdateArtifactGradeVfx(EGoodsGradeType grade)
	{
		_view.ArtifactGradeVfx_Rare.SetActive(grade == EGoodsGradeType.Rare);
		_view.ArtifactGradeVfx_Elite.SetActive(grade == EGoodsGradeType.Elite);
		_view.ArtifactGradeVfx_Epic.SetActive(grade == EGoodsGradeType.Epic);
		_view.ArtifactGradeVfx_Legend.SetActive(grade == EGoodsGradeType.Legend);
		_view.ArtifactGradeVfx_Myth.SetActive(grade == EGoodsGradeType.Unique);
	}

	private void UpdateCompleteBubbleGold(SalesSlot slot, Employee employee)
	{
		int intValue = TConst.CoinToGoldRate.IntValue;
		if (employee == null)
		{
			if (slot.Goods != null)
			{
				_view.TextBubbleComplete_GoldText_Value.text = (slot.Goods.SellPriceTotal / intValue).ToString();
			}
		}
		else
		{
			long num = _tycoonManager.CalculateExpectedSellGold(slot.Goods, employee);
			_view.TextBubbleComplete_GoldText_Value.text = (num / intValue).ToString();
		}
	}

	private void UpdateButtons(SalesSlot slot, Employee employee)
	{
		int employeeId = slot.EmployeeId;
		Goods goods = slot.Goods;
		bool flag = employee != null && employee.IsPanic;
		bool flag2 = employeeId > 0 && goods != null && slot.IsStarted && !slot.IsFinished && !flag;
		bool isFinished = slot.IsFinished;
		_view.BtnSetArtifact.gameObject.SetActive(goods == null && !flag && !isFinished && !flag2);
		_view.BtnSettingEmployee.gameObject.SetActive(!flag && !isFinished && !flag2);
		_view.BtnChangeEmployee.gameObject.SetActive(flag2 && employeeId > 0 && !flag && !isFinished);
		_view.BtnCancelArtifact.gameObject.SetActive(goods != null && !flag && !isFinished);
		_view.BtnStart.gameObject.SetActive(goods != null && employeeId > 0 && !slot.IsStarted && !flag && !isFinished);
		_view.BtnResult.gameObject.SetActive(flag || isFinished);
		_view.ServingNow.SetActive(flag2);
		RefreshButtonNav();
	}

	private void OnSetArtifactClick()
	{
		OpenArtifactSettingAsync().Forget();
	}

	private async UniTaskVoid OpenArtifactSettingAsync()
	{
		if (SelectedSlot == null)
		{
			return;
		}
		TycoonArtifactSettingPopupPresenter tycoonArtifactSettingPopupPresenter = await _popupManager.OpenAsync<TycoonArtifactSettingPopupPresenter>(new TycoonArtifactSettingPopupModel
		{
			SlotIndex = _selectedSlot.Id
		});
		if (await tycoonArtifactSettingPopupPresenter.Task is InventoryGoodsSlotModel inventoryGoodsSlotModel)
		{
			await _tycoonManager.RegisterArtifactAsync(SelectedSlot, inventoryGoodsSlotModel.SlotCategory, inventoryGoodsSlotModel.SlotIndex);
		}
		UpdateDisplay();
	}

	private void OnSettingEmployeeClick()
	{
		OpenEmployeeSettingAsync().Forget();
	}

	private async UniTaskVoid OpenEmployeeSettingAsync()
	{
		SalesSlot selectedSlot = SelectedSlot;
		EmployeeSettingPopupPresenter employeeSettingPopupPresenter = await _popupManager.OpenAsync<EmployeeSettingPopupPresenter>(new EmployeeSettingPopupModel
		{
			SalesSlotIndex = _selectedSlot.Id,
			Artifact = selectedSlot.Goods,
			IsStarted = IsCurrentSlotStarted,
			CachedEmployeeId = selectedSlot.EmployeeId
		});
		object obj = await employeeSettingPopupPresenter.Task;
		if (!IsCurrentSlotStarted && obj is int num && num > 0 && _playerManager.Employees.TryGetValue(num, out var value))
		{
			_tycoonManager.TrySetEmployee(SelectedSlot, value);
		}
		UpdateDisplay();
	}

	private void OnChangeEmployeeClick()
	{
		OpenEmployeeSettingAsync().Forget();
	}

	private void OnCancelArtifactClick()
	{
		CancelArtifactAsync().Forget();
	}

	private async UniTaskVoid CancelArtifactAsync()
	{
		if (_selectedSlot == null)
		{
			return;
		}
		SalesSlot selectedSlot = SelectedSlot;
		if (selectedSlot == null || selectedSlot.Goods == null)
		{
			return;
		}
		if (!_gameManager.MyInventory.CanAddGoods(selectedSlot.Goods))
		{
			_toastManager.NoticeFail(TLocalizedText.InventoryFull.LocalText);
			return;
		}
		TycoonCancelConfirmPopupPresenter tycoonCancelConfirmPopupPresenter = await _popupManager.OpenAsync<TycoonCancelConfirmPopupPresenter>();
		if (await tycoonCancelConfirmPopupPresenter.Task is bool flag && flag)
		{
			await _tycoonManager.CancelSalesSlotAsync(_selectedSlot.Id);
		}
	}

	private void OnStartClick()
	{
		if (SelectedSlot != null)
		{
			_tycoonManager.StartSalesSlot(SelectedSlot);
		}
	}

	private void OnResultClick()
	{
		ProcessResultAsync().Forget();
	}

	private async UniTaskVoid ProcessResultAsync()
	{
		TycoonFinishResult result = await _tycoonManager.ProcessFinishedSlotAsync(_selectedSlot.Id);
		TriggerNegotiationTycoonAchievement(result);
		TycoonResultPopupPresenter tycoonResultPopupPresenter = await _popupManager.OpenAsync<TycoonResultPopupPresenter>(new TycoonResultPopupModel
		{
			IsPanic = result.IsPanic,
			IsSold = result.IsSold,
			EmployeeId = result.EmployeeId,
			Stress = result.Stress,
			Level = result.EmployeeLevel,
			EmployeeExp = result.EmployeeExp,
			EmployeeExpGain = result.EmployeeExpGain,
			Artifact = result.Artifact,
			ResultCoin = result.ResultCoin,
			ResultExp = result.ResultExp,
			FailType = result.FailType,
			SlotIndex = _selectedSlot.Id,
			StressSkillIds = result.StressSkillIds,
			PriceSkillIds = result.PriceSkillIds,
			SkillPriceBonusPercent = result.SkillPriceBonusPercent,
			SkillCoinBonus = result.SkillCoinBonus
		});
		await tycoonResultPopupPresenter.Task;
		if (result.IsLevelUp)
		{
			EmployeeLevelUpPopupPresenter employeeLevelUpPopupPresenter = await _popupManager.OpenAsync<EmployeeLevelUpPopupPresenter>(new EmployeeLevelUpPopupModel
			{
				EmployeeId = result.EmployeeId,
				Stress = result.Stress,
				PrevLevel = result.PrevLevel,
				NewLevel = result.NewLevel,
				Exp = result.EmployeeExp,
				LearnedSkillIds = result.LearnedSkillIds,
				NewlyLearnedSkillIds = result.NewlyLearnedSkillIds
			});
			await employeeLevelUpPopupPresenter.Task;
		}
		_uiEventSystem.FlushLevelAndExp();
	}

	private void TriggerNegotiationTycoonAchievement(TycoonFinishResult result)
	{
		if (result != null && result.Artifact != null)
		{
			_eventSystem.Trigger(new TycoonFinishEvent(result.IsPanic, result.EmployeeId, result.Artifact, result.ResultCoin));
		}
	}

	private List<SalesSlotModel> BuildSalesSlotModels()
	{
		return _playerManager.GetUnlockedSalesSlotIndices().Where((int idx) => _playerManager.SalesSlots.ContainsKey(idx)).Select((int idx) => new SalesSlotModel
		{
			Id = idx
		}).ToList();
	}

	protected override void OnInputEnabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = true;
		base.OnInputEnabled();
		SuspendNav();
		_inputDisposable?.Dispose();
		_inputDisposable = new CompositeDisposable();
		InputManager.UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			_model.OnEscape.OnNext(Unit.Default);
		}).AddTo(_inputDisposable);
	}

	protected override void OnInputDisabled()
	{
		_inputDisposable?.Dispose();
		_inputDisposable = null;
		_isButtonMode = false;
		if (_scrollerModel != null)
		{
			_scrollerModel.RxIsInputEnabled.Value = false;
		}
		base.OnInputDisabled();
	}

	private void OnScrollerOutOfRight()
	{
		if (_activeButtons.Count != 0)
		{
			_isButtonMode = true;
			_scrollerModel.RxIsInputEnabled.Value = false;
			RefreshButtonNav();
			SetDefaultNav(_activeButtons[0]);
			ResumeNav();
		}
	}

	private void OnButtonsOutOfLeft()
	{
		_isButtonMode = false;
		SuspendNav();
		_scrollerModel.RxIsInputEnabled.Value = true;
	}

	private void RefreshButtonNav()
	{
		ClearNav();
		_activeButtons.Clear();
		Button[] array = new Button[6] { _view.BtnSetArtifact, _view.BtnCancelArtifact, _view.BtnSettingEmployee, _view.BtnChangeEmployee, _view.BtnStart, _view.BtnResult };
		foreach (Button button in array)
		{
			if (button.gameObject.activeSelf)
			{
				_activeButtons.Add(button);
			}
		}
		if (_activeButtons.Count == 0)
		{
			if (_isButtonMode)
			{
				_isButtonMode = false;
				SuspendNav();
				_scrollerModel.RxIsInputEnabled.Value = true;
			}
		}
		else
		{
			SetNavHorizontal(_activeButtons);
			if (_isButtonMode)
			{
				SetDefaultNav(_activeButtons[0]);
			}
		}
	}
}
