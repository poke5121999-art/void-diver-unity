using System.Collections.Generic;
using System;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SkillSelectScrollerPresenter : VerticalScrollerPresenter<SkillSelectInfoSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[SerializeField]
	private RectTransform _measureRoot;

	[InjectModel]
	private SkillSelectScrollerModel _model;

	[Inject]
	private GameManager _gameManager;

	private readonly List<float> _cellHeights = new List<float>();

	private bool _measured;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<SkillSelectInfoSlotModel> ScrollerModel => _model;

	private GamePlayer Player => _gameManager.MyPlayer;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(SkillSelectScrollerModel model)
	{
		_cellHeights.Clear();
		_measured = false;
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		CellViewModels.Clear();
		CellViewModels.AddRange(BuildSlotModels());
		foreach (SkillSelectInfoSlotModel cellViewModel in CellViewModels)
		{
			cellViewModel.OnClick.Subscribe(OnClickSkillSelectInfoSlot).AddTo(_disposable);
		}
		Scroller.Delegate = this;
		Measure(model.FocusedIndex);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnSelect(int index)
	{
		base.OnSelect(index);
		_model.FocusedIndex = index;
		_model.ScrollPosition = Scroller.ScrollPosition;
	}

	private void OnClickSkillSelectInfoSlot(SkillSelectInfoSlotModel infoSlotModel)
	{
		_model.OnSlotClick.OnNext(infoSlotModel);
	}

	protected override List<SkillSelectInfoSlotModel> BuildSlotModels()
	{
		return Player.GetSelectableActiveSkillIds().OrderBy(delegate(int _)
		{
			foreach (TalentEffect talentEffect in Player.GetTalentEffects(ETalentEffectType.UnlockSkillMode))
			{
				if (talentEffect.Value2 == _)
				{
					return talentEffect.Value1;
				}
			}
			foreach (TalentEffect talentEffect2 in Player.GetTalentEffects(ETalentEffectType.ChangeSkill))
			{
				if (talentEffect2.Value2 == _)
				{
					return talentEffect2.Value1;
				}
			}
			return _;
		}).ThenBy((int _) => (_ != Player.GetBaseSkillId(_)) ? 1 : 0).ThenBy((int _) => _)
			.Select((int _) => new SkillSelectInfoSlotModel
			{
				SkillId = _,
				TrialIndex = _model.TrialIndex,
				EquippedIndex = Array.IndexOf(_model.EquippedSkillIds, _)
			})
			.ToList();
	}

	private void Measure(int initialIndex = 0)
	{
		PrepareMeasureRoot();
		Canvas.ForceUpdateCanvases();
		BuildHeights();
		_measured = true;
		Scroller.ReloadData();
		Scroller.SetScrollPositionImmediately(_model.ScrollPosition);
		RefreshNav(initialIndex);
	}

	private void PrepareMeasureRoot()
	{
		if (!(_measureRoot != null))
		{
			GameObject gameObject = new GameObject("_MeasureRoot", typeof(RectTransform));
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(base.transform, false);
			component.anchorMin = new Vector2(0f, 1f);
			component.anchorMax = new Vector2(0f, 1f);
			component.pivot = new Vector2(0f, 1f);
			component.anchoredPosition = new Vector2(99999f, 99999f);
			gameObject.hideFlags = HideFlags.DontSave;
			_measureRoot = component;
		}
	}

	private void BuildHeights()
	{
		_cellHeights.Clear();
		if (_cellViewPrefab == null || CellViewModels.Count == 0)
		{
			return;
		}
		foreach (SkillSelectInfoSlotModel cellViewModel in CellViewModels)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(_cellViewPrefab.gameObject, _measureRoot);
			gameObject.SetActive(true);
			gameObject.GetComponent<BaseCell>().SetData(cellViewModel);
			Canvas.ForceUpdateCanvases();
			RectTransform rectTransform = gameObject.transform as RectTransform;
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			float num = LayoutUtility.GetPreferredHeight(rectTransform);
			if (num <= 0f)
			{
				num = ((rectTransform.sizeDelta.y > 0f) ? rectTransform.sizeDelta.y : _cellSize);
			}
			_cellHeights.Add(num);
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		if (_cellHeights.Count != 0)
		{
			return;
		}
		for (int i = 0; i < CellViewModels.Count; i++)
		{
			_cellHeights.Add(_cellSize);
		}
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		if (_measured && dataIndex < _cellHeights.Count)
		{
			return _cellHeights[dataIndex];
		}
		return _cellSize;
	}
}
