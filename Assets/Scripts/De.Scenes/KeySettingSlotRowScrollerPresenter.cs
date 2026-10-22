using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using GameCommon.Table;

public class KeySettingSlotRowScrollerPresenter : GridScrollerPresenter<KeySettingSlotModel, KeySettingSlotRowModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[SerializeField]
	private int _slotsPerRow;

	[InjectModel]
	private KeySettingSlotRowScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<KeySettingSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return _slotsPerRow;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(KeySettingSlotRowScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		RefreshScroller();
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override KeySettingSlotModel[] BuildSlotModels()
	{
		EControlDevice controlDevice = _model.ControlDevice;
		if (controlDevice != EControlDevice.Keyboard && controlDevice != EControlDevice.GamePad)
		{
			return Array.Empty<KeySettingSlotModel>();
		}
		List<TInputAction> list = TInputAction.GroupTable[controlDevice].ToList();
		return InterleaveHalf(list).Select((TInputAction _) => new KeySettingSlotModel
		{
			InputActionId = _.Id
		}).ToArray();

		// TODO(decomp): generic local function InterleaveHalf has no body of its own in the binary (0x0); the order below (first half and second half taken alternately) is inferred from its name.
		static IEnumerable<T> InterleaveHalf<T>(IReadOnlyList<T> list)
		{
			int half = (list.Count + 1) / 2;
			for (int i = 0; i < half; i++)
			{
				yield return list[i];
				if (i + half < list.Count)
				{
					yield return list[i + half];
				}
			}
		}
	}

	protected override KeySettingSlotRowModel[] BuildRowModels(KeySettingSlotModel[] slots)
	{
		KeySettingSlotRowModel[] array = new KeySettingSlotRowModel[(int)Math.Ceiling((float)slots.Length / (float)_slotsPerRow)];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new KeySettingSlotRowModel
			{
				SlotModels = slots.Skip(i * _slotsPerRow).Take(_slotsPerRow).ToArray()
			};
		}
		return array;
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
