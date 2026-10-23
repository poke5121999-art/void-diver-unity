using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class MenuBuffPanelPresenter : BasePanelPresenter
{
	[View]
	private MenuBuffPanelView _view;

	[InjectModel]
	private MenuBuffPanelModel _model;

	[Inject]
	private GameManager _gameManager;

	private MenuBuffScrollerModel _scrollerModel;

	private EmptyPanelModel _emptyPanelModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(MenuBuffPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			Logger.Warning("targetClientId is not found in PlayerCharacters");
			return;
		}
		_scrollerModel = new MenuBuffScrollerModel
		{
			CharacterModel = myCharacterModel
		};
		_scrollerModel.RxVisibleCount.Subscribe(OnVisibleCountChanged).AddTo(_disposable);
		_scrollerModel.OnSlotEnter.Subscribe(_model.OnSlotEnter).AddTo(_disposable);
		_scrollerModel.OnSlotExit.Subscribe(_model.OnSlotExit).AddTo(_disposable);
		_scrollerModel.OutOfTop.Subscribe(delegate
		{
			TriggerOutOfTop();
		}).AddTo(_disposable);
		_scrollerModel.OutOfBottom.Subscribe(delegate
		{
			TriggerOutOfBottom();
		}).AddTo(_disposable);
		_view.Scroller.GetComponent<MenuBuffScrollerPresenter>().Inject(_scrollerModel);
		_emptyPanelModel = new EmptyPanelModel
		{
			Text = TLocalizedText.NoAvailableGoods.LocalText
		};
		_emptyPanelModel.OutOfTop.Subscribe(delegate
		{
			TriggerOutOfTop();
		}).AddTo(_disposable);
		_emptyPanelModel.OutOfBottom.Subscribe(delegate
		{
			TriggerOutOfBottom();
		}).AddTo(_disposable);
		_view.EmptyPanel.Inject(_emptyPanelModel);
		_model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		_model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		int value = _scrollerModel.RxVisibleCount.Value;
		_emptyPanelModel.RxIsInputEnabled.Value = value < 1;
		_scrollerModel.RxIsInputEnabled.Value = value > 0;
	}

	protected override void OnInputDisabled()
	{
		_emptyPanelModel.RxIsInputEnabled.Value = false;
		_scrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnVisibleCountChanged(int count)
	{
		_view.EmptyPanel.gameObject.SetActive(count < 1);
	}
}
