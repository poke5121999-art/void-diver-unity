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

	private Button Button => null;

	private ButtonController ButtonController => null;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnInject(RoomDifficultySlotModel model)
	{
	}

	private void OnSlotClick()
	{
	}

	private void OnSelected(bool isSelected)
	{
	}
}
