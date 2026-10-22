using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;

public class CharacterSettingScrollerPresenter : HorizontalScrollerPresenter<CharacterSettingSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private CharacterSettingScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<CharacterSettingSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(CharacterSettingScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		RefreshScroller();
		model.RxSelectedCharacterId.Subscribe(OnSelectedCharacterIdChanged).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override List<CharacterSettingSlotModel> BuildSlotModels()
	{
		List<CharacterSettingSlotModel> list = _model.Characters.Select((Character ch) => new CharacterSettingSlotModel
		{
			CharacterId = ch.Id
		}).ToList();
		list.ForEach(delegate(CharacterSettingSlotModel s)
		{
			s.RxIsSelected.Value = s.CharacterId == _model.RxSelectedCharacterId.Value;
		});
		return list.Concat(Enumerable.Range(0, 2).Select((int _) => new CharacterSettingSlotModel
		{
			CharacterId = -1
		}).ToList()).ToList();
	}

	private void OnSelectedCharacterIdChanged(int selectedCharacterId)
	{
		CellViewModels.ForEach(delegate(CharacterSettingSlotModel s)
		{
			s.RxIsSelected.Value = s.CharacterId == selectedCharacterId;
		});
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return _cellSize;
	}
}
