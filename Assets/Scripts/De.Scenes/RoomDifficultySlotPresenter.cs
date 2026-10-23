using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class RoomDifficultySlotPresenter : Presenter
{
	[View]
	private RoomDifficultySlotView _view;

	[InjectModel]
	private RoomDifficultySlotModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	[SerializeField]
	private Graphic[] _colorGraphics;

	private CompositeDisposable _disposable;

	private Button Button => _view.GetComponent<Button>();

	private ButtonController ButtonController => _view.GetComponent<Button>().GetComponent<ButtonController>();

	private void Start()
	{
		Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnSlotClick();
		}).AddTo(this);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void OnInject(RoomDifficultySlotModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		Sprite iconRoomDifficulty = _assetManager.GetIconRoomDifficulty(_model.RoomDifficulty);
		_view.Off_Icon.sprite = iconRoomDifficulty;
		_view.On_Icon.sprite = iconRoomDifficulty;
		_view.Off_Text.text = _model.RoomDifficulty.ToLocalText();
		_view.On_Text.text = _model.RoomDifficulty.ToLocalText();
		Color color = _model.RoomDifficulty.GetColor();
		Graphic[] colorGraphics = _colorGraphics;
		for (int i = 0; i < colorGraphics.Length; i++)
		{
			colorGraphics[i].color = color;
		}
		model.RxIsSelected.Subscribe(OnSelected).AddTo(_disposable);
	}

	private void OnSlotClick()
	{
		_model.OnSlotClick.OnNext(_model);
	}

	private void OnSelected(bool isSelected)
	{
		ButtonController.SetSelected(isSelected);
		_view.On_Select.SetActive(isSelected);
		int roomDifficulty = (int)_model.RoomDifficulty;
		if (roomDifficulty < 0 || roomDifficulty >= _view.On_Select_Vfx.Length)
		{
			_logger.Error(string.Format("[RoomDifficultySlotPresenter] On_Select_Vfx index {0} out of range (length: {1})", roomDifficulty, _view.On_Select_Vfx.Length));
			return;
		}
		for (int i = 0; i < _view.On_Select_Vfx.Length; i++)
		{
			_view.On_Select_Vfx[i].SetActive(isSelected && i == roomDifficulty);
		}
	}
}
