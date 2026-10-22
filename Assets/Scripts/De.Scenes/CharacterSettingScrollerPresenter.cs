using System.Collections.Generic;
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

	protected override BaseScrollerModel<CharacterSettingSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(CharacterSettingScrollerModel model)
	{
	}

	protected override List<CharacterSettingSlotModel> BuildSlotModels()
	{
		return null;
	}

	private void OnSelectedCharacterIdChanged(int selectedCharacterId)
	{
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		return null;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 0f;
	}
}
