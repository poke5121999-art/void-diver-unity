using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class TalentTooltipPresenter : BasePanelPresenter
{
	[InjectModel]
	private TalentTooltipModel _model;

	[View]
	private TalentTooltipView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected void OnInject(TalentTooltipModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		TTalent tTalent = TTalent.Table[model.TalentId];
		_view.Name_Text.text = tTalent.Name;
		_view.Desc_Text.text = tTalent.Desc;
		_playerManager.RxTalent.Subscribe(OnTalentUpdated).AddTo(_disposable);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnTalentUpdated(Talent talent)
	{
		EUnlockState talentState = _playerManager.GetTalentState(_model.TalentId);
		_view.Unlocked.SetActive(talentState == EUnlockState.Unlocked);
		_view.Locked.SetActive(talentState == EUnlockState.Locked);
	}
}
