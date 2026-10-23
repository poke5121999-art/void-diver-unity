using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class ParadoxPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private ParadoxPanelModel _model;

	[View]
	private ParadoxPanelView _view;

	private CompositeDisposable _disposable;

	private List<Button> _navButtons = new List<Button>();

	protected override BasePanelModel PanelModel => _model;

	private void OnInject(ParadoxPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		model.Player.RxCharacter.Subscribe(OnCharacterChanged).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void InitGamePadNav()
	{
		_navButtons = _view.Slots.Select((UnityEngine.GameObject s) => s.GetComponent<Button>()).ToList();
		SetNavVertical(_navButtons);
	}

	private void OnCharacterChanged(Character character)
	{
		int[] array = ((!_model.IsPositive) ? character.NegativeParadoxIds : character.PositiveParadoxIds) ?? Array.Empty<int>();
		_view.NoParadox.SetActive(!array.Any());
		_view.CountText.text = string.Format("{0}/{1}", array.Length, TConst.ParadoxCountMax.IntValue);
		for (int i = 0; i < _view.Slots.Length; i++)
		{
			int num = ((i < array.Length) ? array[i] : 0);
			ParadoxSimpleSlotModel paradoxSimpleSlotModel = new ParadoxSimpleSlotModel
			{
				ParadoxId = num,
				IsParadoxLocked = (num >= 1 && num == character.LockedParadoxId)
			};
			paradoxSimpleSlotModel.OnEnter.Subscribe(OnParadoxSlotEnter).AddTo(_disposable);
			paradoxSimpleSlotModel.OnExit.Subscribe(OnParadoxSlotExit).AddTo(_disposable);
			_view.Slots[i].SetActive(true);
			_view.Slots[i].GetComponent<ParadoxSimpleSlotPresenter>().Inject(paradoxSimpleSlotModel);
		}
		_navButtons = _view.Slots.Select((UnityEngine.GameObject s) => s.GetComponent<Button>()).ToList();
		ClearNav();
		SetNavVertical(_navButtons);
	}

	private void OnParadoxSlotEnter(ParadoxSimpleSlotModel slotModel)
	{
		_model.OnParadoxSlotEnter.OnNext(slotModel);
	}

	private void OnParadoxSlotExit(ParadoxSimpleSlotModel slotModel)
	{
		_model.OnParadoxSlotExit.OnNext(slotModel);
	}
}
