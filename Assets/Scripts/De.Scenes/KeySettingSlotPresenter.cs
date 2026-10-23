using GameCommon.Table;
using InputIcons;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class KeySettingSlotPresenter : BaseSlotPresenter<KeySettingSlotModel>
{
	[InjectModel]
	private KeySettingSlotModel _model;

	[View]
	private KeySettingSlotView _view;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private GamePlaySettingService _playSettingService;

	protected override KeySettingSlotModel SlotModel => _model;

	private void OnInject(KeySettingSlotModel model)
	{
		InitSlot(model);
		TInputAction data = TInputAction.Table[model.InputActionId];
		_playSettingService.RxLanguage.Subscribe(delegate
		{
			_view.Text.text = data.Name;
		}).AddTo(Disposable);
		_view.Locked.gameObject.SetActive(!data.IsRebindable);
		SetPromptAction(_view.Binding_KeyPrompt.GetComponent<II_ImagePrompt>(), data.ActionName, data.BindingIndex);
		bool flag = !string.IsNullOrEmpty(data.ModifierActionName);
		_view.Modifier.SetActive(flag);
		_view.Plus.SetActive(flag);
		if (flag)
		{
			SetPromptAction(_view.Modifier_KeyPrompt.GetComponent<II_ImagePrompt>(), data.ModifierActionName, data.ModifierBindingIndex);
		}
	}

	private void RefreshText(TInputAction data)
	{
		_view.Text.text = data.Name;
	}

	private void SetPromptAction(II_ImagePrompt prompt, string actionName, int bindingIndex)
	{
		if (prompt == null || prompt.spritePromptDatas.Count == 0)
		{
			return;
		}
		InputAction inputAction = _inputManager.FindAction(actionName);
		if (inputAction != null)
		{
			II_ImagePrompt.ImagePromptData imagePromptData = prompt.spritePromptDatas[0];
			imagePromptData.actionReference = InputActionReference.Create(inputAction);
			imagePromptData.bindingIndexKeyboard = bindingIndex;
			imagePromptData.bindingIndexGamepad = bindingIndex;
			prompt.UpdateDisplayedImages();
		}
	}
}
