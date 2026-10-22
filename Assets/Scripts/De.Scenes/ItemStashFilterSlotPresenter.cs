using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ItemStashFilterSlotPresenter : BaseSlotPresenter<ItemStashFilterSlotModel>
{
	[InjectModel]
	private ItemStashFilterSlotModel _model;

	[View]
	private ItemStashFilterSlotView _view;

	protected override ItemStashFilterSlotModel SlotModel => _model;

	protected override Button Button => _view.Button;

	private void OnInject(ItemStashFilterSlotModel model)
	{
		InitSlot(model);
		_view.Text.text = model.InteractionName;
		model.RxButtonState.Subscribe(OnButtonLayoutChanged).AddTo(Disposable);
	}

	private void OnButtonLayoutChanged(EButtonState state)
	{
		Animator component = _view.Button.GetComponent<Animator>();
		if (component != null)
		{
			component.SetInteger("Layout", (int)state);
		}
	}
}
